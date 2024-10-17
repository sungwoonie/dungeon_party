using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipped_Stats : SingleTon<Equipped_Stats>
{
    private Equipped data;

    public void Initialize_Data(Equipped server_data)
    {
        data = server_data;

        Character_Manager.instance.Initialize_Data(data.ec);
        Stat_Manager.instance.skill_stat_manager.Initialize_Data(data.es);

        List<string[]> ee = new List<string[]>()
        {
            data.ee_0,
            data.ee_1,
            data.ee_2
        };

        Stat_Manager.instance.equipment_stat_manager.Initialize_Data(ee);
    }

    public void Save_Data_Skill(string[] equipped)
    {
        data.es = equipped;
        Anti_Cheat_Manager.instance.Set("Equipped", JsonUtility.ToJson(data));
    }

    public void Save_Data_Class(string[] equipped)
    {
        data.ec = equipped;
        Anti_Cheat_Manager.instance.Set("Equipped", JsonUtility.ToJson(data));
    }

    public void Save_Data_Equipment(List<string[]> equipped)
    {
        data.ee_0 = equipped[0];
        data.ee_1 = equipped[1];
        data.ee_2 = equipped[2];

        Anti_Cheat_Manager.instance.Set("Equipped", JsonUtility.ToJson(data));
    }
}
