using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User_Data : SingleTon<User_Data>
{
    private Server_User_Data user_data;

    #region "Initialize"

    public void Initialize_Data(Server_User_Data server_user_data)
    {
        user_data = server_user_data;

        if (user_data.ab)
        {
            Skill_Buff_Manager.instance.Use_All_AD_Buff();
        }
    }

    #endregion

    #region "Get"

    public bool AD_Remove()
    {
        return user_data.ar;
    }

    public bool Auto_Buff()
    {
        return user_data.ab;
    }

    #endregion

    #region "Save"

    public void Save_Data_Auto_Buff(bool auto_buff)
    {
        user_data.ab = auto_buff;
        Anti_Cheat_Manager.instance.Set("User_Data", JsonUtility.ToJson(user_data));
    }

    public void Save_Data_Ad_Remove(bool remove)
    {
        user_data.ar = remove;
        Anti_Cheat_Manager.instance.Set("User_Data", JsonUtility.ToJson(user_data));
    }

    public void Save_Data_UUID(string uuid)
    {
        user_data.uuid = uuid;
        Anti_Cheat_Manager.instance.Set("User_Data", JsonUtility.ToJson(user_data));
    }

    public void Save_Data_Offline_Time(string offline_time)
    {
        user_data.ot = offline_time;
        Anti_Cheat_Manager.instance.Set("User_Data", JsonUtility.ToJson(user_data));
    }

    #endregion
}
