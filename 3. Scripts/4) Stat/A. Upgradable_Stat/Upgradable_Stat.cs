using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgradable_Stat", menuName = "Upgradable_Stat")]
public class Upgradable_Stat : ScriptableObject
{
    public Price_Offset price_offset;
    public Stat_Offset stat_offset;

    public int level;

    public void Modify_Data(string data_name, object amount)
    {
        GetType().GetField(data_name).SetValue(this, amount);
        Scriptable_Data_Manager.instance.Save_Scriptable_Data_To_Client(this);
    }
}