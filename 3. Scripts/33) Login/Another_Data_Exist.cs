using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class Another_Data_Exist : SingleTon<Another_Data_Exist>
{
    private GameObject pop_up;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Another_Data_Exist()
    {
        pop_up.SetActive(true);
    }

    #endregion

    #region "Button"

    public void Logout()
    {
        pop_up.SetActive(false);

        var bro = Backend.BMember.Logout();

        if (bro.IsSuccess())
        {
            Error_Message.instance.Set_Error_Message("Logout_Success");
        }

        Login.instance.buttons.SetActive(true);
    }

    public void Continue_Login()
    {
        pop_up.SetActive(false);

        Anti_Cheat_Manager.instance.Remove_All();
        Back_End_Controller.instance.StartCoroutine(Back_End_Controller.instance.Initialize_User_Data());
    }

    #endregion
}