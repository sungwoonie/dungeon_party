using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Manager : SingleTon<Dungeon_Manager>
{
    public bool stage_mode;

    private bool in_dungeon;

    private int kill_count;
    private Dungeon_Type current_dungeon_type;

    private Dungeon_Reward_Manager dungeon_reward_manager;

    private int boost_amount;

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
        dungeon_reward_manager = GetComponent<Dungeon_Reward_Manager>();
    }

    #endregion

    #region "Enter Exit"

    public void Enter_Dungeon(Dungeon_Type dungeon_type, int boost_amount, bool stage_mode)
    {
        Quest_Manager.instance.Increase_Requirement("enter_dungeon", 1);
        Battle_Pass_Manager.instance.Get_Requirement("dungeon", 1);

        this.stage_mode = stage_mode;

        this.boost_amount = boost_amount;
        StartCoroutine(Set_Dungeon(dungeon_type));
    }

    public void Exit_Dungeon()
    {
        StartCoroutine(Dungeon_Out());
    }

    public IEnumerator Set_Dungeon(Dungeon_Type dungeon_type)
    {
        in_dungeon = true;
        current_dungeon_type = dungeon_type;

        Event_Bus.Publish(Game_State.Stop);

        yield return StartCoroutine(Fade.instance.Fade_In());

        string dungeon_localization_key = $"{dungeon_type}_Dungeon_Title";
        Stage_Manager.instance.Set_Stage_Text_To_Localization(dungeon_localization_key);
        Stage_Gage.instance.Set_Slider_To_Timer(60, 60);

        Map_Scroller.instance.Change_Map((int)dungeon_type);

        Character_Manager.instance.Set_All_Characters_To_Offset_Position();
        Character_Manager.instance.Set_Characters_To_Max_Health();

        Monster_Spawner.instance.Remove_All_Spawned_Monsters();

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fade.instance.Fade_Out());

        Event_Bus.Publish(Game_State.Spawn_Dungeon);
        StartCoroutine(Set_Timer());
    }

    public IEnumerator Dungeon_Out()
    {
        in_dungeon = false;

        Event_Bus.Publish(Game_State.Stop);

        yield return StartCoroutine(Fade.instance.Fade_In());

        Stage_Manager.instance.Set_Text_To_Current_Stage();

        Map_Scroller.instance.Change_Map(Stage_Manager.instance.Get_Current_Stage()[0] - 1);

        Character_Manager.instance.Set_All_Characters_To_Offset_Position();
        Character_Manager.instance.Set_Characters_To_Max_Health();

        Monster_Spawner.instance.Remove_All_Spawned_Monsters();

        Stage_Gage.instance.Set_Before(true);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fade.instance.Fade_Out());

        //reward
        dungeon_reward_manager.Get_Dungeon_Reward(kill_count, current_dungeon_type, boost_amount);
        Event_Bus.Publish(Game_State.Spawn_Normal);

        kill_count = 0;
    }

    #endregion

    #region "Sweep"

    public void Sweep_Dungeon(Dungeon_Type dungeon_type)
    {
        current_dungeon_type = dungeon_type;

        dungeon_reward_manager.Get_Sweep_Reward(current_dungeon_type);
    }

    #endregion

    #region "Timer"

    private IEnumerator Set_Timer()
    {
        float current_time = 60;
        float max_time = current_time;

        while (current_time > 0 && in_dungeon)
        {
            if (!Character_Manager.instance.Is_All_Dead())
            {
                current_time -= Time.deltaTime * Game_Time.game_time;
                Stage_Gage.instance.Set_Slider_To_Timer(current_time, (int)max_time);
            }

            yield return null;
        }

        if (in_dungeon)
        {
            Exit_Dungeon();
        }
    }

    #endregion

    #region "Kill Count"

    public void Kill_Count_Up()
    {
        kill_count++;
    }

    #endregion

    #region "Get"

    public bool In_Dungeon()
    {
        return in_dungeon;
    }

    #endregion
}