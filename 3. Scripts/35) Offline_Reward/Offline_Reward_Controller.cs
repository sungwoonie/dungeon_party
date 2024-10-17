using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Offline_Reward_Controller : SingleTon<Offline_Reward_Controller>
{
    [Tooltip("0 = gold, 1 = diamond, 2 = equipment, 3 = key, 4 = experience_point")]
    public int[] reward_per_seconds;
    public Offline_Reward_Pop_Up pop_up;

    private DateTime current_time;
    private DateTime latest_time;

    private Reward_Struct current_reward_struct;

    #region "Initialize"

    public void Initialize_Data(string date_time)
    {
        latest_time = Date_Time_Parser.Get_Parse_Date_Time(date_time);
        current_time = Back_End_Controller.instance.server_time;

        Set_Offline_Reward();
    }

    #endregion

    #region "Set Reward"

    private void Set_Offline_Reward()
    {
        int offline_time = (int)Get_Offline_Seconds();

        if (offline_time < 60.0f)
        {
            Debug_Manager.Debug_Server_Message($"Can't get offline reward. offline time is {offline_time}");
            return;
        }

        int[] reward_counts = Get_Reward_Counts(offline_time);
        int[] high_stage = Stage_Manager.instance.Get_High_Stage();

        current_reward_struct = Stage_Reward_Manager.instance.Get_Reward_Struct_For_Offline_Reward(high_stage, reward_counts);
        pop_up.Set_Reward_Pop_Up(current_reward_struct);
    }

    private int[] Get_Reward_Counts(int offline_time)
    {
        int[] reward_counts = new int[reward_per_seconds.Length];

        for (int i = 0; i < reward_per_seconds.Length; i++)
        {
            reward_counts[i] = offline_time / reward_per_seconds[i];
        }

        if (reward_counts[2] > 10)
        {
            reward_counts[2] = 10;
        }

        return reward_counts;
    }

    private double Get_Offline_Seconds()
    {
        TimeSpan offline_time = current_time - latest_time;

        return offline_time.TotalSeconds;
    }

    #endregion

    #region "Get Reward"

    public void Get_Reward(bool ad)
    {
        if (ad)
        {
            current_reward_struct.reward_budget.gold *= 2.0f;
            current_reward_struct.reward_budget.diamond *= 2.0f;
            current_reward_struct.reward_budget.key *= 2.0f;
            current_reward_struct.experience_point *= 2.0f;
        }

        current_reward_struct.Get_Reward();

        StartCoroutine(Offline_Timer());
    }

    #endregion

    #region "Timer"

    private IEnumerator Offline_Timer()
    {
        while (true)
        {
            Save_Data();
            yield return new WaitForSeconds(60.0f);
            current_time.AddMinutes(1.0f);
        }
    }

    #endregion

    #region "Save Data"

    private void Save_Data()
    {
        User_Data.instance.Save_Data_Offline_Time(current_time.Change_To_String());
    }

    #endregion
}
