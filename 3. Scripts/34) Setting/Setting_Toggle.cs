using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Toggle : MonoBehaviour
{
    public Setting_Toggle_Type type;

    public GameObject[] on_off;

    private Toggle setting_toggle;

    #region "Unity"

    private void Awake()
    {
        Intialize_Component();
    }

    private void Start()
    {
        setting_toggle.isOn = Anti_Cheat_Manager.instance.Get($"Setting_Toggle_{type}", true);
    }

    #endregion

    #region "Initialize"

    private void Intialize_Component()
    {
        setting_toggle = GetComponentInChildren<Toggle>(true);
        setting_toggle.onValueChanged.AddListener(Set_Setting);
    }

    #endregion

    #region "Set"

    private void Set_Setting(bool is_on)
    {
        Anti_Cheat_Manager.instance.Set($"Setting_Toggle_{type}", is_on);

        switch (type)
        {
            case Setting_Toggle_Type.Camera_Shake:
                Camera_Shake.Set_Shake(is_on);
                break;
        }

        on_off[0].SetActive(is_on);
        on_off[1].SetActive(!is_on);
    }

    #endregion
}

public enum Setting_Toggle_Type
{
    Camera_Shake
}