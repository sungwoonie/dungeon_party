using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Equipment_Detail_Pop_Up : Paid_Detail_Pop_Up
{
    public GameObject[] stats;
    public TMP_Text[] stat_texts;

    private Equipment_Stat_Manager equipment_stat_manager;

    #region "Initialize"

    public override void Initialize_Component()
    {
        base.Initialize_Component();

        equipment_stat_manager = stat_manager.GetComponent<Equipment_Stat_Manager>();
    }

    #endregion

    #region "Set"

    public override void Set_Information()
    {
        base.Set_Information();

        icon_image.sprite = current_content.paid_stat.stat_icon;

        level_text.text = "Lv. " + current_content.paid_stat.level;
        having_count_text.text = current_content.paid_stat.having_count + " / 20";
        rank_text.text = current_content.paid_stat.rank.ToString();

        price_text.text = Text_Change.ToCurrencyString(current_content.paid_stat.price_offset.Get_Price(current_content.paid_stat.level));

        having_count_fill.value = (float)current_content.paid_stat.having_count / 20;

        for (int i = 0; i < grade_icons.Length; i++)
        {
            if (i <= current_content.paid_stat.grade)
            {
                grade_icons[i].SetActive(true);
            }
            else
            {
                grade_icons[i].SetActive(false);
            }
        }
    }

    public override void Set_Localize_Text()
    {
        base.Set_Localize_Text();

        title_text.key = current_content.paid_stat.name.Upper_First_Char_By_Underline();
        title_text.Localize_Text();

        description_text.key = current_content.paid_stat.name.Upper_First_Char_By_Underline() + "_description";
        description_text.Localize_Text();
    }

    public override void Set_Stat_Text()
    {
        base.Set_Stat_Text();

        string current_stat = "";
        string next_stat = "";

        foreach (var stat_offset in current_content.paid_stat.stat_offset)
        {
            current_stat = Text_Change.ToCurrencyString(stat_offset.Get_Stat(current_content.paid_stat.level));
            next_stat = Text_Change.ToCurrencyString(stat_offset.Get_Stat(current_content.paid_stat.level + 1));

            stat_texts[stat_offset.stat_type].text = $"{current_stat} > {next_stat}";
            stats[stat_offset.stat_type].SetActive(true);
        }
    }

    #endregion

    #region "Button"

    public void Close_Detail()
    {
        pop_up.SetActive(false);

        foreach (var stat in stats)
        {
            stat.SetActive(false);
        }
    }

    public override void Equip_Button()
    {
        base.Equip_Button();

        if (current_content != null && current_content.paid_stat.having_count > 0)
        {
            equipment_stat_manager.Equip_New_Equipment(current_content);
            Close_Detail();
            return;
        }
        else
        {
            Error_Message.instance.Set_Error_Message("Error_Message_Having_Count_Under_1");
        }
    }

    #endregion

    #region "Merge"

    public override void Grade_Merge()
    {
        base.Grade_Merge();
        
        Quest_Manager.instance.Increase_Requirement("merge_equipment", 1);

        string merge_name = $"Equipment_{current_content.paid_stat.class_type}_{current_content.paid_stat.equipment_type}_{(int)current_content.paid_stat.rank}_{current_content.paid_stat.grade + 1}";

        stat_manager.Get_New_Stat(merge_name);
    }

    public override void Rank_Merge()
    {
        base.Rank_Merge();

        Quest_Manager.instance.Increase_Requirement("merge_equipment", 1);

        string merge_name = $"Equipment_{current_content.paid_stat.class_type}_{current_content.paid_stat.equipment_type}_{((int)current_content.paid_stat.rank + 1)}_0";

        stat_manager.Get_New_Stat(merge_name);
    }

    #endregion
}