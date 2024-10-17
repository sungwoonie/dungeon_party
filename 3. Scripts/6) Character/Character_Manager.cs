using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Manager : SingleTon<Character_Manager>
{
    private Character_Controller[] character_controllers;
    private string[] equipped_class;

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
        character_controllers = GetComponentsInChildren<Character_Controller>();
    }

    public void Initialize_Data(string[] equipped_class)
    {
        this.equipped_class = equipped_class;

        for (int i = 0; i < equipped_class.Length; i++)
        {
            Change_Class(i, equipped_class[i]);
        }
    }

    #endregion

    #region "Set"

    public void Set_Characters_To_Max_Health()
    {
        foreach (Character_Controller character_controller in character_controllers)
        {
            character_controller.Revive_Character();
        }
    }

    public void Change_Class(int class_type, string class_name)
    {
        equipped_class[class_type] = class_name;

        character_controllers[class_type].Set_Character_Class(class_name);
        Save_Data();
        Debug_Manager.Debug_In_Game_Message($"{class_type} class changed to {class_name}");
    }

    public void Set_All_Characters_To_Offset_Position()
    {
        foreach (var character_controller in character_controllers)
        {
            character_controller.Set_To_Offset();
        }
    }

    #endregion

    #region "Get"

    public string Get_Current_Class_Name(int class_type)
    {
        string class_name = equipped_class[class_type];
        return class_name;
    }

    #endregion

    #region "Revive"

    public void Revive(int class_type)
    {
        character_controllers[class_type].Revive_Character();
        character_controllers[class_type].Set_Dead(false);
    }

    public void Revive_All()
    {
        foreach (Character_Controller character_controller in character_controllers)
        {
            if (character_controller.Get_Dead())
            {
                character_controller.Revive_Character();
            }
        }
    }

    #endregion

    #region "Dead"

    public bool Is_All_Dead()
    {
        foreach (Character_Controller character_controller in character_controllers)
        {
            if (!character_controller.Get_Dead())
            {
                return false;
            }
        }

        return true;
    }

    public void Character_Dead(int class_type)
    {
        character_controllers[class_type].Set_Dead(true);

        if (Is_All_Dead())
        {
            Audio_Controller.instance.Play_Audio(1, "Dead");
            Event_Bus.Publish(Game_State.All_Dead);
        }
    }

    #endregion

    #region "Damage"

    public void Give_Damage_To_Alive_Character(double damage)
    {
        foreach (Character_Controller character_controller in character_controllers)
        {
            if (!character_controller.Get_Dead() && character_controller.Get_Ready_To_Combat())
            {
                character_controller.Get_Damage(damage);
                return;
            }
        }

        foreach (Character_Controller character_controller in character_controllers)
        {
            if (!character_controller.Get_Dead())
            {
                character_controller.Get_Damage(damage);
                return;
            }
        }
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        Equipped_Stats.instance.Save_Data_Class(equipped_class);
    }

    #endregion
}