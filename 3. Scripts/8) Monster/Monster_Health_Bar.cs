using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Monster_Health_Bar : SingleTon<Monster_Health_Bar>
{
    public TMP_Text health_text;
    public Image health_bar;

    private Monster_Controller current_monster;

    private readonly Vector3 offset_position = new Vector3(0, 1, 0);

    #region "Set"

    public void Set_Monster_Health(Monster_Controller monster)
    {
        current_monster = monster;

        transform.position = current_monster.transform.position + offset_position;
        gameObject.SetActive(true);
        Set_Health_Bar(monster.Get_Monster_Stat().max_health, monster.Get_Monster_Stat().current_health);
    }

    public void Set_Monster_Health_Position(Vector3 monster_position)
    {
        transform.position = monster_position + offset_position;
    }

    public void Set_Health_Bar(double max_health, double current_health)
    {
        double health_ratio = current_health / max_health;

        health_text.text = Text_Change.ToCurrencyString(health_ratio * 100) + "%";

        StopAllCoroutines();
        StartCoroutine(Slider_Lerp((float)health_ratio));
    }

    private IEnumerator Slider_Lerp(float lerp_amount)
    {
        float current_time = 0.0f;

        while (health_bar.fillAmount != lerp_amount)
        {
            current_time += Time.deltaTime;
            health_bar.fillAmount = Mathf.Lerp(health_bar.fillAmount, lerp_amount, current_time / 2 * 0.8f);

            yield return null;
        }
    }

    #endregion
}