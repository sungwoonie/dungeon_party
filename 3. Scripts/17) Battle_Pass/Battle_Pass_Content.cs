using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Battle_Pass_Content : MonoBehaviour
{
    public Image reward_icon;
    public Localization_Text reward_text;
    public TMP_Text reward_amount_text;

    public GameObject[] complete_objects;
    public GameObject[] default_objects;
    public GameObject received_object;

    private Battle_Pass_Struct current_struct;
    private Battle_Pass pass;

    private Notify notify;

    private bool reached;
    private bool received;
    private bool can_receive;

    #region "Initialize"

    public void Initialize_Component()
    {
        if (!pass)
        {
            pass = GetComponentInParent<Battle_Pass>(true);
            notify = GetComponent<Notify>();
        }
    }

    #endregion

    #region "Received"

    public void Set_Received(bool _received)
    {
        received = _received;
        received_object.SetActive(received);
    }

    #endregion

    #region "Reward"

    public void Get_Reward()
    {
        if (reached && !received && can_receive)
        {
            received_object.SetActive(true);

            notify.Set_Off_Notify();
            pass.Get_Reward(this);

            received = true;

            string[] reward_name_split = current_struct.reward_name.Split("_");

            switch (reward_name_split[0])
            {
                case "class":
                    Stat_Manager.instance.class_stat_manager.Get_New_Stat(current_struct.reward_name.Upper_First_Char_By_Underline());
                    break;
                case "equipment":
                    Stat_Manager.instance.equipment_stat_manager.Get_New_Stat(current_struct.reward_name.Upper_First_Char_By_Underline());
                    break;
                case "skill":
                    Stat_Manager.instance.skill_stat_manager.Get_New_Stat(current_struct.reward_name.Upper_First_Char_By_Underline());
                    break;
                case "experience":
                    Stage_Reward_Manager.instance.Set_Current_Stage_Data(Stage_Manager.instance.Get_High_Stage()[0]);
                    double exprience_reward = Stage_Reward_Manager.instance.Get_Experience_Point(Stage_Manager.instance.Get_High_Stage()[1]);
                    Party_Level_Manager.instance.Get_Experience_Point(current_struct.reward_amount * exprience_reward);
                    break;
                case "gold":
                    Stage_Reward_Manager.instance.Set_Current_Stage_Data(Stage_Manager.instance.Get_High_Stage()[0]);
                    double gold_reward = Stage_Reward_Manager.instance.Get_Drop_Gold(Stage_Manager.instance.Get_High_Stage()[1]);
                    Budget_Manager.instance.Earn_Budget("gold", current_struct.reward_amount * gold_reward);
                    break;
                default:
                    Budget_Manager.instance.Earn_Budget(current_struct.reward_name, current_struct.reward_amount);
                    break;
            }
        }
    }

    #endregion

    #region "Set"

    private void Set_Objects()
    {
        foreach (var complete_object in complete_objects)
        {
            complete_object.SetActive(reached);
        }

        foreach (var default_object in default_objects)
        {
            default_object.SetActive(!reached);
        }
    }

    public void Set_Reached(bool reach)
    {
        reached = reach;

        Set_Objects();

        received_object.SetActive(received);

        if (reach && !received && can_receive)
        {
            notify.Set_On_Notify();
        }
    }

    public void Set_Purchased(bool purchased)
    {
        can_receive = purchased;
        Set_Reached(reached);
    }

    public void Set_Pass_Content(Battle_Pass_Struct target, bool can_receive = true)
    {
        current_struct = target;
        this.can_receive = can_receive;

        //set reached on data

        received_object.SetActive(received);

        if (Stat_Manager.instance.Get_Paid_Stat(current_struct.reward_name) != null)
        {
            reward_icon.sprite = Stat_Manager.instance.Get_Paid_Stat(current_struct.reward_name).stat_icon;
            reward_text.Set_Localization_Key(current_struct.reward_name.Upper_First_Char_By_Underline());
        }
        else
        {
            reward_icon.sprite = Resources.Load<Sprite>($"3. Icon/{current_struct.reward_name.Upper_First_Char_By_Underline()}_Pass_Icon");
            reward_text.Set_Localization_Key(current_struct.reward_name);
        }

        reward_text.Localize_Text();
        reward_amount_text.text = current_struct.reward_amount.ToCurrencyString();

        Set_Objects();
    }

    #endregion

    #region "Get"

    public bool Notified()
    {
        return notify.notify_object.activeSelf;
    }

    public bool Received()
    {
        return received;
    }

    public bool Reached()
    {
        return reached;
    }

    #endregion
}