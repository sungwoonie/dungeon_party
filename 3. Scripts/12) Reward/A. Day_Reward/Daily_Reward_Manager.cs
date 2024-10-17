using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Daily_Reward_Manager : SingleTon<Daily_Reward_Manager>
{
    private Day_Reward_Manager[] reward_managers;

    private int completed;
    private DateTime last_day;

    private Server_Daily_Data data;

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
        reward_managers = GetComponentsInChildren<Day_Reward_Manager>(true);
    }

    public void Initialize_Data(Server_Daily_Data server_data)
    {
        data = server_data;

        completed = data.c;

        last_day = Date_Time_Parser.Get_Parse_Date_Time(data.ld);
        DateTime current_day = Back_End_Controller.instance.server_time;

        if (last_day.Date != current_day.Date)
        {
            completed++;
            last_day = current_day;

            Battle_Pass_Manager.instance.Get_Requirement("login", 1);
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Count(true);

            Save_Data();
        }
        else
        {
            Reward_AD_Pop_Up.instance.Set_Reward_AD_Count(false);
        }

        if (completed >= 31)
        {
            completed = 1;
            data.dr = 0;
            data.wr = 0;

            Save_Data();
        }

        foreach (var reward_manager in reward_managers)
        {
            reward_manager.Initialize_Data(completed, reward_manager.is_daily ? data.dr : data.wr);
        }
    }

    #endregion

    #region "Save Data"

    public void Save_Data(bool daily, int received)
    {
        data.ld = last_day.Change_To_String();

        if (daily)
        {
            data.dr = received;
        }
        else
        {
            data.wr = received;
        }

        data.c = completed;

        Anti_Cheat_Manager.instance.Set("Daily_Data", JsonUtility.ToJson(data));
    }

    public void Save_Data()
    {
        data.ld = last_day.Change_To_String();
        data.c = completed;

        Anti_Cheat_Manager.instance.Set("Daily_Data", JsonUtility.ToJson(data));
    }

    #endregion
}
