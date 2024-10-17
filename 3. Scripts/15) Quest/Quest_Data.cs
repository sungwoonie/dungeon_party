using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Data : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> daily_quest = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, Dictionary<string, object>> monthly_quest = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, Dictionary<string, object>> repeat_quest = new Dictionary<string, Dictionary<string, object>>();

    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        daily_quest = CSVReader.Read("CSV/Daily_Quest_CSV");
        monthly_quest = CSVReader.Read("CSV/Monthly_Quest_CSV");
        repeat_quest = CSVReader.Read("CSV/Repeat_Quest_CSV");
    }

    #endregion

    #region "Get Quest Struct"

    public Quest_Struct Get_Repeat_Quest()
    {
        Dictionary<string, object> new_quest = repeat_quest[Get_Random_Key()];
        string quest_name = new_quest["quest_name"].ToString();
        int requirement = int.Parse(new_quest["requirement"].ToString());
        double reward_diamond = double.Parse(new_quest["reward"].ToString());

        Quest_Struct new_quest_struct = new Quest_Struct(quest_name, requirement, reward_diamond, 0, "diamond");

        return new_quest_struct;
    }

    public Quest_Struct[] Get_Daily_Quest()
    {
        List<Quest_Struct> quest_structs = new List<Quest_Struct>();

        foreach (var item in daily_quest)
        {
            string quest_name = item.Value["quest_name"].ToString();
            int requirement = int.Parse(item.Value["requirement"].ToString());
            double reward_diamond = double.Parse(item.Value["reward"].ToString());
            string reward_name = item.Value["reward_name"].ToString();

            Quest_Struct new_quest_struct = new Quest_Struct(quest_name, requirement, reward_diamond, 0, reward_name);
            quest_structs.Add(new_quest_struct);
        }

        return quest_structs.ToArray();
    }

    public Quest_Struct[] Get_Monthly_Quest()
    {
        List<Quest_Struct> quest_structs = new List<Quest_Struct>();

        foreach (var item in monthly_quest)
        {
            string quest_name = item.Value["quest_name"].ToString();
            int requirement = int.Parse(item.Value["requirement"].ToString());
            double reward_diamond = double.Parse(item.Value["reward"].ToString());
            string reward_name = item.Value["reward_name"].ToString();

            Quest_Struct new_quest_struct = new Quest_Struct(quest_name, requirement, reward_diamond, 0, reward_name);
            quest_structs.Add(new_quest_struct);
        }

        return quest_structs.ToArray();
    }

    #endregion

    #region "Get"

    private string Get_Random_Key()
    {
        int random = Random.Range(0, repeat_quest.Count);

        switch (random)
        {
            case 0:
                return "kill_count";
            case 1:
                return "skill_use";
            case 2:
                return "enter_dungeon";
            case 3:
                return "play_gamble";
            case 4:
                return "spawn_boss";
            case 5:
                return "merge_equipment";
            default:
                return "";
        }
    }

    #endregion
}