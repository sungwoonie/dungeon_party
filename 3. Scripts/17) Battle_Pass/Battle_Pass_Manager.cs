using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Battle_Pass_Manager : SingleTon<Battle_Pass_Manager>
{
    private Battle_Pass[] battle_passes;

    public Server_Battle_Pass_Data current_data;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    public void Initialize_Data(Server_Battle_Pass_Data data)
    {
        current_data = data;
        Initialize_Battle_Pass();
    }

    private void Initialize_Component()
    {
        battle_passes = GetComponentsInChildren<Battle_Pass>(true);
    }

    private void Initialize_Battle_Pass()
    {
        for (int i = 0; i < battle_passes.Length; i++)
        {
            bool purchased = Convert.ToBoolean(current_data.bpp[i]);
            battle_passes[i].Set_Purchased_Object(purchased);

            battle_passes[i].Initialize_Component();
            battle_passes[i].Initialize_Contents();

            int[] received = (int[])current_data.GetType().GetField($"bprd_{i}").GetValue(current_data);
            battle_passes[i].Initialize_Received(received);

            battle_passes[i].Set_Requirement(current_data.bpr[i]);
        }
    }

    #endregion

    #region "Get Requirement"

    public void Get_Requirement(string pass_name, int amount)
    {
        for (int i = 0; i < battle_passes.Length; i++)
        {
            if (battle_passes[i].pass_data.pass_name.Equals(pass_name))
            {
                battle_passes[i].Get_Requirement(amount);
                current_data.bpr[i] += amount;
                Save_Data();
                break;
            }
        }
    }

    #endregion

    #region "Set"

    public void Battle_Pass_Purchased(bool purchased, string pass_name)
    {
        for (int i = 0; i < battle_passes.Length; i++)
        {
            if (battle_passes[i].pass_data.pass_name.Equals(pass_name))
            {
                battle_passes[i].Set_Purchased_Object(purchased);

                current_data.bpp[i] = 1;
                Save_Data();

                Debug_Manager.Debug_In_Game_Message($"{pass_name} pass is unlocked!");
                break;
            }
        }

    }

    #endregion

    #region "Save Data"

    public void Save_Data()
    {
        Anti_Cheat_Manager.instance.Set("Battle_Pass_Data", JsonUtility.ToJson(current_data));
    }

    #endregion

    #region "Receive"

    public void Received_Reward(Battle_Pass pass, bool free)
    {
        for (int i = 0; i < battle_passes.Length; i++)
        {
            if (battle_passes[i].Equals(pass))
            {
                int[] received = (int[])current_data.GetType().GetField($"bprd_{i}").GetValue(current_data);
                received[free ? 1 : 0]++;
                current_data.GetType().GetField($"bprd_{i}").SetValue(current_data, received);

                Save_Data();
                break;
            }
        }
    }

    #endregion
}
