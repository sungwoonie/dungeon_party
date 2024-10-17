using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paid_Stat_Manager : MonoBehaviour
{
    public string budget_type;
    public string resources_path;

    [HideInInspector] public Paid_Stat_Content[] paid_stat_contents; //use for initialize

    private GameObject pop_up;

    #region "Unity"

    public virtual void Awake()
    {
        Initialize_Component();
    }

    public virtual void Start()
    {
        foreach (var paid_stat_content in paid_stat_contents)
        {
            paid_stat_content.Initialize_Component();
            paid_stat_content.Set_Notify();
        }
    }

    #endregion

    #region "Initialize"

    public virtual void Initialize_Component()
    {
        paid_stat_contents = GetComponentsInChildren<Paid_Stat_Content>(true);
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public virtual void Set_Pop_Up()
    {
        Budget_Manager.instance.Set_Budget_Bar(budget_type);
        pop_up.SetActive(true);
    }

    public virtual void Set_Off_Pop_Up()
    {
        Budget_Manager.instance.Set_Budget_Bar("");
        pop_up.SetActive(false);
    }

    #endregion

    #region "Set Content"

    public virtual void Initialize_Content(string stat_name)
    {
        foreach (var paid_stat_content in paid_stat_contents)
        {
            if (paid_stat_content.paid_stat.name.Equals(stat_name))
            {
                paid_stat_content.Initialize_Content();
                paid_stat_content.Set_Notify(true);
                Debug_Manager.Debug_In_Game_Message($"{paid_stat_content.paid_stat} content is initialized");
                break;
            }
        }
    }

    #endregion

    #region "Get New Stat"

    public virtual void Get_New_Stat(string stat_name, int count = 1)
    {
        if (string.IsNullOrEmpty(stat_name))
        {
            return;
        }

        string[] name_split = stat_name.Split("_");
        int rank = 0;

        if (name_split[0] == "Equipment")
        {
            resources_path = $"1) Equipment/Class_{name_split[1]}/Equipment_{name_split[2]}";
            rank = int.Parse(name_split[3]);
        }
        else
        {
            rank = int.Parse(name_split[2]);
        }

        Paid_Stat new_stat = Resources.Load<Paid_Stat>("1. Scriptable_Object/" + resources_path + "/" + stat_name);

        if (new_stat == null)
        {
            //is not exist
            Debug_Manager.Debug_In_Game_Message($"{stat_name} is not exist");
            return;
        }

        new_stat.Modify_Data("having_count", new_stat.having_count + count);

        if (rank >= 4)
        {
            if (name_split[0] == "Class")
            {
                string system_message = $"System_GotNewStat_{name_split[0].Upper_First()}Rank{rank}";
                //Chatting_Manager.instance.Send_Message(system_message);
            }
            else
            {
                if (rank >= 5)
                {
                    string system_message = $"System_GotNewStat_{name_split[0].Upper_First()}Rank{rank}";
                    //Chatting_Manager.instance.Send_Message(system_message);
                }
            }
        }

        Initialize_Content(new_stat.name);

        string reward_text = $"{new_stat.rank} {Localization_Manager.instance.Get_Localized_String(new_stat.name)}";
        Reward_Label_Controller.instance.Set_Reward("equipment", reward_text);
        Debug_Manager.Debug_In_Game_Message($"{new_stat} added");
    }

    #endregion

    #region "Get"

    public int Get_All_Stat_Level()
    {
        int level = 0;

        foreach (var paid_stat_content in paid_stat_contents)
        {
            if (paid_stat_content.paid_stat.having_count > 0 || paid_stat_content.paid_stat.level > 1)
            {
                level += paid_stat_content.paid_stat.level;
            }
        }

        return level;
    }

    public Paid_Stat Get_Paid_Stat(string name)
    {
        foreach (var paid_stat_content in paid_stat_contents)
        {
            if (paid_stat_content.paid_stat.name.Equals(name.Upper_First_Char_By_Underline()))
            {
                return paid_stat_content.paid_stat;
            }
        }

        return null;
    }

    #endregion
}