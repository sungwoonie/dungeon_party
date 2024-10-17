using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Skill_Slot : MonoBehaviour, IPointerClickHandler
{
    public Image skill_icon;
    public Image cool_down_image;

    private Skill_Stat_Manager skill_manager;

    private Paid_Stat current_skill;

    private float current_cool_down;
    private float target_cool_down;

    private TMP_Text cool_down_text;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        skill_manager = FindObjectOfType<Skill_Stat_Manager>(true);
        cool_down_text = GetComponentInChildren<TMP_Text>(true);
    }

    #endregion

    #region "Set"

    public void Clear_Skill()
    {
        current_skill = null;
        current_cool_down = 0.0f;
        target_cool_down = 0.0f;

        skill_icon.color = Color.clear;
        cool_down_image.enabled = false;
    }

    public void Set_Skill(Paid_Stat target_skill)
    {
        StopAllCoroutines();

        skill_icon.color = Color.white;
        cool_down_image.enabled = true;

        current_skill = target_skill;

        skill_icon.sprite = current_skill.stat_icon;
        cool_down_image.fillAmount = 1.0f;

        StartCoroutine(Start_Cool_Down());
    }

    #endregion

    #region "Skill"

    private IEnumerator Start_Cool_Down()
    {
        //get cool down by current skill
        Debug_Manager.Debug_In_Game_Message($"{current_skill} cool down started");

        foreach (var stat_offset in current_skill.stat_offset)
        {
            if (stat_offset.stat_type == 10)
            {
                target_cool_down = (float)stat_offset.Get_Stat(current_skill.level);
            }
        }

        current_cool_down = target_cool_down;

        Debug_Manager.Debug_In_Game_Message($"{current_skill} cool down is {current_cool_down}");

        while (current_skill != null)
        {
            if (current_cool_down > 0.0f)
            {
                current_cool_down -= Time.deltaTime * Game_Time.game_time;
                cool_down_image.fillAmount = current_cool_down / target_cool_down;
                cool_down_text.text = current_cool_down.ToString("N1");
            }
            else
            {
                cool_down_text.text = "";

                if (skill_manager.Auto_Use())
                {
                    Use_Skill();
                }
            }

            yield return null;
        }
    }

    public void Use_Skill()
    {
        if (Event_Bus.Get_Current_State() == Game_State.Combat)
        {
            current_cool_down = target_cool_down;
            cool_down_image.fillAmount = 1.0f;

            Skill_Manager.instance.Use_Skill(current_skill);

            Debug_Manager.Debug_In_Game_Message($"{current_skill} used");
        }
    }

    #endregion

    #region "On Click"

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current_cool_down <= 0 && current_skill != null)
        {
            Use_Skill();
        }
    }

    #endregion
}