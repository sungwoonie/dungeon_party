using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Stat_Controller : SingleTon<Monster_Stat_Controller>
{
    public Monster_Stat[] monster_stats;

    public Monster_Stat Get_Monster_Stat(string code)
    {
        foreach (Monster_Stat stat in monster_stats)
        {
            if (stat.name.Equals(code))
            {
                return stat;
            }
        }

        return null;
    }
}
