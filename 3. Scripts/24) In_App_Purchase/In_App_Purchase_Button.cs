using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class In_App_Purchase_Button : MonoBehaviour
{
    public string product_name;

    private Button button;
    private TMP_Text price_text;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        Set_Price_Text();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        button = GetComponent<Button>();
        price_text = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(() => Click_Purchase_Button());
    }

    #endregion

    #region "Set"

    private void Set_Price_Text()
    {
        price_text.text = In_App_Purchase_Manager.instance.Get_Product_Local_Price(product_name);
    }

    #endregion

    #region "Purchase"

    private void Click_Purchase_Button()
    {
        In_App_Purchase_Manager.instance.Start_Purchase_Product(product_name);
    }

    #endregion
}