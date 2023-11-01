using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character_Health : SingleTon<Character_Health>
{
    public Image fill;
    public TMP_Text health_text;

    public double max_health;
    public double current_health;

    public void Initialize_Health(double health)
    {
        current_health = max_health;
    }

    public void Get_Max_Health(double health)
    {
        current_health += health - max_health;

        max_health = health;

        StartCoroutine(Fill(current_health));
    }

    public void Get_Damage(double damage)
    {
        Camera_Shake.instance.Shake_Camera(0.1f);

        current_health -= damage;

        if (current_health < 0)
        {
            current_health = 0;
            //pop up
            //death
        }
        else
        {

        }

        StartCoroutine(Fill(current_health));
    }

    private IEnumerator Fill(double goal)
    {
        health_text.text = Text_Change.ToCurrencyString(goal) + " / " + Text_Change.ToCurrencyString(max_health);
        goal = goal / 100;

        while (Mathf.Abs(fill.fillAmount - (float)goal) >= 0.001f)
        {
            fill.fillAmount = Mathf.Lerp(fill.fillAmount, (float)goal, Time.deltaTime * 5.0f);

            yield return null;
        }
    }
}
