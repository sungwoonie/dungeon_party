using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Dungeon_Detail_Pop_Up : SingleTon<Dungeon_Detail_Pop_Up>
{
    public Localization_Text title_text;
    public TMP_Text reward_text;

    public GameObject pop_up;
    public GameObject dungeon_pop_up;

    private Dungeon_Type current_dungeon;

    private Toggle stage_toggle;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        stage_toggle = GetComponentInChildren<Toggle>(true);
        stage_toggle.onValueChanged.AddListener(delegate
        {
            Set_Detail_Amounts();
        });
    }

    #endregion

    #region "Set Detail"

    private void Set_Detail_Amounts()
    {
        string expect_reward = string.Empty;
        int[] high_stage = stage_toggle.isOn ? Stage_Manager.instance.Get_Current_Stage() : Stage_Manager.instance.Get_High_Stage();
        Stage_Reward_Manager.instance.Set_Current_Stage_Data(high_stage[0]);

        switch (current_dungeon)
        {
            case Dungeon_Type.Equipment:
                expect_reward = Dungeon_Reward_Manager.instance.Get_Reward_Equipment(8).Count.ToString() + Localization_Manager.instance.Get_Localized_String("Dungeon_Equipment_Max_Reward");
                break;
            case Dungeon_Type.Experience_Point:
                expect_reward = (Stage_Reward_Manager.instance.Get_Experience_Point(high_stage[1]) * 100 * 8).ToCurrencyString();
                break;
            case Dungeon_Type.Beyond_Stone:
                expect_reward = Dungeon_Reward_Manager.instance.Get_Beyond_Stone(8, high_stage).ToCurrencyString();
                break;
            case Dungeon_Type.Enhance_Stone:
                expect_reward = Dungeon_Reward_Manager.instance.Get_Enhance_Stone(8, high_stage).ToCurrencyString();
                break;
            case Dungeon_Type.Gold:
                expect_reward = (Stage_Reward_Manager.instance.Get_Drop_Gold(high_stage[1]) * 5 * 8).ToCurrencyString();
                break;
        }

        reward_text.text = expect_reward;
    }

    public void Set_Dungeon_Detail_Pop_Up(Dungeon_Type dungeon_type)
    {
        current_dungeon = dungeon_type;

        string title_localization_key = $"{current_dungeon}_Dungeon_Title";
        title_text.Set_Localization_Key(title_localization_key);
        title_text.Localize_Text();

        Set_Detail_Amounts();

        pop_up.SetActive(true);
    }

    #endregion

    #region "Button"

    public void Enter_Sweep()
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            Error_Message.instance.Set_Error_Message("Error_Massage_Already_Dungeon");
            return;
        }

        if (Budget_Manager.instance.Can_Use_Budget("key", 5))
        {
            Budget_Manager.instance.Use_Budget("key", 5);

            Sweep();
        }
    }

    public void Enter_Dungeon(int boost_amount)
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            Error_Message.instance.Set_Error_Message("Error_Massage_Already_Dungeon");
            return;
        }

        double price = 0.0f;

        switch (boost_amount)
        {
            case 1:
                price = 3.0f;
                break;
            case 3:
                price = 10.0f;
                break;
            case 10:
                price = 50.0f;
                break;
            default:
                price = 3.0f;
                break;
        }

        if (Budget_Manager.instance.Can_Use_Budget("key", price))
        {
            Budget_Manager.instance.Use_Budget("key", price);

            Enter(boost_amount);
        }
    }

    public void Enter_Dungeon_AD()
    {
        if (Dungeon_Manager.instance.In_Dungeon())
        {
            Error_Message.instance.Set_Error_Message("Error_Massage_Already_Dungeon");
            return;
        }

        Reward_AD_Pop_Up.instance.Set_Reward_AD_Pop_Up("AD_Reward_Enter_Dungeon", Enter_AD);
    }

    private void Enter_AD()
    {
        pop_up.SetActive(false);
        dungeon_pop_up.SetActive(false);

        Dungeon_Manager.instance.Enter_Dungeon(current_dungeon, 3, stage_toggle.isOn);
    }

    private void Enter(int boost_amount)
    {
        pop_up.SetActive(false);
        dungeon_pop_up.SetActive(false);

        Dungeon_Manager.instance.Enter_Dungeon(current_dungeon, boost_amount, stage_toggle.isOn);
    }

    private void Sweep()
    {
        Dungeon_Manager.instance.Sweep_Dungeon(current_dungeon);
    }

    #endregion
}