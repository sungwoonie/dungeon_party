using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class_Stat", menuName = "Class_Stat", order = 2)]
public class Class_Stat : ScriptableObject
{
    public double striking_power;
    public float attack_delay;
    public float critical_ratio;
    public float critical_damage;

    [TextArea] public string attack_information;
    public double attack_damage;

    public int beyond;

    public RuntimeAnimatorController animator;
}
