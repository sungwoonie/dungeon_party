using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terms_Pop_Up : MonoBehaviour
{
    private Toggle[] toggles;

    private void Awake()
    {
        Initialize_Components();
    }

    private void Start()
    {
        Set_Pop_Up();
    }

    private void Initialize_Components()
    {
        toggles = GetComponentsInChildren<Toggle>(true);
    }

    private void Set_Pop_Up()
    {
        int checked_on = PlayerPrefs.GetInt("Terms", 0);

        if (checked_on == 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Check_Button()
    {
        foreach (var toggle in toggles)
        {
            if (toggle.isOn == false)
            {
                Error_Message.instance.Set_Error_Message("Error_Message_Check_Term");
                return;
            }
        }

        PlayerPrefs.SetInt("Terms", 1);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
