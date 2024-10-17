using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stage_Data
{
    public float drop_gold_ratio_minimum;
    public float drop_gold_ratio_maximum;
    public double drop_gold_offset;

    public float equipment_drop_chance;
    public float key_drop_chance;

    public double drop_diamond;
    public float diamond_drop_chance;

    public double experience_point;

    public List<float> equipment_rank_chance;
}