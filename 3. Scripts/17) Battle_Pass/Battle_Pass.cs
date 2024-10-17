using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Battle_Pass : MonoBehaviour
{
    public Battle_Pass_Content[] battle_pass_contents;
    public Battle_Pass_Content[] free_pass_contents;
    public TMP_Text requirement_text;

    public GameObject not_purchased_object;

    [HideInInspector]public Battle_Pass_Data pass_data;
    private int current_requirement;

    private Slider slider;
    private Button reward_button;
    private Battle_Pass_Count[] pass_count;
    private Notify notify;

    private bool purchased;

    #region "Initialize"

    public void Initialize_Received(int[] received)
    {
        for (int i = 0; i < battle_pass_contents.Length; i++)
        {
            if (i < received[0])
            {
                battle_pass_contents[i].Set_Received(true);
            }
            else
            {
                battle_pass_contents[i].Set_Received(false);
            }
        }

        for (int i = 0; i < free_pass_contents.Length; i++)
        {
            if (i < received[1])
            {
                free_pass_contents[i].Set_Received(true);
            }
            else
            {
                free_pass_contents[i].Set_Received(false);
            }
        }
    }

    public void Initialize_Component()
    {
        pass_data = GetComponent<Battle_Pass_Data>();
        slider = GetComponentInChildren<Slider>(true);
        pass_count = GetComponentsInChildren<Battle_Pass_Count>(true);
        notify = GetComponent<Notify>();
        reward_button = GetComponentInChildren<Button>(true);
        reward_button.onClick.AddListener(() => Get_Reward_Button());

        pass_data.Initialize_CSV_Data();

        foreach (var battle_pass_content in battle_pass_contents)
        {
            battle_pass_content.Initialize_Component();
        }

        foreach (var free_pass_content in free_pass_contents)
        {
            free_pass_content.Initialize_Component();
        }
    }

    public void Initialize_Contents()
    {
        List<Battle_Pass_Struct> battle_pass = pass_data.Get_Battle_Pass_Structs();
        List<Battle_Pass_Struct> free_pass = pass_data.Get_Free_Pass_Structs();

        for (int i = 0; i < battle_pass.Count; i++)
        {
            battle_pass_contents[i].Set_Pass_Content(battle_pass[i], purchased);
        }

        for (int i = 0; i < free_pass.Count; i++)
        {
            free_pass_contents[i].Set_Pass_Content(free_pass[i]);
        }

        Set_Contents();

        int per_count = pass_data.Requirement() / pass_count.Length;

        for (int i = 0; i < pass_count.Length; i++)
        {
            pass_count[i].Set_Amount(per_count * (i + 1));
        }
    }

    #endregion

    #region "Set"

    private void Set_Contents()
    {
        requirement_text.text = $"<color=#ffad37>{current_requirement}</color> / {pass_data.Requirement()}";
        slider.value = (float)current_requirement / (float)pass_data.Requirement();
    }

    public void Set_Purchased_Object(bool purchased)
    {
        this.purchased = purchased;
        not_purchased_object.SetActive(!purchased);

        foreach (var battle_pass_content in battle_pass_contents)
        {
            battle_pass_content.Set_Purchased(purchased);
        }
    }

    #endregion

    #region "Requirement"

    public void Set_Requirement(int requirement)
    {
        current_requirement = requirement;
        Set_Requirement();
    }

    public void Get_Requirement(int got_requirement)
    {
        current_requirement += got_requirement;

        if (current_requirement >= pass_data.Requirement())
        {
            current_requirement = pass_data.Requirement();
        }

        Set_Requirement();
    }

    private void Set_Requirement()
    {
        for (int i = 0; i < battle_pass_contents.Length; i++)
        {
            if (battle_pass_contents[i].Reached() == false)
            {
                bool reached = ((float)pass_data.Requirement() / ((float)battle_pass_contents.Length) * (i + 1)) <= (float)current_requirement;
                battle_pass_contents[i].Set_Reached(reached);

                pass_count[i].Set_Object(reached);
            }
        }

        for (int i = 0; i < free_pass_contents.Length; i++)
        {
            if (free_pass_contents[i].Reached() == false)
            {
                bool reached = ((float)pass_data.Requirement() / ((float)free_pass_contents.Length) * (i + 1)) <= (float)current_requirement;
                free_pass_contents[i].Set_Reached(reached);
            }
        }

        Set_Contents();
    }

    #endregion

    #region "Notify"

    public void Set_Notify()
    {
        bool notified = false;

        foreach (var battle_pass_content in battle_pass_contents)
        {
            if (battle_pass_content.Notified())
            {
                notified = true;
                break;
            }
        }

        foreach (var free_pass_content in free_pass_contents)
        {
            if (free_pass_content.Notified())
            {
                notified = true;
                break;
            }
        }

        if (notified)
        {
            notify.Set_On_Notify();
        }
        else
        {
            notify.Set_Off_Notify();
        }
    }

    #endregion

    #region "Get Reward"

    public void Get_Reward_Button()
    {
        foreach (var battle_pass_content in battle_pass_contents)
        {
            battle_pass_content.Get_Reward();
        }

        foreach (var free_pass_content in free_pass_contents)
        {
            free_pass_content.Get_Reward();
        }
    }

    public void Get_Reward(Battle_Pass_Content content)
    {
        foreach (var battle_pass_content in battle_pass_contents)
        {
            if (battle_pass_content.Equals(content))
            {
                Battle_Pass_Manager.instance.Received_Reward(this, false);
                break;
            }
        }

        foreach (var free_pass_content in free_pass_contents)
        {
            if (free_pass_content.Equals(content))
            {
                Battle_Pass_Manager.instance.Received_Reward(this, true);
                break;
            }
        }
    }

    #endregion
}