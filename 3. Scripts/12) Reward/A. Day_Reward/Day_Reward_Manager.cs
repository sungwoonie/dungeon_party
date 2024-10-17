using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Day_Reward_Manager : MonoBehaviour
{
    public bool is_daily;

    public TMP_Text title_text;
    public GameObject pop_up;

    private Notify notify;

    private Day_Reward_Content[] contents;

    private Dictionary<string, Dictionary<string, object>> csv_datas = new Dictionary<string, Dictionary<string, object>>();

    private DateTime last_day;

    private int completed;
    private int received;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
        Initialize_CSV();
    }

    #endregion

    #region "Initialize"

    public void Initialize_Data(int _completed, int _received)
    {
        received = _received;
        completed = _completed;

        Set_Text();
        Set_Reward();
    }

    private void Initialize_CSV()
    {
        string csv = is_daily ? "Daily_Reward_CSV" : "Weekly_Reward_CSV";
        csv_datas = CSVReader.Read($"CSV/{csv}");
    }

    private void Initialize_Component()
    {
        contents = GetComponentsInChildren<Day_Reward_Content>(true);
        notify = GetComponent<Notify>();
    }

    #endregion

    #region "Show"

    public void Show_Pop_Up()
    {
        pop_up.gameObject.SetActive(true);
    }

    #endregion

    #region "Set"

    public void Set_Notify(bool is_on)
    {
        if (is_on)
        {
            notify.Set_On_Notify();
        }
        else
        {
            notify.Set_Off_Notify();
        }
    }

    private void Set_Text()
    {
        string daily = is_daily ? "daily" : "weekly";
        string title = $"{Localization_Manager.instance.Get_Localized_String($"{daily}_reward_title")}   <color=#f6e19c>{completed}</color> / {csv_datas.Count}";
        title_text.text = title;
    }

    private void Set_Reward()
    {
        for (int i = 0; i < csv_datas.Count; i++)
        {
            int reward_number = int.Parse(csv_datas[(i + 1).ToString()]["reward_number"].ToString());
            string reward_name = csv_datas[(i + 1).ToString()]["reward_name"].ToString();
            double reward_count = double.Parse(csv_datas[(i + 1).ToString()]["reward_count"].ToString());

            Day_Reward_Struct new_reward = new Day_Reward_Struct(reward_number, reward_name, reward_count);

            contents[i].Set_Reward(new_reward);
            contents[i].Set_Status(completed, received);
        }
    }

    #endregion

    #region "Get Reward"

    public void Get_Reward()
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].gameObject.activeSelf)
            {
                if (contents[i].completed == true && contents[i].received == false)
                {
                    contents[i].Get_Reward();
                    received++;
                    Set_Notify(false);
                }
            }
        }

        Daily_Reward_Manager.instance.Save_Data(is_daily, received);
    }

    #endregion
}