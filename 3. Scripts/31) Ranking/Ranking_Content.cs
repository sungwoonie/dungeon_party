using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ranking_Content : MonoBehaviour
{
    public TMP_Text user_name_text;
    public TMP_Text amount_text;
    public TMP_Text rank_text;

    public int rank;

    public void Set_Ranking_Content(Ranking target_ranking, bool cpp)
    {
        user_name_text.text = target_ranking.user_name;
        rank_text.text = target_ranking.rank.ToString();

        if (cpp)
        {
            amount_text.text = double.Parse(target_ranking.ranking_amount.ToString()).ToCurrencyString();
        }
        else
        {
            amount_text.text = target_ranking.ranking_amount.ToString();
        }
    }
}
