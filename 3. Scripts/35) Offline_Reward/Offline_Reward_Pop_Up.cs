using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Offline_Reward_Pop_Up : MonoBehaviour
{
    [Tooltip("0 = gold, 1 = diamond, 2 = key, 3 = experience_point")]
    public TMP_Text[] reward_amount_texts;

    public GameObject equipment_reward;
    private Reward_Content[] equipment_contents;

    private Reward_Struct current_reward;

    #region "Initialize"

    private void Initialize_Component()
    {
        equipment_contents = equipment_reward.GetComponentsInChildren<Reward_Content>(true);
    }

    #endregion

    #region "Set"

    public void Set_Reward_Pop_Up(Reward_Struct reward_struct)
    {
        if (equipment_contents == null)
        {
            Initialize_Component();
        }

        current_reward = reward_struct;

        Set_Amount_Texts();
        Set_Equipment();

        gameObject.SetActive(true);
    }

    private void Set_Amount_Texts()
    {
        reward_amount_texts[0].text = current_reward.reward_budget.gold.ToCurrencyString();
        reward_amount_texts[1].text = current_reward.reward_budget.diamond.ToCurrencyString();
        reward_amount_texts[2].text = current_reward.reward_budget.key.ToCurrencyString();
        reward_amount_texts[3].text = current_reward.experience_point.ToCurrencyString();
    }

    private void Set_Equipment()
    {
        if (current_reward.reward_equipment.Length > 0)
        {
            for (int i = 0; i < current_reward.reward_equipment.Length; i++)
            {
                equipment_contents[i].Set_Reward_Content_Paid_Stat(current_reward.reward_equipment[i]);
            }

            equipment_reward.SetActive(true);
        }
        else
        {
            equipment_reward.SetActive(false);
        }
    }

    #endregion

    #region "Button"

    public void Get_Reward_Button(bool ad)
    {
        if (ad)
        {
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_Offline_Reward", Get_Ad_Reward);
        }
        else
        {
            Offline_Reward_Controller.instance.Get_Reward(false);
            gameObject.SetActive(false);
        }
    }

    private void Get_Ad_Reward()
    {
        Offline_Reward_Controller.instance.Get_Reward(true);
        gameObject.SetActive(false);
    }

    #endregion
}
