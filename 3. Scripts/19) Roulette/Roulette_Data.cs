using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette_Data : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> data;

    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        data = CSVReader.Read("CSV/Roulette_CSV");
    }

    #endregion

    #region "Get"

    public Roulette_Reward[] Get_Reward()
    {
        List<Roulette_Reward> new_rewards = new List<Roulette_Reward>();

        foreach (var target in data)
        {
            Roulette_Reward reward = new Roulette_Reward(target.Value["number"].ToString(), target.Value["reward_name"].ToString(), target.Value["reward_amount"].ToString());
            new_rewards.Add(reward);
        }

        return new_rewards.ToArray();
    }

    #endregion
}