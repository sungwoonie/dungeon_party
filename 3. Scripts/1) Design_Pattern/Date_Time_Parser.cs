using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public static class Date_Time_Parser
{
    public static DateTime Get_Parse_Date_Time(string date_time)
    {
        return DateTime.Parse(date_time);
    }

    public static string Change_To_String(this DateTime date_time)
    {
        // DateTime ��ü�� 24�ð��� ������ ���ڿ��� ��ȯ
        string formattedDt = date_time.ToString("yyyy-MM-dd HH:mm");
        return formattedDt;
    }
}
