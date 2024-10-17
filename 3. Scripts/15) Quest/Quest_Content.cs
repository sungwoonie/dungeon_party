using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest_Content : MonoBehaviour
{
    public Quest_Struct current_quest;
    public bool repeat;
    public bool daily;

    public Image icon_image;

    public TMP_Text process_text;

    public TMP_Text reward_text;
    public Image reward_icon_image;

    public Localization_Text title_text;

    public GameObject[] complete_objects;

    public bool complete;
    public bool received;

    private Button button;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => Get_Reward());
    }

    #endregion

    #region "Reward"

    protected virtual void Get_Reward()
    {
        if (complete && !received)
        {
            received = true;

            Reward();

            if (repeat)
            {
                Quest_Manager.instance.Repeat_Quest_Complete();
            }
        }
    }

    private void Reward()
    {
        if (current_quest.reward_name.Equals("experience_point"))
        {
            Party_Level_Manager.instance.Get_Experience_Point(current_quest.reward);
        }
        else
        {
            Budget_Manager.instance.Earn_Budget(current_quest.reward_name, current_quest.reward);
        }

        Quest_Manager.instance.Get_Reward(this, repeat, daily);
    }

    #endregion

    #region "Requirement"

    public int Increase_Current_Requirement(int amount)
    {
        current_quest.current_requirement += amount;

        if (current_quest.current_requirement >= current_quest.requirement)
        {
            current_quest.current_requirement = current_quest.requirement;

            if (complete == false)
            {
                Quest_Complete();
            }
        }

        Set_Process();
        return current_quest.current_requirement;
    }

    public void Set_Requirement(int amount)
    {
        current_quest.current_requirement += amount;

        if (current_quest.current_requirement >= current_quest.requirement)
        {
            current_quest.current_requirement = current_quest.requirement;
            complete = true;
        }

        Set_Process();
    }

    #endregion

    #region "Set"

    protected virtual void Set_Received_Icon()
    {

    }

    protected virtual void Set_Process()
    {
        process_text.text = $"{current_quest.current_requirement} / {current_quest.requirement}";
    }

    protected virtual void Quest_Complete()
    {
        complete = true;

        Set_Complete_Object();
        Quest_Manager.instance.Quest_Complete(daily, repeat);
    }

    public virtual void Set_New_Quest(Quest_Struct new_quest, bool _received, int _requirement)
    {
        current_quest = new_quest;
        complete = false;
        received = _received;

        Set_Requirement(_requirement);

        Set_Icon();
        //Set_Process();
        Set_Text();
        Set_Complete_Object();
        Set_Received_Icon();
    }

    public void Set_Icon()
    {
        icon_image.sprite = Resources.Load<Sprite>($"3. Icon/{current_quest.quest_name.Upper_First_Char_By_Underline()}_Icon");
        reward_icon_image.sprite = Resources.Load<Sprite>($"3. Icon/{current_quest.reward_name.Upper_First_Char_By_Underline()}_Icon");
    }

    protected virtual void Set_Text()
    {
        title_text.Set_Localization_Key(current_quest.quest_name + "_title");
        title_text.Localize_Text();

        if (repeat)
        {
            reward_text.text = current_quest.reward.ToString();
        }
        else
        {
            reward_text.text = double.Parse(current_quest.reward.ToString()).ToCurrencyString();
        }
    }

    protected virtual void Set_Complete_Object()
    {
        foreach (GameObject complete_object in complete_objects)
        {
            complete_object.SetActive(complete);
        }
    }

    #endregion
}