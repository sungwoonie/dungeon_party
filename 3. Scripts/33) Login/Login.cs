using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;

public class Login : SingleTon<Login>
{
    public TMP_InputField id_input_field;
    public TMP_InputField password_input_field;

    public Button login_button;
    public GameObject buttons;
    public GameObject nick_name_object;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    private void Start()
    {
        Auto_Login();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        login_button.onClick.AddListener(() => Login_Button());
    }

    #endregion

    #region "Auto Login"

    private void Auto_Login()
    {
        Debug_Manager.Debug_Server_Message("Auto login start");

        BackendReturnObject bro = Backend.BMember.LoginWithTheBackendToken();

        if (bro.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message("Auto login success");

            Check_User_Name();
        }
        else
        {
            Debug_Manager.Debug_Server_Message("Auto login failed. No data");

            buttons.SetActive(true);
        }
    }

    #endregion

    #region "Login"

    public void Login_Button()
    {
        login_button.interactable = false;

        BackendReturnObject bro = Backend.BMember.CustomLogin(id_input_field.text, password_input_field.text);

        if (bro.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Login Success!");

            Check_User_Name();
            login_button.interactable = true;
        }
        else
        {
            login_button.interactable = true;
            Error_Message.instance.Set_Error_Message($"Login_Error_{bro.GetStatusCode()}");
        }
    }

    private void Check_User_Name()
    {
        Debug_Manager.Debug_Server_Message($"Check User Name Start");

        BackendReturnObject bro = Backend.BMember.GetUserInfo();

        if (bro.GetReturnValuetoJSON()["row"]["nickname"] == null)
        {
            Debug_Manager.Debug_Server_Message($"User Name is Empty");

            nick_name_object.SetActive(true);
        }
        else
        {
            Debug_Manager.Debug_Server_Message($"User Name is not Empty. Continue Login");

            Start_Initialize();
        }
    }

    public void Start_Initialize()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        buttons.SetActive(false);
        nick_name_object.SetActive(false);
        Back_End_Controller.instance.StartCoroutine(Back_End_Controller.instance.Initialize_User_Data());
    }

    #endregion
}