using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using BackEnd;

public class Forgot : MonoBehaviour
{
    public TMP_InputField password_id_input_field;
    public TMP_InputField password_email_input_field;

    public TMP_InputField id_email_input_field;

    private GameObject password_pop_up;
    private GameObject id_pop_up;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        password_pop_up = transform.GetChild(0).gameObject;
        id_pop_up = transform.GetChild(1).gameObject;
    }

    #endregion

    #region "Set Pop Up"

    public void Set_Forgot_Pop_Up(bool password)
    {
        if (password)
        {
            password_pop_up.SetActive(true);
        }
        else
        {
            id_pop_up.SetActive(true);
        }
    }

    #endregion

    #region "Button"

    public void Check_Button(bool password)
    {
        if (password)
        {
            Find_Password();
        }
        else
        {
            Find_ID();
        }
    }

    private void Find_Password()
    {
        string id = password_id_input_field.text;
        string email = password_email_input_field.text;

        var bro = Backend.BMember.ResetPassword(id, email);

        if (bro.IsSuccess())
        {
            Error_Message.instance.Set_Error_Message($"Find_Password_Success");
        }
        else
        {
            Error_Message.instance.Set_Error_Message($"Find_Password_Error_{bro.GetStatusCode()}");
        }
    }

    private void Find_ID()
    {
        string id = id_email_input_field.text;

        var bro = Backend.BMember.FindCustomID(id);

        if (bro.IsSuccess())
        {
            Error_Message.instance.Set_Error_Message($"Find_ID_Success");
        }
        else
        {
            Error_Message.instance.Set_Error_Message($"Find_ID_Error_{bro.GetStatusCode()}");
        }
    }

    #endregion
}
