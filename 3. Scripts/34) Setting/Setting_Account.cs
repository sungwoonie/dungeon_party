using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

public class Setting_Account : SingleTon<Setting_Account>
{
    public Localization_Text title_text;
    public Localization_Text description_text;
    public TMP_InputField new_password_input_field;
    public TMP_InputField old_password_input_field;

    private Button button;
    private GameObject pop_up;

    private GameObject password_change_pop_up;
    private Button password_change_button;

    private bool current_status;

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
        password_change_pop_up = transform.GetChild(1).gameObject;

        password_change_button = password_change_pop_up.transform.GetComponentInChildren<Button>(true);
        password_change_button.onClick.AddListener(() => Change_Password());

        button = pop_up.transform.GetComponentInChildren<Button>(true);
        button.onClick.AddListener(() => Check_Button());
    }

    #endregion

    #region "Button"

    private void Check_Button()
    {
        if (current_status)
        {
            Logout();
        }
        else
        {
            Delete_Account();
        }
    }

    private void Change_Password()
    {
        string password = new_password_input_field.text;

        if (password.Length < 4 || password.Length > 20)
        {
            Error_Message.instance.Set_Error_Message($"Sign_Up_Password_Error");
            return;
        }

        var bro = Backend.BMember.UpdatePassword(old_password_input_field.text, password);

        if (bro.IsSuccess())
        {
            Error_Message.instance.Set_Error_Message("Password_Change_Success");
            password_change_pop_up.SetActive(false);
        }
        else
        {
            Error_Message.instance.Set_Error_Message($"Password_Change_Error_{bro.GetStatusCode()}");
        }
    }

    #endregion

    #region "Account"

    private void Delete_Account()
    {
        var bro = Backend.BMember.WithdrawAccount();

        if (bro.IsSuccess())
        {
            Anti_Cheat_Manager.instance.Remove_All();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }

    private void Logout()
    {
        Database_Controller.instance.Update_All_Data_To_Server_Asynch();

        var bro = Backend.BMember.Logout();

        if (bro.IsSuccess())
        {
            Anti_Cheat_Manager.instance.Remove_All();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }

    #endregion

    #region "Set"

    public void Set_Setting_Acount(bool logout)
    {
        string title_key = logout ? "Setting_Account_Logout" : "Setting_Account_Delete_Account";
        string description_key = logout ? "Setting_Account_Logout_Description" : "Setting_Account_Delete_Account_Description";

        title_text.Set_Localization_Key(title_key);
        description_text.Set_Localization_Key(description_key);

        title_text.Localize_Text();
        description_text.Localize_Text();

        current_status = logout;

        pop_up.SetActive(true);
    }

    public void Set_Change_Password()
    {
        password_change_pop_up.SetActive(true);
    }

    #endregion
}