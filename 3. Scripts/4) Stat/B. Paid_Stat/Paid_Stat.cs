using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Paid_Stat", menuName = "Paid_Stat")]
public class Paid_Stat : ScriptableObject
{
    public Price_Offset price_offset;
    public Stat_Offset[] stat_offset;

    public int level;
    public int having_count;

    public Paid_Rank rank;
    public int grade;
    public int class_type;
    public int equipment_type;

    public Sprite stat_icon;

    public void Modify_Data(string data_name, object amount)
    {
        GetType().GetField(data_name).SetValue(this, amount);
        Scriptable_Data_Manager.instance.Save_Scriptable_Data_To_Client(this);
    }

    public double Get_Stat(int stat_type)
    {
        foreach (var offset in stat_offset) 
        {
            if (offset.stat_type == stat_type)
            {
                return offset.Get_Stat(level);
            }
        }

        return 0;
    }
}