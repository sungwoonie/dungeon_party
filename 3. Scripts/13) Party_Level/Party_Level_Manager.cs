using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Party_Level_Manager : SingleTon<Party_Level_Manager>
{
    public TMP_Text level_text;

    private int current_party_level = 1;

    private Party_Experience party_experience;
    private Level_Up_Reward level_up_reward;

    private Level_Data current_level_data;
    private Server_Level_Data data;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Set Data From Server"

    public void Initialize_Data(Server_Level_Data server_data)
    {
        data = server_data;

        current_party_level = data.l;

        Set_Party_Level(current_party_level);
        party_experience.Set_Experience_Point(data.e.FromCurrencyString());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        party_experience = GetComponent<Party_Experience>();
        level_up_reward = GetComponent<Level_Up_Reward>();
    }

    #endregion

    #region "Set Level"

    public void Set_Party_Level(int party_level)
    {
        current_party_level = party_level;
        current_level_data = level_up_reward.Get_Current_Level_Data(current_party_level);

        level_text.text = $"Lv. {current_party_level}";

        party_experience.Set_Requirement_Experience_Point(current_level_data.requirement_experience_point);
    }

    #endregion

    #region "Get"

    public int Get_Party_Level()
    {
        return current_party_level;
    }

    #endregion

    #region "Get Experience Point"

    public void Get_Experience_Point(double experience_point)
    {
        if(experience_point <= 0)
        {
            return;
        }

        double result_experience_point = experience_point + (experience_point * Stat_Manager.instance.Calculate_Stat(41) * 0.01f);

        party_experience.Get_Experience_Point(result_experience_point);
        Reward_Label_Controller.instance.Set_Reward("experience_point", result_experience_point.ToCurrencyString());

        Save_Data();
    }

    #endregion

    #region "Level Up"

    public void Level_Up(double remain_experience_point)
    {
        Debug_Manager.Debug_In_Game_Message($"Level up. Current level is {current_party_level}. Level up to {current_party_level + 1}");
        Audio_Controller.instance.Play_Audio(1, "Level_Up");
        Reward_Struct reward = level_up_reward.Get_Level_Up_Reward(current_level_data);

        Reward_Pop_Up.instance.Set_Reward_Pop_Up(Reward_State.Level_Up, reward, current_party_level);

        current_party_level++;
        Set_Party_Level(current_party_level);

        Check_Level_For_Save();
        Get_Experience_Point(remain_experience_point);

        Save_Data();

        Battle_Pass_Manager.instance.Get_Requirement("level", 1);
    }

    private void Check_Level_For_Save()
    {
        if (current_party_level == 15)
        {
            Ranking_Manager.instance.Start_Update_Ranking();
        }

        if (current_party_level == 10)
        {
            Database_Controller.instance.Update_All_Data_To_Server();
        }
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        data.l = current_party_level;
        data.e = party_experience.Experience_Point().ToCurrencyString();
        Anti_Cheat_Manager.instance.Set("Level_Data", JsonUtility.ToJson(data));
    }

    #endregion
}