using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Buff_Manager : SingleTon<Skill_Buff_Manager>
{
    private Skill_Buff_Slot[] buff_slots;

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
        buff_slots = GetComponentsInChildren<Skill_Buff_Slot>(true);
    }

    #endregion

    #region "Use Buff Skill"

    public void Use_All_AD_Buff()
    {
        for (int i = 0; i < 4; i++)
        {
            Paid_Stat ad_buff_stat  = Resources.Load<Paid_Stat>($"1. Scriptable_Object/2) Skill/AD_Buff_{i}");
            Use_New_Buff_Skill(ad_buff_stat);
        }
    }

    public void Use_New_Buff_Skill(Paid_Stat skill)
    {
        foreach (var slot in buff_slots)
        {
            if (!slot.gameObject.activeSelf)
            {
                slot.Set_Buff_Slot(skill);
                return;
            }
        }
    }

    #endregion

    #region "Get Buff Stat"

    public bool Is_Activating(Paid_Stat target_buff)
    {
        foreach (var slot in buff_slots) //find activating slot
        {
            if (slot.gameObject.activeSelf)
            {
                if (slot.current_buff_skill == target_buff)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public double Get_Current_Buff_Stat(int stat_type)
    {
        double target_stat = 0.0f;

        foreach (var slot in buff_slots) //find activating slot
        {
            if (slot.gameObject.activeSelf)
            {
                foreach (var stat_offset in slot.current_buff_skill.stat_offset) //find equal stat type on slot
                {
                    if (stat_offset.stat_type == stat_type)
                    {
                        target_stat += stat_offset.Get_Stat(slot.current_buff_skill.level);
                        break;
                    }
                }
            }
        }

        Debug_Manager.Debug_In_Game_Message($"current buff amount of {stat_type} stat is {target_stat}");

        return target_stat;
    }

    #endregion
}