using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamble_Button : MonoBehaviour
{
    public int gamble_amount;
    public bool ad;

    public TMP_Text amount_text;
    public TMP_Text price_text;

    public Gamble_Content gamble_content;
    private Button button;

    #region "Initialize"

    public void Initialize_Component()
    {
        gamble_content = GetComponentInParent<Gamble_Content>(true);
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Play_Gamble());

        Initialize_Content();
    }

    private void Initialize_Content()
    {
        if (!ad)
        {
            price_text.text = (gamble_content.current_data.price * gamble_amount).ToString();
        }

        amount_text.text = $"X {gamble_amount}";
    }

    #endregion

    #region "Button"

    public void Play_Gamble()
    {
        if (ad)
        {
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_Gamble", Start_Gamble);

            Debug_Manager.Debug_In_Game_Message($"Start play {gamble_content.gamble_type} gamble. amount : {gamble_amount}. AD");
        }
        else
        {
            double price = gamble_content.current_data.price * gamble_amount;

            if (Budget_Manager.instance.Can_Use_Budget("diamond", price))
            {
                Budget_Manager.instance.Use_Budget("diamond", price);
                Start_Gamble();

                Debug_Manager.Debug_In_Game_Message($"Start play {gamble_content.gamble_type} gamble. amount : {gamble_amount}");
            }
            else
            {
                Debug_Manager.Debug_In_Game_Message($"Can't play {gamble_content.gamble_type} gamble. not enough diamond");
            }
        }
    }

    private void Start_Gamble()
    {
        gamble_content.Play_Gamble(gamble_amount, this);
    }

    #endregion
}
