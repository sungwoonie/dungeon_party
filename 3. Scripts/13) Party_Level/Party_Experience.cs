using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Party_Experience : MonoBehaviour
{
    public Image experience_bar;
    public TMP_Text experience_text;

    private double current_requirement_experience_point;
    private double current_experience_point;

    private Party_Level_Manager level_manager;

    #region "Unity"

    private void Start()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        level_manager = Party_Level_Manager.instance;
    }

    #endregion

    #region "Get Experience Point"

    public void Get_Experience_Point(double experience_point)
    {
        current_experience_point += experience_point;

        if (current_experience_point >= current_requirement_experience_point)
        {
            double remain_experience_point = current_experience_point - current_requirement_experience_point;
            current_experience_point = 0;

            level_manager.Level_Up(remain_experience_point);
        }
        else
        {
            StartCoroutine(Set_Experience_Bar_Lerp());
        }
    }

    #endregion

    #region "Set Experience"

    public void Set_Requirement_Experience_Point(double requirement_experience_point)
    {
        current_requirement_experience_point = requirement_experience_point;

        StartCoroutine(Set_Experience_Bar_Lerp());
    }

    public void Set_Experience_Point(double experience_point)
    {
        current_experience_point = experience_point;
        Set_Experience_Bar();
    }

    #endregion

    #region "Set Experience Bar"

    private void Set_Experience_Bar()
    {
        float amount = (float)((float)current_experience_point / (float)current_requirement_experience_point);
        experience_bar.fillAmount = amount;

        experience_text.text = $"{Text_Change.ToCurrencyString(current_experience_point)} / {Text_Change.ToCurrencyString(current_requirement_experience_point)}";
    }

    private IEnumerator Set_Experience_Bar_Lerp()
    {
        experience_text.text = $"{Text_Change.ToCurrencyString(current_experience_point)} / {Text_Change.ToCurrencyString(current_requirement_experience_point)}";

        float lerp_time = 0.0f;

        float amount = (float)(current_experience_point / current_requirement_experience_point);

        while(experience_bar.fillAmount != amount)
        {
            amount = (float)((float)current_experience_point / (float)current_requirement_experience_point);
            lerp_time += Time.deltaTime;
            experience_bar.fillAmount = Mathf.Lerp(experience_bar.fillAmount, amount, lerp_time / 2);   
            yield return null;
        }
    }

    #endregion

    #region "Get"

    public double Experience_Point()
    {
        return current_experience_point;
    }

    #endregion
}