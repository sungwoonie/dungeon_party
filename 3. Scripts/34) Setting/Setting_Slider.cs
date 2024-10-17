using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Slider : MonoBehaviour
{
    public Setting_Slider_Type type;

    private Slider setting_slider;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        setting_slider.value = Anti_Cheat_Manager.instance.Get($"Setting_Slider_{type}", 0.5f);
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        setting_slider = GetComponentInChildren<Slider>(true);
        setting_slider.onValueChanged.AddListener(Set_Setting);
    }

    #endregion

    #region "Set"

    private void Set_Setting(float amount)
    {
        //change methoed to under when other setting added
        /*
        switch (type)
        {
            case Setting_Slider_Type.BGM:
                break;
            case Setting_Slider_Type.Sound_Effect:
                break;
        }
        */

        int current_type = (int)type;
        Audio_Controller.instance.Set_Sound(current_type, amount);

        Anti_Cheat_Manager.instance.Set($"Setting_Slider_{type}", amount);
    }

    #endregion
}

public enum Setting_Slider_Type
{
    BGM, Sound_Effect
}