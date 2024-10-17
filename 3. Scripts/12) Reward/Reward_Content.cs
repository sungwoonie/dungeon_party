using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Reward_Content : MonoBehaviour
{
    public Image reward_icon;
    public TMP_Text amount_text;

    public GameObject[] grade_icons;
    public TMP_Text rank_text;

    public string reward_name;

    #region "Unity"

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region "Set"

    public void Set_Reward_Content_Budget(double amount)
    {
        if (amount <= 0)
        {
            return;
        }
        else
        {
            amount_text.text = Text_Change.ToCurrencyString(amount);
            gameObject.SetActive(true);
        }
    }

    public void Set_Reward_Content_Paid_Stat(string stat_name)
    {
        if (string.IsNullOrEmpty(stat_name))
        {
            return;
        }
        else
        {
            Paid_Stat stat = Stat_Manager.instance.Get_Paid_Stat(stat_name);
            reward_icon.sprite = stat.stat_icon;

            for (int i = 0; i < grade_icons.Length; i++)
            {
                if (i <= stat.grade)
                {
                    grade_icons[i].SetActive(true);
                }
                else
                {
                    grade_icons[i].SetActive(false);
                }
            }

            rank_text.text = stat.rank.ToString();

            gameObject.SetActive(true);
        }
    }

    #endregion
}