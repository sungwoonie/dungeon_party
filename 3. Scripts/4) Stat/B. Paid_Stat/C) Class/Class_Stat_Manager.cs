using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Class_Stat_Manager : Paid_Stat_Manager
{
    public Paid_Stat[] current_class = new Paid_Stat[3];

    #region "Change"

    public void Change_Class(int class_type, Paid_Stat target_stat)
    {
        current_class[class_type] = target_stat;
    }

    #endregion

    #region "Get"

    public double Get_Stat(int class_type, int stat_type)
    {
        double stat = 0.0f;

        foreach (var target_class in current_class[class_type].stat_offset)
        {
            if(target_class.stat_type == stat_type)
            {
                stat += target_class.Get_Stat(current_class[class_type].level);
            }
        }

        return stat;
    }

    #endregion
}