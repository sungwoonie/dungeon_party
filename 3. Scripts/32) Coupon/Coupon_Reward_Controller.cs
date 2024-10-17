using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coupon_Reward_Controller : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, object>> reward_csv = new Dictionary<string, Dictionary<string, object>>();

    private List<Coupon_Reward> coupon_rewards = new List<Coupon_Reward>();

    #region "Unity"

    private void Awake()
    {
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    private void Initialize_CSV()
    {
        reward_csv = CSVReader.Read("CSV/Coupon_Reward_CSV");

        foreach (var datas in reward_csv)
        {
            Coupon_Reward new_reward = new Coupon_Reward();

            new_reward.coupon_code = datas.Value["coupon_code"].ToString();

            new_reward.rewards = datas.Value["rewards"].ToString().Split(";");

            string[] amounts_text = datas.Value["amounts"].ToString().Split(";");
            double[] amounts = new double[amounts_text.Length];

            for (int i = 0; i < amounts_text.Length; i++)
            {
                amounts[i] = double.Parse(amounts_text[i]);
            }

            new_reward.amounts = amounts;

            coupon_rewards.Add(new_reward);
        }
    }

    #endregion

    #region "Reward"

    public bool Coupon_Exist(string coupon_code)
    {
        foreach (var coupon_reward in coupon_rewards)
        {
            if (coupon_reward.coupon_code == coupon_code)
            {
                return true;
            }
        }

        return false;
    }

    public void Give_Coupon_Reward(string coupon_code)
    {
        foreach (var coupon_reward in coupon_rewards)
        {
            if (coupon_reward.coupon_code == coupon_code)
            {
                Give_Reward(coupon_reward);
                break;
            }
        }
    }

    private void Give_Reward(Coupon_Reward reward)
    {
        string[] reward_types = reward.rewards;
        double[] reward_amounts = reward.amounts;

        for (int i = 0; i < reward_types.Length; i++)
        {
            if (Budget_Manager.instance.Is_Budget(reward_types[i]))
            {
                Budget_Manager.instance.Earn_Budget(reward_types[i], reward_amounts[i]);
                continue;
            }
            else
            {
                switch (reward_types[i].Split("_")[0])
                {
                    case "Equipment":
                        Stat_Manager.instance.equipment_stat_manager.Get_New_Stat(reward_types[i]);
                        break;
                    case "Skill":
                        Stat_Manager.instance.skill_stat_manager.Get_New_Stat(reward_types[i]);
                        break;
                    case "Class":
                        Stat_Manager.instance.class_stat_manager.Get_New_Stat(reward_types[i]);
                        break;
                }
            }
        }
    }

    #endregion
}
