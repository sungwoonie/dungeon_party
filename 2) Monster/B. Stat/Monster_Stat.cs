using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Stat", menuName = "Monster_Stat", order = 0)]
public class Monster_Stat : ScriptableObject
{
    public double max_health;
    public double damage;

    public double drop_gold;
}
