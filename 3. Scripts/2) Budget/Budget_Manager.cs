using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Budget_Manager : SingleTon<Budget_Manager>
{
    public Budget current_budget;
    public Budget_Bar budget_bar;

    #region "Set Data From Server"

    public void Initialize_Data(Server_Budget_Data budget_data)
    {
        current_budget.gold = budget_data.gold.FromCurrencyString();
        current_budget.beyond_stone = budget_data.beyond_stone.FromCurrencyString();
        current_budget.ability_stone = budget_data.ability_stone.FromCurrencyString();
        current_budget.enhance_stone = budget_data.enhance_stone.FromCurrencyString();
        current_budget.key = budget_data.key.FromCurrencyString();
        current_budget.diamond = budget_data.diamond.FromCurrencyString();

        current_budget.paid_diamond = budget_data.paid_diamond;
    }

    #endregion

    #region "Budget_Bar"

    public void Set_Budget_Bar(string budget_type)
    {
        budget_bar.Set_Budget_Bar(budget_type);
        budget_bar.gameObject.SetActive(true);
    }

    #endregion

    #region "Get Value"

    public bool Can_Use_Budget(string budget_type, double amount)
    {
        double budget = budget_type.Equals("diamond") ? current_budget.Get_Diamond() : current_budget.Get_Budget_Value(budget_type);

        if (budget < amount)
        {
            Error_Message.instance.Set_Error_Message($"Error_Message_Not_Enough_Budget");
        }

        return budget >= amount;
    }

    public double Get_Budget(string budget_type)
    {
        return budget_type.Equals("diamond") ? current_budget.Get_Diamond() : current_budget.Get_Budget_Value(budget_type);
    }

    #endregion

    #region "Use Value"

    public void Use_Budget(string budget_type, double amount)
    {
        if (budget_type.Equals("diamond"))
        {
            current_budget.Use_Diamond(amount);
        }
        else
        {
            current_budget.Use_Budget(budget_type, amount);
        }

        budget_bar.Set_Budget_Text(budget_type);

        Save_Data();

        Debug_Manager.Debug_In_Game_Message($"{amount} {budget_type} used");
    }

    #endregion

    #region "Earn"

    public void Get_Paid_Diamond(double amount)
    {
        current_budget.paid_diamond.Add(amount);
        budget_bar.Set_Budget_Text("diamond");

        Save_Data();

        Debug_Manager.Debug_In_Game_Message($"{amount} paid diamond added");
    }

    public void Earn_New_Budget(Budget new_budget)
    {
        current_budget.Get_New_Budget(new_budget);

        Debug_Manager.Debug_In_Game_Message($"earned {new_budget}");
    }

    public void Earn_Budget(string budget_type, double amount)
    {
        if (amount <= 0)
        {
            return;
        }

        if (budget_type.Equals("gold"))
        {
            amount += amount * Stat_Manager.instance.Calculate_Stat(40) * 0.01f;
        }

        var target_budget = current_budget.Get_Budget_Value(budget_type);
        double budget = double.Parse(target_budget.ToString());

        double save_data = 0.0f;

        if (budget + amount > double.MaxValue)
        {
            save_data = double.MaxValue;
        }
        else
        {
            save_data = budget + amount;
        }

        current_budget.Set_Budget_Value(budget_type, save_data);

        budget_bar.Set_Budget_Text(budget_type);

        Reward_Label_Controller.instance.Set_Reward(budget_type, amount.ToCurrencyString());

        Save_Data();

        Debug_Manager.Debug_In_Game_Message($"earned {amount} {budget_type}");
    }

    #endregion

    #region "Get"

    public bool Is_Budget(string name)
    {
        return current_budget.GetType().GetField(name) != null;
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        Anti_Cheat_Manager.instance.Set("Budget", JsonUtility.ToJson(current_budget));
    }

    #endregion
}