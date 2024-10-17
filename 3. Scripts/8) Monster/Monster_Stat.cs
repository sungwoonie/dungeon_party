using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Monster_Stat
{
    public double max_health;
    public double current_health;

    public double damage;

    public Monster_Stat(double _max_health, double _damage)
    {
        max_health = _max_health;
        damage = _damage;

        current_health = max_health;
    }
}