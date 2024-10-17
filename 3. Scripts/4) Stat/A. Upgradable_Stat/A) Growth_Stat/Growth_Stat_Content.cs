using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Growth_Stat_Content : Upgradable_Stat_Content, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private bool clicking;

    #region "Set"

    public override void Set_Max_Text()
    {
        base.Set_Max_Text();

        int current_level = upgradable_stat.level;
        double current_ratio = upgradable_stat.stat_offset.Get_Stat(current_level);
        string current_ratio_string = Text_Change.ToCurrencyString(current_ratio);

        ratio_text.text = $"{ratio_color}{current_ratio_string}</color>";
    }

    public override void Set_Level_Text()
    {
        base.Set_Level_Text();

        int current_level = upgradable_stat.level;

        level_text.text = $"Lv. {current_level}";
    }

    public override void Set_Ratio_Text()
    {
        base.Set_Ratio_Text();

        int current_level = upgradable_stat.level;

        double current_ratio = upgradable_stat.stat_offset.Get_Stat(current_level);
        double next_ratio = upgradable_stat.stat_offset.Get_Stat(current_level + times_count);

        string current_ratio_string = Text_Change.ToCurrencyString(current_ratio);
        string next_ratio_string = Text_Change.ToCurrencyString(next_ratio);

        ratio_text.text = $"{current_ratio_string} > {ratio_color}{next_ratio_string}</color>";
    }

    #endregion

    #region "Clicking"

    private IEnumerator Clicking_Enhance_Button()
    {
        while (clicking)
        {
            Enhance_Button();
            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #region "Event"

    public void OnPointerDown(PointerEventData eventData)
    {
        clicking = true;
        StartCoroutine(Clicking_Enhance_Button());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clicking = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        clicking = false;
    }

    #endregion
}