using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Day_Reward_Content : MonoBehaviour
{
    public TMP_Text day_text;
    public TMP_Text amount_text;
    public Image reward_icon;

    public GameObject received_object;
    public GameObject complete_object;

    public bool received;
    public bool completed;

    public Day_Reward_Struct current_reward;

    private Day_Reward_Manager reward_manager;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        if (!reward_manager)
        {
            reward_manager = GetComponentInParent<Day_Reward_Manager>();
        }
    }

    #endregion

    #region "Set"

    public void Set_Reward(Day_Reward_Struct reward)
    {
        current_reward = reward;

        if (Stat_Manager.instance.Get_Paid_Stat(current_reward.reward_name) != null)
        {
            reward_icon.sprite = Stat_Manager.instance.Get_Paid_Stat(current_reward.reward_name).stat_icon;
            amount_text.text = Localization_Manager.instance.Get_Localized_String(current_reward.reward_name.Upper_First_Char_By_Underline());
        }
        else
        {
            reward_icon.sprite = Resources.Load<Sprite>($"3. Icon/{current_reward.reward_name.Upper_First_Char_By_Underline()}_Icon");
            amount_text.text = current_reward.reward_count.ToCurrencyString();
        }

        day_text.text = current_reward.reward_number.ToString();

        gameObject.SetActive(true);
    }

    public void Set_Status(int complete, int receive)
    {
        received = receive >= current_reward.reward_number;
        completed = complete >= current_reward.reward_number;

        if (completed && !received)
        {
            if (!reward_manager)
            {
                Initialize_Component();
                reward_manager.Set_Notify(true);
            }
        }

        Set_Objects();
    }

    private void Set_Objects()
    {
        received_object.SetActive(received);
        complete_object.SetActive(!received && completed);
    }

    #endregion

    #region "Get Reward"

    public void Get_Reward()
    {
        if (received)
        {
            return;
        }

        string[] reward_type = current_reward.reward_name.Split("_");

        switch (reward_type[0])
        {
            case "experience":
                Party_Level_Manager.instance.Get_Experience_Point(current_reward.reward_count);
                break;
            case "Equipment":
                Stat_Manager.instance.equipment_stat_manager.Get_New_Stat(current_reward.reward_name, (int)current_reward.reward_count);
                break;
            case "Class":
                Stat_Manager.instance.class_stat_manager.Get_New_Stat(current_reward.reward_name, (int)current_reward.reward_count);
                break;
            default:
                Budget_Manager.instance.Earn_Budget(current_reward.reward_name, current_reward.reward_count);
                break;
        }

        received = true;
        Set_Objects();
    }

    #endregion
}