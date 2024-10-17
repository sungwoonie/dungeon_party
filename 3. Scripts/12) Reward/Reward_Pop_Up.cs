using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Reward_Pop_Up : SingleTon<Reward_Pop_Up>
{
    public GameObject[] badges;
    public TMP_Text amount_text;

    public Reward_Content ability_reward, key_reward, gold_reward, diamond_reward, experience_reward, enhance_stone_reward, beyond_stone_reward;
    public Reward_Content[] equipment_rewards;

    private GameObject pop_up;

    private Reward_Struct current_reward;

    private List<Remain_Pop_Up> remain = new List<Remain_Pop_Up>();

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    private IEnumerator Set_Off()
    {
        yield return new WaitForSeconds(2.0f);

        pop_up.SetActive(false);

        if (remain.Count > 0)
        {
            if (!remain[0].reward_struct.Equals(current_reward))
            {
                Set_Reward_Pop_Up(remain[0].reward_state, remain[0].reward_struct, remain[0].amount);
                remain.Remove(remain[0]);
            }
        }
    }

    #region "Set"

    public void Set_Reward_Pop_Up(Reward_State state, Reward_Struct reward, int amount)
    {
        if (pop_up.activeSelf)
        {
            Remain_Pop_Up current_pop_up = new Remain_Pop_Up(state, reward, amount);
            remain.Add(current_pop_up);
            return;
        }

        current_reward = reward;

        //amount_text.text = amount.ToString();
        //badges[(int)state].SetActive(true);

        gold_reward.Set_Reward_Content_Budget(current_reward.reward_budget.gold);
        diamond_reward.Set_Reward_Content_Budget(current_reward.reward_budget.diamond);
        key_reward.Set_Reward_Content_Budget(current_reward.reward_budget.key);
        ability_reward.Set_Reward_Content_Budget(current_reward.reward_budget.ability_stone);
        enhance_stone_reward.Set_Reward_Content_Budget(current_reward.reward_budget.enhance_stone);
        beyond_stone_reward.Set_Reward_Content_Budget(current_reward.reward_budget.beyond_stone);

        experience_reward.Set_Reward_Content_Budget(current_reward.experience_point);

        string[] equipment_reward = current_reward.reward_equipment;

        for (int i = 0; i < equipment_reward.Length; i++)
        {
            equipment_rewards[i].Set_Reward_Content_Paid_Stat(equipment_reward[i]);
        }

        pop_up.SetActive(true);

        current_reward.Get_Reward();

        StartCoroutine(Set_Off());
    }

    #endregion

    #region "Button"

    public void Close_Button()
    {
        pop_up.SetActive(false);

        foreach (GameObject badge in badges)
        {
            badge.SetActive(false);
        }

        if (remain.Count > 0)
        {
            if (!remain[0].reward_struct.Equals(current_reward))
            {
                Set_Reward_Pop_Up(remain[0].reward_state, remain[0].reward_struct, remain[0].amount);
                remain.Remove(remain[0]);
            }
        }

        current_reward.Get_Reward();
    }

    public void Skip_Button()
    {
        pop_up.SetActive(false);

        foreach (GameObject badge in badges)
        {
            badge.SetActive(false);
        }

        foreach (var item in remain)
        {
            if (!item.reward_struct.Equals(current_reward))
            {
                Debug.Log("Get Reward" + item.reward_struct.reward_budget.gold);
                item.reward_struct.Get_Reward();
            }
        }

        remain.Clear();
        current_reward.Get_Reward();
    }

    #endregion
}

public struct Remain_Pop_Up
{
    public Reward_State reward_state;
    public Reward_Struct reward_struct;
    public int amount;

    public Remain_Pop_Up(Reward_State _reward_state, Reward_Struct _reward_struct, int _amount)
    {
        reward_state = _reward_state;
        reward_struct = _reward_struct;
        amount = _amount;
    }
}