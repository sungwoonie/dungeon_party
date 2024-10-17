using System.Collections.Generic;
using UnityEngine;

public class Beyond_Stat_Manager : Upgradable_Stat_Manager
{
    [Header("Component")]
    public Localization_Text beyond_rank_text;
    public Localization_Text rank_up_level_text;

    private Beyond_Rank current_rank;
    private Dictionary<string, Dictionary<string, object>> beyond_data = new Dictionary<string, Dictionary<string, object>>();

    #region "Set Data From Server"

    public override void Initialize_Data(int data)
    {
        base.Initialize_Data(data);

        current_rank = (Beyond_Rank)data;
        max_level = int.Parse(beyond_data[current_rank.ToString()]["max_level"].ToString());

        foreach (var upgradable_stat_content in upgradable_stat_contents)
        {
            upgradable_stat_content.Initialize_Component();
            upgradable_stat_content.Set_Content();
        }

        Set_Rank_Text();
    }


    #endregion

    #region "Initialize"

    protected override void Initialize_Component()
    {
        base.Initialize_Component();

        Initialize_CSV();
    }

    private void Initialize_CSV()
    {
        beyond_data = CSVReader.Read("CSV/Beyond_CSV");
    }

    #endregion

    #region "Rank Up"

    public void Beyond_Rank_Up()
    {
        //check all beyond stat's level is over or equal to max level
        foreach (var upgradable_stat_content in upgradable_stat_contents)
        {
            if (upgradable_stat_content.upgradable_stat.level < max_level)
            {
                //Can't rank up beyond. have to rank up to limit all stats
                Debug_Manager.Debug_In_Game_Message($"can't rank up beyond. {upgradable_stat_content.upgradable_stat.name} is not max level");
                Error_Message.instance.Set_Error_Message($"Error_Message_Beyond_Rank_Not_Max_Level");
                return;
            }
        }

        //check player's rank is under max rank
        if (current_rank != Beyond_Rank.LL)
        {
            int rank_up_level = int.Parse(beyond_data[current_rank.ToString()]["rank_up_level"].ToString());

            current_rank++;

            //set max level by current level on csv data
            max_level = int.Parse(beyond_data[current_rank.ToString()]["max_level"].ToString());

            Set_Rank_Text();

            //initialize beyond stat's content
            foreach (var upgradable_stat_content in upgradable_stat_contents)
            {
                upgradable_stat_content.Set_Content();
            }

            //Beyond Rank Up
            Save_Data();
            Debug_Manager.Debug_In_Game_Message($"beyond rank up to {current_rank}");

            if (Party_Level_Manager.instance.Get_Party_Level() >= rank_up_level)
            {
                /*
                current_rank++;

                //set max level by current level on csv data
                max_level = int.Parse(beyond_data[current_rank.ToString()]["max_level"].ToString());

                Set_Rank_Text();

                //initialize beyond stat's content
                foreach (var upgradable_stat_content in upgradable_stat_contents)
                {
                    upgradable_stat_content.Set_Content();
                }

                //Beyond Rank Up
                Save_Data();
                Debug_Manager.Debug_In_Game_Message($"beyond rank up to {current_rank}");
                */
            }
            else
            {
                //Can't rank up beyond. party level is under rank up level
                Debug_Manager.Debug_In_Game_Message($"can't rank up beyond. party level is not reach to requirement");
                Error_Message.instance.Set_Error_Message($"Error_Message_Beyond_Rank_Party_Level");
            }
        }
        else
        {
            //Rank is maximum
            Debug_Manager.Debug_In_Game_Message($"can't rank up beyond. already max rank");
            Error_Message.instance.Set_Error_Message($"Error_Message_Beyond_Rank_Already_Max");
        }
    }

    #endregion

    #region "Set Text"

    public override void Set_Pop_Up()
    {
        Set_Rank_Text();

        base.Set_Pop_Up();
    }

    /// <summary>
    /// 랭크 텍스트들을 수정함. Set_Title_Text, Set_Level_Text 함수 실행함
    /// </summary>
    private void Set_Rank_Text()
    {
        Set_Title_Text();
        Set_Rank_Up_Level_Text();
    }

    private void Set_Title_Text()
    {
        string localized_text = Localization_Manager.instance.Get_Localized_String(beyond_rank_text.key);
        string[] sliced_text = localized_text.Split("&");

        string result_text = $"{sliced_text[0]}<color=yellow><size=140%>{current_rank}</size></color>{sliced_text[1]}";

        beyond_rank_text.Set_Text(result_text);
    }

    private void Set_Rank_Up_Level_Text()
    {
        if (string.IsNullOrEmpty(beyond_data[current_rank.ToString()]["rank_up_level"].ToString()))
        {
            rank_up_level_text.Localize_Text();
        }
        else
        {
            string localized_text = Localization_Manager.instance.Get_Localized_String("beyond_rank_up_level");

            string[] sliced_text = localized_text.Split("&");

            string result_text = $"{sliced_text[0]} {beyond_data[current_rank.ToString()]["rank_up_level"]}{sliced_text[1]}";

            rank_up_level_text.Set_Text(result_text);
        }
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        Anti_Cheat_Manager.instance.Set("Beyond_Rank", ((int)current_rank).ToString());
    }

    #endregion
}