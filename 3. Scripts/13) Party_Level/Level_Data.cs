using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Level_Data
{
    public int level;
    public double requirement_experience_point;
    public double reward_gold;

    public double reward_diamond;

    public float equipment_chance;
    public float key_chance;

    public List<float> equipment_rank_chance;
}