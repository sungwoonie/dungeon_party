using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster_Health_Bar : SingleTon<Monster_Health_Bar>
{
    public Image fill;
    public TMP_Text health_text;

    public void Initialize_Health_Bar(double max_health)
    {
        StopAllCoroutines();

        fill.fillAmount = 1.0f;
        health_text.text = Text_Change.ToCurrencyString(max_health) + " / " + Text_Change.ToCurrencyString(max_health);
    }

    public void Set_Health_Bar(double max_health, double current_health)
    {
        StartCoroutine(Fill_Reduce((float)(current_health / max_health)));
        health_text.text = Text_Change.ToCurrencyString(max_health) + " / " + Text_Change.ToCurrencyString(current_health);
    }

    private IEnumerator Fill_Reduce(float goal)
    {
        while (fill.fillAmount > goal)
        {
            fill.fillAmount -= Time.deltaTime;

            yield return null;
        }
    }
}
