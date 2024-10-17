using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Detail_Pop_Up : Paid_Detail_Pop_Up
{
    public Paid_Stat_Content initialize_content;
    public Paid_Stat_Content detail_content;
    public Localization_Text cool_down_text;
    private Skill_Stat_Manager skill_manager;

    #region "Unity"

    private void Start()
    {
        Set_Detail_Pop_Up(initialize_content);
    }

    #endregion

    #region "Initialize"

    public override void Initialize_Component()
    {
        base.Initialize_Component();

        skill_manager = stat_manager.GetComponent<Skill_Stat_Manager>();
    }

    #endregion

    #region "Set"

    public override void Set_Information()
    {
        base.Set_Information();

        detail_content.paid_stat = current_content.paid_stat;
        detail_content.Initialize_Content();
    }

    public override void Set_Localize_Text()
    {
        base.Set_Localize_Text();

        title_text.key = current_content.paid_stat.name;
        title_text.Localize_Text();

        Set_Description_Text();
    }

    private void Set_Description_Text()
    {
        string[] localized_split = Localization_Manager.instance.Get_Localized_String(current_content.paid_stat.name + "_description").Split("&");
        string description_text_string = string.Empty;

        int cool_down = 0;
        int effect_time = 0;
        double ratio = 0.0f;
        int attack_count = 0;

        foreach (var stat_offset in current_content.paid_stat.stat_offset)
        {
            if (stat_offset.stat_type == 10)
            {
                cool_down = (int)stat_offset.Get_Stat(current_content.paid_stat.level);
            }
            else if (stat_offset.stat_type == 11)
            {
                effect_time = (int)stat_offset.Get_Stat(current_content.paid_stat.level);
            }
            else if(stat_offset.stat_type == 12)
            {
                attack_count = (int)stat_offset.Get_Stat(current_content.paid_stat.level);
            }
            else
            {
                ratio = stat_offset.Get_Stat(current_content.paid_stat.level);
            }
        }

        string cool_down_string = Text_Change.ToCurrencyString(cool_down);
        string effect_time_string = $"<color=yellow>{Text_Change.ToCurrencyString(effect_time)}</color>";
        string ratio_string = $"<color=red>{Text_Change.ToCurrencyString(ratio)}</color>";

        bool buff = Current_Skill_Is_Buff();

        string cool_down_localized_string = $"{Localization_Manager.instance.Get_Localized_String("Cool_Down")} : <color=#86C5FF>{cool_down_string}</color>";

        cool_down_text.Set_Text(cool_down_localized_string);

        description_text_string = $"{localized_split[0]}{(buff ? effect_time_string : ratio_string)}{localized_split[1]}" +
            $"<color=orange>{(buff ? ratio_string : attack_count)}</color>{localized_split[2]}";

        description_text.Set_Text(description_text_string);
    }

    private bool Current_Skill_Is_Buff()
    {
        foreach (var stat_offset in current_content.paid_stat.stat_offset)
        {
            if (stat_offset.stat_type == 11)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region "Button"

    public void Clear_Skill()
    {
        if (current_content != null)
        {
            skill_manager.Clear_Skill(current_content);
        }
    }

    public override void Enhance_Button()
    {
        base.Enhance_Button();

        Set_Description_Text();
    }

    public override void Equip_Button()
    {
        base.Equip_Button();

        skill_manager.Try_Equip_Skill(current_content);
    }

    #endregion
}