using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using BackEnd;

public class Sign_Up : MonoBehaviour
{
    public TMP_InputField id_input_field;
    public TMP_InputField password_input_field;
    public TMP_InputField email_input_field;

    public Button sign_up_button;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        sign_up_button.onClick.AddListener(() => Sign_Up_Button());
    }

    #endregion

    #region "Sign Up"

    public void Sign_Up_Button()
    {
        string id = id_input_field.text;
        string password = password_input_field.text;
        string email = email_input_field.text;

        if (id.Length < 4 || id.Length > 12 || !Is_English_Only(id) || Bad_Word_Censor.Contains_Bad_Word(id))
        {
            Error_Message.instance.Set_Error_Message($"Sign_Up_ID_Error");
            return;
        }

        if (password.Length < 4 || password.Length > 20)
        {
            Error_Message.instance.Set_Error_Message($"Sign_Up_Password_Error");
            return;
        }

        if (!email.Contains("@"))
        {
            Error_Message.instance.Set_Error_Message($"Sign_Up_Email_Error");
            return;
        }

        sign_up_button.interactable = false;
        Try_Sign_Up(id, password);
    }

    private void Try_Sign_Up(string id, string password)
    {
        BackendReturnObject callback = Backend.BMember.CustomSignUp(id, password);

        if (callback.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message("Sign Up Success!");
            Set_Email();
        }
        else
        {
            Error_Message.instance.Set_Error_Message($"Sign_Up_Error_{callback.GetStatusCode()}");
        }

        sign_up_button.interactable = true;
    }

    private void Set_Email()
    {
        Debug_Manager.Debug_Server_Message($"Start Set email");

        var bro = Backend.BMember.UpdateCustomEmail(email_input_field.text);

        if (bro.IsSuccess())
        {
            Debug_Manager.Debug_Server_Message($"Setting Email Success. Sign Up methoed end. turn off pop up");
        }
        else
        {
            Debug_Manager.Debug_Server_Message($"Setting Email is failed");
        }

        transform.GetChild(0).gameObject.SetActive(false);
    }

    public static bool Is_English_Only(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
    }

    #endregion
}
