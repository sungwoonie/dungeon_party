using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using BackEnd;

public class Set_User_Name : MonoBehaviour
{
    public TMP_InputField user_name_input_field;

    public Button set_button;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        set_button.onClick.AddListener(() => Set_Button());
    }

    #endregion

    #region "Sign Up"

    public void Set_Button()
    {
        string user_name = user_name_input_field.text;

        if (user_name.Length < 2 || user_name.Length > 8 || Bad_Word_Censor.Contains_Bad_Word(user_name) || Contains_Special_Characters(user_name))
        {
            Error_Message.instance.Set_Error_Message($"Set_User_Name_Error");
            return;
        }

        set_button.interactable = false;
        Try_Set_User_Name(user_name);
    }

    private void Try_Set_User_Name(string user_name)
    {
        Debug_Manager.Debug_Server_Message("Start Set User Name");

        BackendReturnObject callback = Backend.BMember.CreateNickname(user_name);

        if (callback.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message("Set User Name Success!");
            Set_User_Name_Success();
        }
        else
        {
            Error_Message.instance.Set_Error_Message($"Set_User_Name_Error_{callback.GetStatusCode()}");
        }

        set_button.interactable = true;
    }

    private void Set_User_Name_Success()
    {
        Login.instance.Start_Initialize();
    }

    public bool Contains_Special_Characters(string input)
    {
        // 특수 문자 확인을 위한 정규 표현식
        return Regex.IsMatch(input, @"[^\w\s]");
    }

    #endregion
}