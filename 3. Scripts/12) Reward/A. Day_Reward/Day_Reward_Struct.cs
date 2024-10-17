using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Day_Reward_Struct
{
    public int reward_number;
    public string reward_name;
    public double reward_count;

    public Day_Reward_Struct(int _reward_number, string _reward_name, double _reward_count)
    {
        reward_number = _reward_number;
        reward_name = _reward_name;
        reward_count = _reward_count;
    }
}