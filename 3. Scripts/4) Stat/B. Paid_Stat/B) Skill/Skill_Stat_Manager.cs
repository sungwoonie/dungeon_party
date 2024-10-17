using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Skill_Stat_Manager는 스킬의 데미지나 효과를 받아오는게 아닌, UI 컨트롤러 입니다. 예로 스킬 장착, 해제 등등
/// </summary>
public class Skill_Stat_Manager : Paid_Stat_Manager
{
    public Skill_Slot[] skill_slots;
    public Toggle auto_skill_toggle;

    private Skill_Slot_Content[] skill_slot_contents;
    private Paid_Stat_Content[] stat_contents;

    private string[] equipped_skill;

    #region "Unity"

    public override void Start()
    {
        base.Start();

        auto_skill_toggle.isOn = Anti_Cheat_Manager.instance.Get("Auto_Skill", false);
    }

    #endregion

    #region "Initialize"

    public override void Initialize_Component()
    {
        base.Initialize_Component();

        skill_slot_contents = GetComponentsInChildren<Skill_Slot_Content>(true);
        stat_contents = GetComponentsInChildren<Paid_Stat_Content>(true);

        auto_skill_toggle.onValueChanged.AddListener(delegate
        {
            Set_Auto_Skill();
        });
    }

    public void Initialize_Data(string[] datas)
    {
        equipped_skill = datas;

        for (int i = 0; i < datas.Length; i++)
        {
            if (string.IsNullOrEmpty(datas[i]))
            {
                continue;
            }
            else
            {
                foreach (var stat_contents in stat_contents)
                {
                    if (stat_contents.paid_stat.name.Equals(datas[i]))
                    {
                        Try_Equip_Skill(stat_contents);
                        break;
                    }
                }
            }
        }
    }

    #endregion

    #region "Equip"

    public void Clear_Skill(Paid_Stat_Content target_content)
    {
        for (int i = 0; i < skill_slot_contents.Length; i++)
        {
            if (skill_slot_contents[i].Get_Stat() != null)
            {
                if (skill_slot_contents[i].Get_Stat() == target_content.paid_stat)
                {
                    skill_slot_contents[i].Clear_Skill();
                    skill_slots[i].Clear_Skill();
                    equipped_skill[i] = "";

                    Save_Data();

                    Debug_Manager.Debug_In_Game_Message($"{target_content.paid_stat} skill is cleared");
                    return;
                }
            }
        }
    }

    public void Try_Equip_Skill(Paid_Stat_Content target_content)
    {
        if (target_content.paid_stat.having_count <= 0)
        {
            if (target_content.paid_stat.level <= 1)
            {
                Error_Message.instance.Set_Error_Message("Error_Message_Having_Count_Under_1");
                return;
            }
        }

        if (Already_Equiped(target_content.paid_stat))
        {
            //already equiped
            Debug_Manager.Debug_In_Game_Message($"{target_content.paid_stat} skill is already equiped");
            Error_Message.instance.Set_Error_Message("Error_Message_Already_Equiped");

            return;
        }

        for (int i = 0; i < skill_slot_contents.Length; i++)
        {
            if (skill_slot_contents[i].Get_Stat() == null)
            {
                skill_slot_contents[i].Equip_New_Skill(target_content);
                skill_slots[i].Set_Skill(target_content.paid_stat);

                equipped_skill[i] = target_content.paid_stat.name;
                Save_Data();

                Debug_Manager.Debug_In_Game_Message($"{target_content.paid_stat} skill setted to skill slot {i}");
                return;
            }
        }

        Debug_Manager.Debug_In_Game_Message($"can't equip {target_content.paid_stat}. there is no empty space");
        Error_Message.instance.Set_Error_Message("Error_Message_No_Space");
    }

    public bool Already_Equiped(Paid_Stat target_skill)
    {
        foreach (var skill_slot in skill_slot_contents)
        {
            if (skill_slot.Get_Stat() == target_skill)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region "Auto"

    public void Set_Auto_Skill()
    {
        Anti_Cheat_Manager.instance.Set("Auto_Skill", auto_skill_toggle.isOn);
        Debug_Manager.Debug_In_Game_Message($"auto skill {auto_skill_toggle.isOn}");
    }

    public bool Auto_Use()
    {
        return auto_skill_toggle.isOn;
    }

    #endregion

    #region "Save"

    private void Save_Data()
    {
        Equipped_Stats.instance.Save_Data_Skill(equipped_skill);
    }

    #endregion

    #region "Get Content"

    public Paid_Stat_Content Target_Content(Paid_Stat stat)
    {
        foreach (var stat_content in stat_contents)
        {
            if (stat_content.paid_stat == stat)
            {
                return stat_content;
            }
        }

        return null;
    }

    #endregion
}