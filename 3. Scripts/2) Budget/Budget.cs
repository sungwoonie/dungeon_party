using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Budget
{
    public double gold;
    public double beyond_stone;
    public double enhance_stone;
    public double ability_stone;
    public double diamond;
    public double key;

    public List<double> paid_diamond = new List<double>();

    public Budget()
    {

    }

    public Budget(Server_Budget_Data data)
    {
        gold = data.gold.FromCurrencyString();
        beyond_stone = data.beyond_stone.FromCurrencyString();
        enhance_stone = data.enhance_stone.FromCurrencyString();
        ability_stone = data.ability_stone.FromCurrencyString();
        diamond = data.diamond.FromCurrencyString();
        key = data.key.FromCurrencyString();

        paid_diamond = data.paid_diamond;
    }

    #region "Diamond"

    public double Get_Paid_Diamond()
    {
        double paid_diamond_amount = 0.0;

        foreach (var item in paid_diamond)
        {
            paid_diamond_amount += item;
        }

        return paid_diamond_amount;
    }

    public double Get_Diamond()
    {
        return Get_Paid_Diamond() + diamond;
    }

    public void Use_Diamond(double amount)
    {
        double remaining_amount = amount;

        if (Get_Paid_Diamond() > 0)
        {
            for (int i = paid_diamond.Count - 1; i >= 0; i--)
            {
                if (remaining_amount >= paid_diamond[i])
                {
                    remaining_amount -= paid_diamond[i];
                    paid_diamond.RemoveAt(i);
                }
                else
                {
                    paid_diamond[i] -= remaining_amount;
                    remaining_amount = 0;
                    break;
                }
            }

            if (remaining_amount > 0)
            {
                diamond -= remaining_amount;
            }
        }
        else
        {
            diamond -= remaining_amount;
        }
    }

    #endregion

    #region "Budget"

    public void Add_Budget(Budget new_budget)
    {
        gold += new_budget.gold;
        beyond_stone += new_budget.beyond_stone;
        ability_stone += new_budget.ability_stone;
        diamond += new_budget.diamond;
        key += new_budget.key;
        enhance_stone += new_budget.key;
    }

    public void Get_New_Budget(Budget new_budget)
    {
        if (new_budget == null)
        {
            return;
        }

        Budget_Manager.instance.Earn_Budget("gold", new_budget.gold);
        Budget_Manager.instance.Earn_Budget("beyond_stone", new_budget.beyond_stone);
        Budget_Manager.instance.Earn_Budget("ability_stone", new_budget.ability_stone);
        Budget_Manager.instance.Earn_Budget("diamond", new_budget.diamond);
        Budget_Manager.instance.Earn_Budget("key", new_budget.key);
        Budget_Manager.instance.Earn_Budget("enhance_stone", new_budget.enhance_stone);
    }

    public void Use_Budget(string budget_type, double amount)
    {
        Set_Budget_Value(budget_type, Get_Budget_Value(budget_type) - amount);
    }

    public double Get_Budget_Value(string budget_type)
    {
        var target_budget = GetType().GetField(budget_type).GetValue(this);
        return double.Parse(target_budget.ToString());
    }

    public void Set_Budget_Value(string budget_type, double budget)
    {
        GetType().GetField(budget_type).SetValue(this, budget);
    }

    #endregion
}