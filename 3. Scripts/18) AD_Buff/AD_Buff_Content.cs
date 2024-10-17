using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AD_Buff_Content : MonoBehaviour
{
    public Paid_Stat ad_buff;

    private Button button;

    #region "Unity"

    private void Awake()
    {
        Initialize_Button();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Button()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => On_Click());
    }

    #endregion

    #region "On Click"

    public void On_Click()
    {
        if (Skill_Buff_Manager.instance.Is_Activating(ad_buff))
        {
            return;
        }

        string reward_key = $"AD_Reward_{ad_buff.name}";

        Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up(reward_key, Start_Buff);
    }

    private void Start_Buff()
    {
        Skill_Buff_Manager.instance.Use_New_Buff_Skill(ad_buff);
    }

    #endregion
}