using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Buff_Slot : MonoBehaviour
{
    public Paid_Stat current_buff_skill;

    private Image skill_icon;
    private Image buff_timer_icon;

    #region "Unity"

    private void Awake()
    {
        if (skill_icon == null)
        {
            Initialize_Component();
        }
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        skill_icon = transform.GetChild(0).gameObject.GetComponent<Image>();
        buff_timer_icon = transform.GetChild(1).gameObject.GetComponent<Image>();
    }

    #endregion

    #region "Set Slot"

    public void Set_Buff_Slot(Paid_Stat skill)
    {
        if (skill_icon == null)
        {
            Initialize_Component();
        }

        current_buff_skill = skill;

        skill_icon.sprite = skill.stat_icon;
        buff_timer_icon.fillAmount = 1.0f;

        gameObject.SetActive(true);
        StartCoroutine(Buff_On());
    }

    private void Buff_Off()
    {
        if (current_buff_skill.name.Contains("AD"))
        {
            if (User_Data.instance.Auto_Buff())
            {
                Set_Buff_Slot(current_buff_skill);
                return;
            }
        }

        current_buff_skill = null;
        gameObject.SetActive(false);
    }

    #endregion

    #region "Buff"

    private IEnumerator Buff_On()
    {
        float buff_time = (float)current_buff_skill.Get_Stat(11);
        float buff_timer = buff_time;

        if (buff_time <= 0.0f)
        {
            //is not buff skill. but it's running on buff slot
            yield break;
        }

        while (buff_timer > 0)
        {
            buff_timer -= Time.deltaTime * Game_Time.game_time;
            buff_timer_icon.fillAmount = buff_timer / buff_time;
            yield return null;
        }

        Buff_Off();
    }

    #endregion
}