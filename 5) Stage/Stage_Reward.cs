using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage_Reward : MonoBehaviour
{
    public Stage_Reward_List[] rewards;

    public GameObject reward_pop_up;
    public GameObject[] reward_items;
    public Image[] reward_icons;

    public void Get_Reward(int stage)
    {
        Stage_Reward_List current_stage_reward = null;

        string code = "Stage_Reward_" + stage;

        foreach (Stage_Reward_List reward in rewards)
        {
            if (reward.name == code)
            {
                current_stage_reward = reward;
                break;
            }
        }

        /*
        for (int i = 0; i < current_stage_reward.reward_count; i++)
        {
            int special_chance = Random.Range(0, 100);

            if (special_chance < current_stage_reward.special_reward_chance)
            {
                //special
            }
            else
            {

            }
        }*/

        reward_pop_up.SetActive(true);
    }

    public void Continue()
    {
        reward_pop_up.SetActive(false);
    }

    public void Watch_AD()
    {
        reward_pop_up.SetActive(false);
    }
}
