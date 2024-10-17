using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Budget_Bar : MonoBehaviour
{
    public TMP_Text[] budget_texts;
    public Image icon;

    private string current_budget = string.Empty;

    #region "Unity"

    private void Start()
    {
        Set_Budget_Bar(string.Empty);
    }

    #endregion

    #region "Set"

    public void Set_Budget_Bar(string budget_type)
    {
        if (string.IsNullOrEmpty(budget_type))
        {
            budget_type = "gold";
        }

        double budget = Budget_Manager.instance.Get_Budget(budget_type);
        budget_texts[1].text = Text_Change.ToCurrencyString(budget);

        if (!current_budget.Equals(budget_type))
        {
            icon.sprite = Resources.Load<Sprite>("3. Icon/" + budget_type + "_Icon");
            current_budget = budget_type;
        }

        double diamond = Budget_Manager.instance.Get_Budget("diamond");
        budget_texts[0].text = Text_Change.ToCurrencyString(diamond);

        Debug_Manager.Debug_In_Game_Message($"Budget bar setted to {budget_type}");
    }

    public void Set_Budget_Text(string budget_type)
    {
        if (budget_type.Equals("diamond"))
        {
            budget_texts[0].text = Text_Change.ToCurrencyString(Budget_Manager.instance.current_budget.Get_Diamond());
        }
        else if (budget_type.Equals(current_budget))
        {
            double budget = Budget_Manager.instance.Get_Budget(budget_type);
            budget_texts[1].text = Text_Change.ToCurrencyString(budget);
        }
    }

    #endregion
}