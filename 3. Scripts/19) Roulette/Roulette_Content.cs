using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Roulette_Content : MonoBehaviour
{
    public TMP_Text amount_text;
    public Image icon_image;
    public GameObject focus_object;

    public Roulette_Reward current_reward;

    #region "Set"

    public void Set_Content(Roulette_Reward reward)
    {
        current_reward = reward;

        amount_text.text = current_reward.reward_amount.ToCurrencyString();
        icon_image.sprite = Resources.Load<Sprite>($"3. Icon/{current_reward.reward_name.Upper_First_Char_By_Underline()}_Icon");
    }

    #endregion

    #region "Focus"

    public void Focus_Content(bool focus)
    {
        focus_object.SetActive(focus);
    }

    #endregion
}