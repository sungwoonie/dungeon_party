using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Stat_Manager : SingleTon<Character_Stat_Manager>
{
    public double Get_Damage(Class_Stat class_stat)
    {
        //변수 받은 후 받은 변수로 계산.
        return 100;
    }

    public float Get_Attack_Delay(Class_Stat class_stat)
    {
        return 1.0f;
    }

    public float Get_Critical_Ratio(float critical_ratio)
    {
        return 1.0f;
    }

    public float Get_Critical_Damage(float critical_ratio)
    {
        return 1.0f;
    }

    public double Get_Max_Health()
    {
        return 100.0f;
    }

    public double Get_Defense_Point()
    {
        return 0.0f;
    }
}
