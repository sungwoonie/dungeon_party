using UnityEngine;

public class Stage_Manager : SingleTon<Stage_Manager>
{
    public Localization_Text stage_text;

    private int current_floor = 1;
    private int current_stage = 1;

    private int high_floor;
    private int high_stage;

    private Stage_Reward_Manager stage_reward_manager;
    private Stage_Gage stage_gage;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        stage_reward_manager = GetComponent<Stage_Reward_Manager>();
        stage_gage = GetComponent<Stage_Gage>();
    }

    public void Initialize_Data(Server_Stage_Data datas)
    {
        current_floor = datas.s[0];
        current_stage = datas.s[1];

        high_floor = datas.hs[0];
        high_stage = datas.hs[1];

        stage_gage.Set_Stage_Gage(datas.g);

        Set_Stage(current_floor, current_stage);
        Map_Scroller.instance.Change_Map(current_floor - 1);
    }

    #endregion

    #region "Set"

    public void Set_Stage_Text_To_Localization(string key)
    {
        stage_text.Set_Localization_Key(key);
        stage_text.Localize_Text();
    }

    public void Set_Stage_Text(string text)
    {
        stage_text.Set_Text(text);
    }

    public void Set_Text_To_Current_Stage()
    {
        Set_Stage_Text($"STAGE {current_floor} - {current_stage}");
    }

    public void Set_Stage(int floor, int stage)
    {
        if (floor > 99)
        {
            floor = 99;
            stage = 100;
            Error_Message.instance.Set_Error_Message("Error_Message_Max_Stage"); //변경해야됨 무한으로
        }

        current_floor = floor;
        current_stage = stage;

        if (current_stage > high_stage)
        {
            high_stage = current_stage;
        }

        if (current_floor > high_floor)
        {
            high_floor = current_floor;
            high_stage = 1;
        }


        Set_Stage_Text($"STAGE {current_floor} - {current_stage}");

        Save_Data();
    }

    #endregion

    #region "Get"

    public int[] Get_High_Stage()
    {
        int[] stage = { high_floor, high_stage };
        return stage;
    }

    public int[] Get_Current_Stage()
    {
        int[] stage = { current_floor, current_stage };

        return stage;
    }

    public bool Can_Spawn_Boss()
    {
        return stage_gage.Can_Spawn_Boss() && !Dungeon_Manager.instance.In_Dungeon();
    }

    #endregion

    #region "Up"

    public void Stage_Gage_Up()
    {
        int[] stage = Get_Current_Stage();

        Reward_Struct new_reward = stage_reward_manager.Get_Reward(stage, 1.0f, 1, 1);
        new_reward.Get_Reward();

        stage_gage.Stage_Gage_Up();
    }

    public void Stage_Up()
    {
        int[] stage = Get_Current_Stage();

        if (current_stage + 1 > 100)
        {
            Audio_Controller.instance.Play_Audio(1, "Floor_Clear");
            Reward_Struct new_reward = stage_reward_manager.Get_Reward(stage, 10.0f, 8, 7, 100);

            Reward_Pop_Up.instance.Set_Reward_Pop_Up(Reward_State.Floor, new_reward, current_floor);

            Floor_Up();
            return;
        }
        else
        {
            Audio_Controller.instance.Play_Audio(1, "Stage_Clear");
            Reward_Struct new_reward = stage_reward_manager.Get_Reward(stage, 5.0f, 5, 3, 10);

            Reward_Pop_Up.instance.Set_Reward_Pop_Up(Reward_State.Stage, new_reward, current_stage);

            Set_Stage(current_floor, ++current_stage);

            Event_Bus.Publish(Game_State.Spawn_Normal);

            Debug_Manager.Debug_In_Game_Message($"Stage Up to {current_stage}");
        }
    }

    public void Floor_Up()
    {
        Set_Stage(++current_floor, 1);

        Event_Bus.Publish(Game_State.Floor_Change);

        Debug_Manager.Debug_In_Game_Message($"Floor Up to {current_floor}");
    }

    #endregion

    #region "Save Data"

    public void Save_Data()
    {
        Server_Stage_Data save_data = new Server_Stage_Data();
        save_data.s = Get_Current_Stage();
        save_data.hs = Get_High_Stage();
        save_data.g = stage_gage.Get_Gage();

        Anti_Cheat_Manager.instance.Set("Stage", JsonUtility.ToJson(save_data));
    }

    #endregion
}