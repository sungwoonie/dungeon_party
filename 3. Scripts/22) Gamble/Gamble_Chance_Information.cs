using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gamble_Chance_Information : SingleTon<Gamble_Chance_Information>
{
    public TMP_Text[] rank_chance_texts;
    public TMP_Text[] grade_chance_texts;

    public GameObject grade_chance_pop_up;

    #region "Set"

    public void Set_Chance_Information(float[] rank_chances, float[] grade_chances)
    {
        for (int i = 0; i < rank_chance_texts.Length; i++)
        {
            rank_chance_texts[i].text = rank_chances[i].ToString() + "%";
        }

        if (grade_chances[0] == 0)
        {
            grade_chance_pop_up.SetActive(false);
        }
        else
        {
            grade_chance_pop_up.SetActive(true);

            for (int i = 0; i < grade_chance_texts.Length; i++)
            {
                grade_chance_texts[i].text = grade_chances[i].ToString() + "%";
            }
        }
    }

    #endregion
}