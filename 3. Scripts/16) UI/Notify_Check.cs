using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify_Check : SingleTon<Notify_Check>
{
    public GameObject notify_icon;

    private List<GameObject> current_notified = new List<GameObject>();

    public void Set_Notify(bool is_on, GameObject target_icon)
    {
        if (is_on)
        {
            if (!current_notified.Contains(target_icon))
            {
                current_notified.Add(target_icon);
            }

            notify_icon.SetActive(true);
            target_icon.SetActive(true);
        }
        else
        {
            current_notified.Remove(target_icon);
            if (current_notified.Count <= 0)
            {
                notify_icon.SetActive(false);
            }

            target_icon.SetActive(false);
        }
    }
}