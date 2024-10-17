using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Setting_Button : MonoBehaviour
{
    public Setting_Button_Type type;

    private Button button;
    private TMP_Text text;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        if (type == Setting_Button_Type.UUID)
        {
            text.text = $"UUID : {Back_End_Controller.instance.uuid}";
        }
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => Set_Setting());

        text = GetComponentInChildren<TMP_Text>(true);
    }

    #endregion

    #region "Set"

    private void Set_Setting()
    {
        switch (type)
        {
            case Setting_Button_Type.UUID:
                GUIUtility.systemCopyBuffer = Back_End_Controller.instance.uuid;
                Error_Message.instance.Set_Error_Message("Copied_To_Clip_Board");
                break;
            case Setting_Button_Type.Game_Page:
                //Application.OpenURL("");
                break;
            case Setting_Button_Type.Company_Page:
                //Application.OpenURL("");
                break;
            case Setting_Button_Type.Delete_Account:
                Setting_Account.instance.Set_Setting_Acount(false);
                break;
            case Setting_Button_Type.Logout:
                Setting_Account.instance.Set_Setting_Acount(true);
                break;
            case Setting_Button_Type.Change_Password:
                Setting_Account.instance.Set_Change_Password();
                break;
            case Setting_Button_Type.Privacy_Policy:
                Application.OpenURL("https://storage.thebackend.io/b27a95313fe772c1dffb78a151d554284270ff25a4933f29be0a2d9137914ac5/privacy.html");
                break;
            case Setting_Button_Type.Terms:
                Application.OpenURL("https://storage.thebackend.io/b27a95313fe772c1dffb78a151d554284270ff25a4933f29be0a2d9137914ac5/terms.html");
                break;
        }
    }

    #endregion
}

public enum Setting_Button_Type
{
    UUID, Game_Page, Company_Page, Delete_Account, Logout, Change_Password, Privacy_Policy, Terms
}