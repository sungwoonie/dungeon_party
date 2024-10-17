using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class Debug_Manager
{
    [Conditional("USE_DEBUG")]
    public static void Debug_In_Game_Message(string debug_masage)
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            UnityEngine.Debug.Log(debug_masage);
        }
    }

    [Conditional("USE_DEBUG_SERVER")]
    public static void Debug_Server_Message(string debug_masage)
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
        {
            UnityEngine.Debug.Log(debug_masage);
        }
    }
}