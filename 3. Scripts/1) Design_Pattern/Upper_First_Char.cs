using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Upper_First_Char
{
    public static string Upper_First_Char_By_Underline(this string input_string)
    {
        string value = string.Empty;

        string[] split_by_underline = input_string.Split("_");

        for (int i = 0; i < split_by_underline.Length; i++)
        {
            split_by_underline[i] = char.ToUpper(split_by_underline[i][0]) + split_by_underline[i].Substring(1);

            if (i + 1 == split_by_underline.Length)
            {
                value += split_by_underline[i];
            }
            else
            {
                value += split_by_underline[i] + "_";
            }
        }

        return value;
    }

    public static string Upper_First(this string input_string)
    {
        return char.ToUpper(input_string[0]) + input_string.Substring(1);
    }
}