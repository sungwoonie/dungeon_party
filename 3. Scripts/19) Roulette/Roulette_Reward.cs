using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Roulette_Reward
{
    public int roulette_number;
    public string reward_name;
    public double reward_amount;

    public Roulette_Reward(string number, string name, string amount)
    {
        roulette_number = int.Parse(number);
        reward_name = name;
        reward_amount = double.Parse(amount);
    }
}