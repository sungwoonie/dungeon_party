using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Manager : SingleTon<Stat_Manager>
{
    public Upgradable_Stat_Manager growth_stat_manager;
    public Upgradable_Stat_Manager beyond_stat_manager;
    public Upgradable_Stat_Manager ability_stat_manager;

    public Equipment_Stat_Manager equipment_stat_manager;
    public Class_Stat_Manager class_stat_manager;
    public Skill_Stat_Manager skill_stat_manager;

    #region "Calculate"

    public double Calculate_Stat(int stat_type)
    {
        double growth_stat = growth_stat_manager.Get_Stat(stat_type);
        double beyond_stat = beyond_stat_manager.Get_Stat(stat_type);
        double ability_stat = ability_stat_manager.Get_Stat(stat_type);
        double buff_stat = Skill_Buff_Manager.instance.Get_Current_Buff_Stat(stat_type);

        double stat = growth_stat + beyond_stat + ability_stat + buff_stat;
        return stat;
    }

    public double Calculate_Stat(int class_type, int stat_type)
    {
        double growth_stat = growth_stat_manager.Get_Stat(stat_type);
        double beyond_stat = beyond_stat_manager.Get_Stat(stat_type);
        double ability_stat = ability_stat_manager.Get_Stat(stat_type);
        double equipment_stat = equipment_stat_manager.Get_Equipment_Stat(class_type, stat_type);
        double class_stat = class_stat_manager.Get_Stat(class_type, stat_type);
        double buff_stat = Skill_Buff_Manager.instance.Get_Current_Buff_Stat(stat_type);

        double basic_stat = growth_stat + class_stat + equipment_stat;
        double ratio_stat = beyond_stat + ability_stat;

        double stat = basic_stat + (ratio_stat * 0.01f * basic_stat);
        buff_stat = buff_stat * 0.01f * stat;

        stat += buff_stat;

        return stat;
    }

    public bool Calculate_Critical(int class_type, double damage, out double critical_damage)
    {
        double critical_ratio = Calculate_Stat(class_type, 3);
        Debug_Manager.Debug_In_Game_Message($"{class_type}'s critical ratio is {critical_ratio}");

        float random_value = Random.value * 100.0f;

        if (random_value < critical_ratio)
        {
            double critical_damage_ratio = Calculate_Stat(class_type, 4);
            critical_damage = critical_damage_ratio * 0.01f * damage;
            critical_damage = damage + critical_damage;

            return true;
        }
        else
        {
            critical_damage = damage;
            return false;
        }
    }

    #endregion

    #region "Status"

    public double Combat_Power()
    {
        double growth_level = growth_stat_manager.Get_All_Stat_Level();
        double beyond_level = beyond_stat_manager.Get_All_Stat_Level();
        double ability_level = ability_stat_manager.Get_All_Stat_Level();

        double equipment_level = equipment_stat_manager.Get_All_Stat_Level();
        double class_level = class_stat_manager.Get_All_Stat_Level();
        double skill_level = skill_stat_manager.Get_All_Stat_Level();

        double paid_combat_power = (equipment_level * class_level * skill_level) * 1.2f;

        double combat_power = (growth_level + beyond_level + ability_level) * (paid_combat_power > 0 ? paid_combat_power : 1);
        return combat_power;
    }

    public double Stat_Amount_For_Display(int stat_type)
    {
        double growth_stat = growth_stat_manager.Get_Stat(stat_type);
        double beyond_stat = beyond_stat_manager.Get_Stat(stat_type);
        double ability_stat = ability_stat_manager.Get_Stat(stat_type);

        double equipment_stat = 0.0f;
        for (int i = 0; i < 3; i++)
        {
            equipment_stat += equipment_stat_manager.Get_Equipment_Stat(i, stat_type);
        }

        double class_stat = 0.0f;
        for (int i = 0; i < 3; i++)
        {
            class_stat += class_stat_manager.Get_Stat(i, stat_type);
        }

        double basic_stat = growth_stat + class_stat + equipment_stat;
        double ratio_stat = beyond_stat + ability_stat;

        return basic_stat + (ratio_stat * 0.01f * basic_stat);
    }

    #endregion

    #region "Get"

    public Paid_Stat Get_Paid_Stat(string name)
    {
        foreach (var paid_stat_content in equipment_stat_manager.paid_stat_contents)
        {
            if (paid_stat_content.paid_stat.name.Equals(name.Upper_First_Char_By_Underline()))
            {
                return paid_stat_content.paid_stat;
            }
        }

        foreach (var paid_stat_content in class_stat_manager.paid_stat_contents)
        {
            if (paid_stat_content.paid_stat.name.Equals(name.Upper_First_Char_By_Underline()))
            {
                return paid_stat_content.paid_stat;
            }
        }

        foreach (var paid_stat_content in skill_stat_manager.paid_stat_contents)
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