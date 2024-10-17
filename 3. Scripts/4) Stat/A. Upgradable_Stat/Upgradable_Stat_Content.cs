using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Upgradable_Stat_Content : MonoBehaviour
{
    public Upgradable_Stat upgradable_stat;

    public TMP_Text level_text;
    public TMP_Text price_text;
    public TMP_Text ratio_text;

    private Button enhance_button;

    public bool can_upgrade = true;

    protected Upgradable_Stat_Manager upgradable_stat_manager;
    public int times_count;

    protected const string max_text = "MAX";
    protected const string ratio_color = "<color=#FFF0AA>";

    #region "Unity"

    protected virtual void Awake()
    {
        Initialize_Component();
    }

    protected virtual void Start()
    {
        Set_Content();
    }

    #endregion

    #region "Initialize"

    public virtual void Initialize_Component()
    {
        if (upgradable_stat_manager == null)
        {
            Initialize_Enhance_Button();

            upgradable_stat_manager = GetComponentInParent<Upgradable_Stat_Manager>(true);
        }
    }

    public virtual void Initialize_Enhance_Button()
    {
        if (GetComponentInChildren<Button>(true) != null)
        {
            if (enhance_button == null)
            {
                enhance_button = GetComponentInChildren<Button>(true);
            }
            else
            {
                enhance_button.onClick.RemoveAllListeners();
            }

            enhance_button.onClick.AddListener(() => Enhance_Button());
        }
    }

    #endregion

    #region "Set"

    /// <summary>
    /// �������� ���� �ؽ�Ʈ�� ���� �ؽ�Ʈ, ���� �ؽ�Ʈ�� �����ϴ� �Լ�
    /// </summary>
    public virtual void Set_Content()
    {
        times_count = upgradable_stat_manager.times_count;

        if (Is_Max_Level())
        {
            //���� ���� ������ ������ �ִ� �����̶�� �ִ� ������ �°� ����
            Set_Max_Text();
        }
        else
        {
            //���� ������ ����ؼ� ����
            Set_Level_Text();
            Set_Ratio_Text();
            Set_Price_Text();
        }
    }

    public virtual void Set_Max_Text()
    {
        price_text.text = max_text;
        level_text.text = max_text;
    }

    /// <summary>
    /// ���� ������ ����ؼ� ���� �ؽ�Ʈ ����
    /// </summary>
    public virtual void Set_Level_Text(){}

    /// <summary>
    /// ���� ������ ����ؼ� ���� �ؽ�Ʈ ����
    /// </summary>
    public virtual void Set_Ratio_Text(){}

    /// <summary>
    /// ���� ������ ����ؼ� ���� �ؽ�Ʈ ����
    /// </summary>
    public virtual void Set_Price_Text()
    {
        int target_level = upgradable_stat.level + times_count;
        double price = upgradable_stat.price_offset.Get_Total_Price(upgradable_stat.level, target_level);
        string price_string = Text_Change.ToCurrencyString(price);

        price_text.text = price_string;
    }

    #endregion

    #region "Bool"

    /// <summary>
    /// ���� ������ ������ ���� �Ŵ����� max_level ���� �����Ͽ� max_level ���� �ƴ��� ����
    /// </summary>
    /// <returns></returns>
    public bool Is_Max_Level()
    {
        if (upgradable_stat.level >= upgradable_stat_manager.max_level)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region "Button"

    /// <summary>
    /// ��ȭ ��ư
    /// </summary>
    public virtual void Enhance_Button()
    {
        if (Is_Max_Level())
        {
            Debug_Manager.Debug_In_Game_Message($"{upgradable_stat.name} is already max level");
            Error_Message.instance.Set_Error_Message($"Error_Message_Already_Max_Level");
            return;
        }

        if (!can_upgrade)
        {
            Debug_Manager.Debug_In_Game_Message($"Can't upgrade. can upgrade is {can_upgrade}");
            return;
        }

        double price = upgradable_stat.price_offset.Get_Total_Price(upgradable_stat.level, upgradable_stat.level + times_count);
        string budget_type = upgradable_stat_manager.budget_type;

        if (Budget_Manager.instance.Can_Use_Budget(budget_type, price))
        {
            //enhance
            Budget_Manager.instance.Use_Budget(budget_type, price);

            upgradable_stat.Modify_Data("level", upgradable_stat.level + times_count);
            Set_Content();

            Debug_Manager.Debug_In_Game_Message($"{upgradable_stat.name} upgraded to {upgradable_stat.level}");
        }
        else
        {
            //not enough money
            Debug_Manager.Debug_In_Game_Message($"not enough money to upgrade {upgradable_stat.name}");
            Error_Message.instance.Set_Error_Message("Error_Message_Not_Enough_Budget");
        }
    }

    #endregion
}