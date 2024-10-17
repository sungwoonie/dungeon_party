using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Reward_AD_Pop_Up : SingleTon<Reward_AD_Pop_Up>
{
    public Localization_Text reward_text;
    public TMP_Text reward_ad_count_text;

    private GameObject pop_up;
    private Reward_AD_Button reward_ad_button;

    private UnityAction current_action;

    private int current_reward_ad_count;

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
        reward_ad_button = GetComponentInChildren<Reward_AD_Button>(true);
    }

    #endregion

    #region "Set"

    public void Set_Reward_AD_Count(bool reset)
    {
        if (reset)
        {
            Anti_Cheat_Manager.instance.Set("AD_Reward_Count", 10);
        }

        current_reward_ad_count = Anti_Cheat_Manager.instance.Get("AD_Reward_Count", 10);
    }

    public void Set_Reward_AD_Pop_Up(string key, UnityAction action)
    {
        current_action = action;

        reward_text.Set_Localization_Key(key);
        reward_text.Localize_Text();

        reward_ad_count_text.text = $"{current_reward_ad_count} / 10";

        pop_up.SetActive(true);
    }

    #endregion

    #region "Call by Button"

    public void Show_Reward_AD()
    {
        if (current_reward_ad_count > 0)
        {
            if (User_Data.instance.AD_Remove() == true)
            {
                Give_Reward();
            }
            else
            {
                AD_Controller.instance.Show_Reward_AD();
            }

            StartCoroutine(reward_ad_button.Start_AD_Button_Count());
            pop_up.SetActive(false);
        }
        else
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Over_AD_Count");
        }
    }

    #endregion

    #region "Give Reward"

    public void Give_Reward()
    {
        StartCoroutine(Give_AD_Reward());
    }

    private IEnumerator Give_AD_Reward()
    {
        //광고 쓰레드에서 보상을 주게되면 크래시 될 수도 있어서 다음 프레임으로 진행
        yield return new WaitForEndOfFrame();
        current_action.Invoke();
        current_action = null;

        current_reward_ad_count--;
        Anti_Cheat_Manager.instance.Set("AD_Reward_Count", current_reward_ad_count);
    }

    #endregion
}
