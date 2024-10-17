using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Accel : MonoBehaviour
{
    private Toggle toggle;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        toggle.isOn = Anti_Cheat_Manager.instance.Get("Time_Accel", false);
        Accel();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate
        {
            Accel();
        });
    }

    #endregion

    private void Accel()
    {
        if (toggle.isOn)
        {
            Game_Time.Set_Game_Time(2.0f);
        }
        else
        {
            Game_Time.Set_Game_Time(1.0f);
        }

        Anti_Cheat_Manager.instance.Set("Time_Accel", toggle.isOn);
    }
}