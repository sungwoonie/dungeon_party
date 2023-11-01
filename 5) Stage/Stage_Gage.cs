using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage_Gage : SingleTon<Stage_Gage>
{
    public Image fill;
    public TMP_Text gage_text;
    public GameObject boss_button_image;

    public float current_gage;

    public void Set_Stage_Gage(float amount)
    {
        current_gage = amount;

        if (current_gage < 100)
        {
            boss_button_image.SetActive(false);
        }

        StopAllCoroutines();
        StartCoroutine(Fill(current_gage));
    }

    public void Increase_Stage_Gage(float amount)
    {
        current_gage += amount;

        StartCoroutine(Fill(current_gage));

        if (current_gage >= 100)
        {
            boss_button_image.SetActive(true);
            gage_text.text = 100 + "%";
        }
        else
        {
            gage_text.text = current_gage + "%";
        }
    }

    private IEnumerator Fill(float goal)
    {
        goal = goal / 100;

        while (Mathf.Abs(fill.fillAmount - goal) >= 0.001f)
        {
            fill.fillAmount = Mathf.Lerp(fill.fillAmount, goal, Time.deltaTime * 5.0f);

            yield return null;
        }
    }
}
