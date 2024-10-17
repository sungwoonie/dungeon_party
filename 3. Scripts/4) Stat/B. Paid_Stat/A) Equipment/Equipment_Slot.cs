using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Equipment_Slot : MonoBehaviour, IPointerClickHandler
{
    public Paid_Detail_Pop_Up detail_pop_up;

    private Paid_Stat_Content current_paid_content;
    private Paid_Stat current_equipment;

    private Image equipment_icon_image;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        equipment_icon_image = transform.GetChild(1).GetComponent<Image>();
    }

    #endregion

    #region "Set"

    public void Set_Slot(Paid_Stat target_stat)
    {
        if (equipment_icon_image == null)
        {
            Initialize_Component();
        }

        if (target_stat == null)
        {
            //clear
            current_paid_content = null;
            current_equipment = null;

            equipment_icon_image.sprite = null;
            equipment_icon_image.color = Color.clear;
        }
        else
        {
            //equip
            current_equipment = target_stat;

            equipment_icon_image.sprite = current_equipment.stat_icon;
            equipment_icon_image.color = Color.white;
        }
    }

    public void Set_Slot(Paid_Stat_Content target_stat_content)
    {
        if (equipment_icon_image == null)
        {
            Initialize_Component();
        }

        if (target_stat_content == null)
        {
            //clear
            current_paid_content = null;
            current_equipment = null;

            equipment_icon_image.sprite = null;
            equipment_icon_image.color = Color.clear;
        }
        else
        {
            //equip
            current_paid_content = target_stat_content;
            current_equipment = current_paid_content.paid_stat;

            equipment_icon_image.sprite = current_equipment.stat_icon;
            equipment_icon_image.color = Color.white;
        }
    }

    #endregion

    #region "On Click"

    public void OnPointerClick(PointerEventData eventData)
    {
        if (current_paid_content == null)
        {
            //is not equiped
            return;
        }

        //detail_pop_up.Set_Detail_Pop_Up(current_paid_content);
    }

    #endregion
}