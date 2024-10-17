using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Exchange_Content : MonoBehaviour
{
    public string product_name;
    public bool ad;

    public TMP_Text price_text;

    private Button[] buttons;

    private Exchange_Product_Data product_data;

    #region "Initialize"

    private void Initialize_Component()
    {
        buttons = GetComponentsInChildren<Button>(true);

        buttons[0].onClick.AddListener(() => AD_Button());
        buttons[1].onClick.AddListener(() => Purchase_Button());
    }

    public void Initialize_Content()
    {
        Initialize_Component();

        product_data = Exchange_Product_Data_Manager.instance.Get_Product_Data(product_name);

        if (price_text)
        {
            price_text.text = product_data.price.ToString();
        }
    }

    #endregion

    #region "Button"

    private void AD_Button()
    {
        Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up($"AD_Reward_{product_name}", Give_Reward);
    }

    private void Purchase_Button()
    {
        if (Budget_Manager.instance.Can_Use_Budget("diamond", product_data.price))
        {
            Budget_Manager.instance.Use_Budget("diamond", product_data.price);

            Give_Reward();
        }
    }

    #endregion

    #region "Reward"

    private void Give_Reward()
    {
        Budget_Manager.instance.Earn_Budget(product_data.reward_type, product_data.reward_amount);
    }

    #endregion
}
