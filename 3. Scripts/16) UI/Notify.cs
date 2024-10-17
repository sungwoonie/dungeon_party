using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify : MonoBehaviour
{
    public Notify other_notify;
    public GameObject notify_object;

    private int notify_count;

    public void Set_On_Notify()
    {
        if (notify_object)
        {
            notify_count++;
            notify_object.SetActive(true);
        }

        if (other_notify)
        {
            other_notify.Set_On_Notify();
        }
    }

    public void Set_Off_Notify(bool reset = false)
    {
        if (notify_object)
        {
            notify_count = reset ? 0 : notify_count - 1;

            if (notify_count <= 0)
            {
                notify_object.SetActive(false);
                notify_count = 0;
            }
        }

        if (other_notify)
        {
            other_notify.Set_Off_Notify();
        }
    }

    public bool Notified()
    {
        return notify_count > 0;
    }
}
