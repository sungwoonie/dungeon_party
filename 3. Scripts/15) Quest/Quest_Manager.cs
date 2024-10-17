using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Quest_Manager : SingleTon<Quest_Manager>
{
    public Quest_Content repeat_quest_content;

    public Quest_Content[] daily_quest_contents;
    public Quest_Content[] monthly_quest_contents;

    public TMP_Text[] timer_texts;

    private Quest_Data quest_data;

    private DateTime[] latest_time; //타이머 만들어야됨
    private DateTime current_time;
    private TimeSpan[] timer_offsets = new TimeSpan[2]{ new TimeSpan(24, 0, 0), new TimeSpan(31, 0, 0, 0) };
    private TimeSpan[] remaining_time = new TimeSpan[2]{ new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 0, 0) };

    private int[] daily_requirement; 
    private int[] monthly_requirement;
    private int[] daily_received;
    private int[] monthly_received;

    private Server_Quest_Data data;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    public void Initialize_Data(Server_Quest_Data server_data)
    {
        data = server_data;

        daily_requirement = data.dr;
        daily_received = data.drd;
        monthly_requirement = data.mr;
        monthly_received = data.mrd;

        latest_time = new DateTime[data.lqt.Length];
        latest_time[0] = Date_Time_Parser.Get_Parse_Date_Time(data.lqt[0]);
        latest_time[1] = Date_Time_Parser.Get_Parse_Date_Time(data.lqt[1]);

        current_time = Back_End_Controller.instance.server_time;

        Set_Remaining_Time();

        Set_Quest(true);
        Set_Quest(false);
        Set_Repeat_Quest();

        StartCoroutine(Quest_Timer());
    }

    private void Initialize_Component()
    {
        quest_data = GetComponent<Quest_Data>();
    }

    #endregion

    #region "Increase"

    public void Increase_Requirement(string quest_name, int requirement)
    {
        if (repeat_quest_content.current_quest.quest_name.Equals(quest_name))
        {
            repeat_quest_content.Increase_Current_Requirement(requirement);
        }

        for (int i = 0; i < daily_quest_contents.Length; i++)
        {
            if (daily_quest_contents[i].current_quest.quest_name.Equals(quest_name))
            {
                daily_requirement[i] = daily_quest_contents[i].Increase_Current_Requirement(requirement);
            }
        }

        for (int i = 0; i < monthly_quest_contents.Length; i++)
        {
            if (monthly_quest_contents[i].current_quest.quest_name.Equals(quest_name))
            {
                monthly_requirement[i] = monthly_quest_contents[i].Increase_Current_Requirement(requirement);
            }
        }

        Save_Data();
    }

    #endregion

    #region "Reward"

    public void Get_Reward(Quest_Content content, bool repeat, bool daily)
    {
        if (!repeat)
        {
            if (daily)
            {
                for (int i = 0; i < daily_quest_contents.Length; i++)
                {
                    if (daily_quest_contents[i] == content)
                    {
                        daily_received[i] = 1;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < monthly_quest_contents.Length; i++)
                {
                    if (monthly_quest_contents[i] == content)
                    {
                        monthly_received[i] = 1;
                        break;
                    }
                }
            }

            Save_Data();
        }
    }

    #endregion

    #region "Quest_Complete"

    public void Repeat_Quest_Complete()
    {
        Set_Repeat_Quest();
    }

    public void Quest_Complete(bool daily, bool repeat)
    {
        if (daily && !repeat)
        {
            Increase_Requirement("clear_daily_quest", 1);
        }
        //check all clear
    }

    #endregion

    #region "Set"

    private void Reset_Quest(bool daily)
    {
        if (daily)
        {
            latest_time[0] = current_time;
            daily_requirement = new int[daily_requirement.Length];
            daily_received = new int[daily_received.Length];
        }
        else
        {
            latest_time[1] = current_time;
            monthly_requirement = new int[monthly_requirement.Length];
            monthly_received = new int[monthly_requirement.Length];
        }

        Save_Data();
    }

    public void Set_Quest(bool daily)
    {
        Quest_Struct[] quest_structs = daily ? quest_data.Get_Daily_Quest() : quest_data.Get_Monthly_Quest();
        Quest_Content[] quest_contents = daily ? daily_quest_contents : monthly_quest_contents;
        int[] requirement = daily ? daily_requirement : monthly_requirement;
        int[] received = daily ? daily_received : monthly_received;

        for (int i = 0; i < quest_contents.Length; i++)
        {
            quest_contents[i].Set_New_Quest(quest_structs[i], Convert.ToBoolean(received[i]), requirement[i]);
        }
    }

    public void Set_Repeat_Quest()
    {
        Quest_Struct quest = quest_data.Get_Repeat_Quest();
        repeat_quest_content.Set_New_Quest(quest, false, 0);
    }

    #endregion

    #region "Save Data"

    public void Save_Data()
    {
        data.dr = daily_requirement;
        data.drd = daily_received;
        data.mr = monthly_requirement;
        data.mrd = monthly_received;

        data.lqt = new string[2] { latest_time[0].Change_To_String(), latest_time[1].Change_To_String() };

        Anti_Cheat_Manager.instance.Set("Quest_Data", JsonUtility.ToJson(data));
    }

    #endregion

    #region "Timer"

    private IEnumerator Quest_Timer()
    {
        string[] localized_times = new string[3];
        localized_times[0] = Localization_Manager.instance.Get_Localized_String("Day");
        localized_times[1] = Localization_Manager.instance.Get_Localized_String("Hour");
        localized_times[2] = Localization_Manager.instance.Get_Localized_String("Minute");

        while (true)
        {
            timer_texts[0].text = $"{(int)remaining_time[0].TotalHours}{localized_times[1]} {remaining_time[0].Minutes}{localized_times[2]}";
            timer_texts[1].text = $"{remaining_time[1].Days}{localized_times[0]} {remaining_time[1].Hours}{localized_times[1]} {remaining_time[1].Minutes}{localized_times[2]}";

            yield return new WaitForSeconds(60.0f);

            current_time = current_time.AddMinutes(1);
            Set_Remaining_Time();
        }
    }

    private void Set_Remaining_Time()
    {
        remaining_time[0] = timer_offsets[0] - (current_time - latest_time[0]);
        remaining_time[1] = timer_offsets[1] - (current_time - latest_time[1]);

        if (remaining_time[0].Ticks <= 0)
        {
            Reset_Quest(true);
            remaining_time[0] = timer_offsets[0];
        }

        if (remaining_time[1].Ticks <= 0)
        {
            Reset_Quest(false);
            remaining_time[1] = timer_offsets[1];
        }
    }

    #endregion
}