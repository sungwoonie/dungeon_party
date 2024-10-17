using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgradable_Stat_Manager : MonoBehaviour
{
    public int max_level = int.MaxValue;
    public int times_count = 1;
    public string budget_type;

    [HideInInspector]public Upgradable_Stat_Content[] upgradable_stat_contents;

    private GameObject pop_up;

    #region "Unity"

    protected virtual void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Set Data From Server"

    public virtual void Initialize_Data(int data)
    {
    }

    #endregion

    #region "Initialize"

    protected virtual void Initialize_Component()
    {
        upgradable_stat_contents = GetComponentsInChildren<Upgradable_Stat_Content>(true);
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
        pop_up.SetActive(false);
        Budget_Manager.instance.Set_Budget_Bar("gold");
        Debug_Manager.Debug_In_Game_Message($"{transform.name} pop up off");
    }

    #endregion

    #region "Get Value"

    public virtual double Get_Stat(int stat_type)
    {
        double stat = 0.0f;

        foreach (var upgradable_stat_content in upgradable_stat_contents)
        {
            if (upgradable_stat_content.upgradable_stat.stat_offset.stat_type.Equals(stat_type))
            {
                stat += upgradable_stat_content.upgradable_stat.stat_offset.Get_Stat(upgradable_stat_content.upgradable_stat.level);
                continue;
            }
        }

        Debug_Manager.Debug_In_Game_Message($"{stat_type} stat by {transform.name} is {stat}");

        return stat;
    }

    public int Get_All_Stat_Level()
    {
        int level = 0;

        foreach (var upgradable_stat_content in upgradable_stat_contents)
        {
            level += upgradable_stat_content.upgradable_stat.level;
        }

        if (level <= 0)
        {
            return 1;
        }

        return level;
    }

    #endregion

    #region "Set Times"

    public void Set_Times(int count)
    {
        times_count = count;

        foreach (var upgradable_stat_content in upgradable_stat_contents)
        {
            upgradable_stat_content.Set_Content();
        }
    }

    #endregion
}