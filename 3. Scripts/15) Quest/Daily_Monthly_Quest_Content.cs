using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Daily_Monthly_Quest_Content : Quest_Content
{
    public GameObject received_icon;
    public Slider process_slider;
    public Localization_Text description_text;
    public GameObject[] default_objects;

    private Notify notify;

    #region "Set"

    protected override void Set_Received_Icon()
    {
        base.Set_Received_Icon();

        received_icon.SetActive(received);

        if (notify == null)
        {
            notify = GetComponent<Notify>();
        }

        if (received == false && complete == true)
        {
            notify.Set_On_Notify();
        }
        else
        {
            //notify.Set_Off_Notify();
        }
    }

    protected override void Set_Complete_Object()
    {
        base.Set_Complete_Object();

        foreach (GameObject default_object in default_objects)
        {
            default_object.SetActive(true);
        }
    }

    protected override void Set_Process()
    {
        base.Set_Process();

        process_slider.value = (float)current_quest.current_requirement / (float)current_quest.requirement;
    }

    protected override void Set_Text()
    {
        base.Set_Text();

        description_text.Set_Localization_Key(current_quest.quest_name + "_description");
        description_text.Localize_Text();
    }

    #endregion

    #region "Complete"

    protected override void Quest_Complete()
    {
        base.Quest_Complete();

        if (received == false && complete == true)
        {
            notify.Set_On_Notify();
        }
        else
        {
            notify.Set_Off_Notify();
        }
    }

    #endregion

    #region "Reward"

    protected override void Get_Reward()
    {
        base.Get_Reward();
        received_icon.SetActive(received);
        notify.Set_Off_Notify();
    }

    #endregion
}