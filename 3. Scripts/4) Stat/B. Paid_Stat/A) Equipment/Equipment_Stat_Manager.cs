using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_Stat_Manager : Paid_Stat_Manager
{
    public Class_Equipment_Stat[] class_equipment_stat;

    private Equipment_Slot[] equipment_slots;
    private Equipment_Show_Case show_case;

    private List<string[]> equipped;
    private int current_equipment_slot_number;

    #region "Initialize"

    public override void Initialize_Component()
    {
        base.Initialize_Component();

        equipment_slots = GetComponentsInChildren<Equipment_Slot>(true);
        show_case = GetComponentInChildren<Equipment_Show_Case>(true);
    }

    public void Initialize_Data(List<string[]> server_equipped)
    {
        equipped = server_equipped;

        for (int i = 0; i < equipped.Count; i++)
        {
            for (int j = 0; j < equipped[i].Length; j++)
            {
                if (!string.IsNullOrEmpty(equipped[i][j]))
                {
                    Equip_New_Equipment(Get_Paid_Stat(equipped[i][j]));
                }
            }
        }

        Set_Slots_By_Class(0);
    }

    #endregion

    #region "Equip"

    public void Auto_Equip_New(Paid_Stat target_stat)
    {
        if (target_stat == null)
        {
            return;
        }

        int equipment_type = target_stat.equipment_type;
        int class_type = target_stat.class_type;

        class_equipment_stat[class_type].current_equipment[equipment_type] = target_stat;
        equipped[class_type][equipment_type] = target_stat.name;
    }

    public void Equip_New_Equipment(Paid_Stat target_stat)
    {
        if (target_stat == null)
        {
            return;
        }

        int equipment_type = target_stat.equipment_type;
        int class_type = target_stat.class_type;

        equipment_slots[equipment_type].Set_Slot(target_stat);

        class_equipment_stat[class_type].current_equipment[equipment_type] = target_stat;

        equipped[class_type][equipment_type] = target_stat.name;
        show_case.Set_Show_Case(class_type);
    }

    public void Equip_New_Equipment(Paid_Stat_Content target_stat_content)
    {
        int equipment_type = target_stat_content.paid_stat.equipment_type;
        int class_type = target_stat_content.paid_stat.class_type;

        equipment_slots[equipment_type].Set_Slot(target_stat_content);

        class_equipment_stat[class_type].current_equipment[equipment_type] = target_stat_content.paid_stat;

        show_case.Set_Show_Case(class_type);

        equipped[class_type][equipment_type] = target_stat_content.paid_stat.name;
        Save_Data();
    }

    #endregion

    #region "Set Slot"

    public override void Set_Pop_Up()
    {
        base.Set_Pop_Up();

        //Set_Slots_By_Class(0);
    }

    public void Set_Slots_By_Class(int class_type)
    {
        for (int i = 0; i < class_equipment_stat[class_type].current_equipment.Count; i++)
        {
            equipment_slots[i].Set_Slot(class_equipment_stat[class_type].current_equipment[i]);
        }

        show_case.Set_Show_Case(class_type);
        current_equipment_slot_number = class_type;
    }

    #endregion

    #region "Get Stat"

    public double Get_Equipment_Stat(int class_type, int stat_type)
    {
        double stat = 0.0f;

        foreach (var current_equipment in class_equipment_stat[class_type].current_equipment)
        {
            if (current_equipment != null)
            {
                foreach (var stat_offset in current_equipment.stat_offset)
                {
                    if (stat_offset.stat_type.Equals(stat_type))
                    {
                        stat += stat_offset.Get_Stat(current_equipment.level);
                        continue;
                    }
                }
            }
        }

        return stat;
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        Equipped_Stats.instance.Save_Data_Equipment(equipped);
    }

    #endregion

    #region "Auto Equip"

    public void Auto_Equip()
    {
        for (int i = 0; i < class_equipment_stat.Length; i++)
        {
            foreach (var paid_stat_content in paid_stat_contents)
            {
                Paid_Stat target_stat = paid_stat_content.paid_stat;

                if (target_stat.having_count > 0)
                {
                    Paid_Stat equipped_stat = null;

                    if (class_equipment_stat[i].current_equipment[target_stat.equipment_type] == null)
                    {
                        Auto_Equip_New(target_stat);
                        continue;
                    }
                    else
                    {
                        equipped_stat = class_equipment_stat[i].current_equipment[target_stat.equipment_type];
                    }

                    if (target_stat.class_type == i)
                    {
                        if (equipped_stat == null)
                        {
                            Auto_Equip_New(target_stat);
                        }
                        else if (target_stat.rank > equipped_stat.rank)
                        {
                            Auto_Equip_New(target_stat);
                        }
                        else if (target_stat.grade > equipped_stat.grade && target_stat.rank >= equipped_stat.rank)
                        {
                            Auto_Equip_New(target_stat);
                        }
                    }
                }
            }
        }

        Set_Slots_By_Class(current_equipment_slot_number);
        Save_Data();
    }

    #endregion
}

#region "Class Equipment Struct"

[System.Serializable]
public struct Class_Equipment_Stat
{
    public List<Paid_Stat> current_equipment;
}

#endregion