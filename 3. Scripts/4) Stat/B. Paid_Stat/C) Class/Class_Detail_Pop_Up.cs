using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Class_Detail_Pop_Up : Paid_Detail_Pop_Up
{
    public Paid_Stat_Content initialize_content;

    public Image class_image; //character_image
    public Image class_icon; //class icon
    public Localization_Text special_attack_description;
    public TMP_Text[] stat_texts;

    private Class_Stat_Manager class_stat_manager;

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

        class_stat_manager = stat_manager.GetComponent<Class_Stat_Manager>();
    }

    #endregion

    #region "Set"

    public override void Set_Detail_Pop_Up(Paid_Stat_Content target_content)
    {
        base.Set_Detail_Pop_Up(target_content);

        Set_Image();
    }

    private void Set_Image()
    {
        string image_resources_path = "5. Image/Class/" + current_content.paid_stat.name + "_Image";
        class_image.sprite = Resources.Load<Sprite>(image_resources_path);

        string icon_resources_path = "3. Icon/Class_Icon_" + current_content.paid_stat.class_type;
        class_icon.sprite = Resources.Load<Sprite>(icon_resources_path);
    }

    public override void Set_Information()
    {
        base.Set_Information();

        level_text.text = current_content.paid_stat.level.ToString();
        having_count_text.text = current_content.paid_stat.having_count + " / 20";
        rank_text.text = current_content.paid_stat.rank.ToString();

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

        string current_stat_name = current_content.paid_stat.name;

        title_text.key = current_stat_name;
        title_text.Localize_Text();

        Set_Description_Text();
        Set_Special_Description_Text();
    }

    private void Set_Description_Text()
    {
        string current_stat_name = current_content.paid_stat.name;

        string[] localized_split = Localization_Manager.instance.Get_Localized_String(current_stat_name + "_description").Split("&");
        string description_text_string = string.Empty;

        int attack_count = (int)current_content.paid_stat.Get_Stat(20);
        float attack_damage = (float)current_content.paid_stat.Get_Stat(21);

        string attack_damage_string = attack_damage + "%";

        description_text_string = $"{localized_split[0]}{attack_damage_string}{localized_split[1]}{attack_count}{localized_split[2]}";

        description_text.Set_Text(description_text_string);
    }

    private void Set_Special_Description_Text()
    {
        string current_stat_name = current_content.paid_stat.name;

        string[] localized_split = Localization_Manager.instance.Get_Localized_String(current_stat_name + "_special_attack_description").Split("&");
        string description_text_string = string.Empty;

        int special_attack_chance_count = (int)current_content.paid_stat.Get_Stat(22);
        int special_attack_count = (int)current_content.paid_stat.Get_Stat(23);
        float special_attack_damage = (float)current_content.paid_stat.Get_Stat(24);

        string special_attack_damage_string = special_attack_damage + "%";

        description_text_string = $"{special_attack_chance_count}{localized_split[1]}{special_attack_damage_string}{localized_split[2]}{special_attack_count}{localized_split[3]}";

        special_attack_description.Set_Text(description_text_string);
    }

    public override void Set_Stat_Text()
    {
        base.Set_Stat_Text();

        for (int i = 0; i < stat_texts.Length; i++)
        {
            double stat = 0.0f;

            stat = current_content.paid_stat.Get_Stat(i);

            string stat_text = Text_Change.ToCurrencyString(stat);

            if (i >= 3)
            {
                stat_text += "%";
            }

            stat_texts[i].text = stat_text;
        }
    }

    #endregion

    #region "Equip"

    public override void Equip_Button()
    {
        base.Equip_Button();

        if (current_content.paid_stat.having_count > 0 || current_content.paid_stat.level > 1)
        {
            //equip
            Character_Manager.instance.Change_Class(current_content.paid_stat.class_type, current_content.paid_stat.name);
            Error_Message.instance.Set_Error_Message($"Error_Message_Class_Equipped");
        }
        else
        {
            //Can't equip class. you don't have this class
            Error_Message.instance.Set_Error_Message($"Error_Message_Having_Count_Under_1");
        }
    }

    #endregion
}