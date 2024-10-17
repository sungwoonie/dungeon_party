using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Local
{
    public static Local_List current_local = Local_List.kr;

    #region "Set Local"

    public static void Set_Local(Local_List local)
    {
        current_local = local;
    }

    #endregion

    #region "Get Local"

    public static string Get_Current_Local()
    {
        return current_local.ToString();
    }

    #endregion
}

public enum Local_List
{
    kr, jp, en
}