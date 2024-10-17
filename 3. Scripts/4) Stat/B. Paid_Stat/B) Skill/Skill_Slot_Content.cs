using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill_Slot_Content : MonoBehaviour, IPointerClickHandler
{
    private Image skill_icon_image;
    private Skill_Detail_Pop_Up detail_pop_up;

    private Paid_Stat_Content current_content;
    private Paid_Stat current_stat;

    #region "Initialize"

    private void Initialize_Component()
    {
        detail_pop_up = GetComponentInParent<Skill_Detail_Pop_Up>(true);
        skill_icon_image = transform.GetChild(0).GetComponent<Image>();
    }

    #endregion

    #region "Set"

    public void Equip_New_Skill(Paid_Stat target_stat)
    {
        if (skill_icon_image == null)
        {
            Initialize_Component();
        }

        current_content = Stat_Manager.instance.skill_stat_manager.Target_Content(current_stat);
        current_stat = target_stat;

        skill_icon_image.color = Color.white;
        skill_icon_image.sprite = current_stat.stat_icon;
    }

    public void Equip_New_Skill(Paid_Stat_Content target_content)
    {
        current_content = target_content;
        current_stat = current_content.paid_stat;

        if (skill_icon_image == null)
        {
            Initialize_Component();
        }

        skill_icon_image.color = Color.white;
        skill_icon_image.sprite = current_stat.stat_icon;
    }

    public void Clear_Skill()
    {
        current_content = null;
        current_stat = null;
        skill_icon_image.color = Color.clear;
    }

    #endregion

    #region "On Click"

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current_content == null)
        {
            detail_pop_up.Set_Detail_Pop_Up(Stat_Manager.instance.skill_stat_manager.Target_Content(current_stat));

        }
        else
        {
            detail_pop_up.Set_Detail_Pop_Up(current_content);

            //target content is null
        }
    }

    #endregion

    #region "Get"

    public Paid_Stat Get_Stat()
    {
        return current_stat;
    }

    #endregion
}