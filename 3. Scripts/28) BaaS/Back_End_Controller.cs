using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Util;
using LitJson;
using TMPro;
using System;

public class Back_End_Controller : SingleTon<Back_End_Controller>
{
    public bool test;
    [HideInInspector]public string uuid;
    [HideInInspector]public DateTime server_time;

    private Auto_Save auto_save;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();

        Initialize_Back_End();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        auto_save = GetComponent<Auto_Save>();

        Bad_Word_Censor.Initialize();
    }

    private void Initialize_Back_End()
    {
        var bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro);
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro);
        }
    }

    #endregion

    #region "Initialize User Data"

    public IEnumerator Initialize_User_Data()
    {
        Debug_Manager.Debug_Server_Message("Load Data Start");

        Loading_Bar.instance.Set_Loading_Bar();

        if (Correct_Version() == false)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.starcloudgames.dungeonparty&hl=ko");
            yield break;
        }

        if (!Check_UUID())
        {
            Another_Data_Exist.instance.Set_Another_Data_Exist();
            Loading_Bar.instance.Set_Off();
            yield break;
        }

        Set_Server_Time();
        Loading_Bar.instance.Set_Loading_Bar();

        Database_Controller.instance.Initialize_Database();
        Loading_Bar.instance.Set_Loading_Bar();

        In_App_Purchase_Manager.instance.Initialize_In_App_Purchasing();
        yield return new WaitForSeconds(0.3f);

        Debug_Manager.Debug_Server_Message("Load Data Finished");
        auto_save.Start_Auto_Save();

        Loading_Bar.instance.Set_Loading_Bar();

        StartCoroutine(Scene_Manager.Load_Scene("In_Game"));
    }

#endregion

    #region "UUID"

    private bool Check_UUID()
    {
        BackendReturnObject bro = Backend.BMember.GetUserInfo();
        uuid = bro.GetReturnValuetoJSON()["row"]["gamerId"].ToString();

        string saved_uuid = Anti_Cheat_Manager.instance.Get("UUID", uuid);

        if (saved_uuid != uuid)
        {
            return false;
        }
        else
        {
            Anti_Cheat_Manager.instance.Set("UUID", uuid);
            return true;
        }
    }

#endregion

    #region "Server Time"

    private void Set_Server_Time()
    {
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        server_time = Date_Time_Parser.Get_Parse_Date_Time(time);
    }

#endregion

    #region "Version Check"

    public bool Correct_Version()
    {
#if UNITY_EDITOR
        return true;
#else

        var bro = Backend.Utils.GetLatestVersion();

        string[] server_version = bro.GetReturnValuetoJSON()["version"].ToString().Split(".");
        string[] application_version = Application.version.Split(".");

        int[] compare_server_version = new int[] { int.Parse(server_version[0]), int.Parse(server_version[1]), int.Parse(server_version[2])};
        int[] compare_application_version = new int[] { int.Parse(application_version[0]), int.Parse(application_version[1]), int.Parse(application_version[2]) };

        for (int i = 0; i < compare_server_version.Length; i++)
        {
            if (compare_application_version[i] > compare_server_version[i])
            {
                return true;
            }
        }

        for (int i = 0; i < compare_server_version.Length; i++)
        {
            if (compare_application_version[i] != compare_server_version[i])
            {
                return false;
            }
        }

        return true;

#endif
    }

#endregion
}