using System;
using UnityEngine;

public static class Text_Change
{
    static readonly string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    /// <summary>
    /// double �� �����͸� Ŭ��Ŀ ������ ȭ�� ������ ǥ��
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static string ToCurrencyString(this double number)
    {
        string zero = "0";

        if (number < 1.0f)
        {
            return string.Format("{0}{1}{2}", "", number.ToString("F2"), "");
        }

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  ��ȣ ��� ���ڿ�
        string significant = (number < 0) ? "-" : string.Empty;

        //  ������ ����
        string formattedNumber = string.Empty;

        //  ���� ���ڿ�
        string unityString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = number.ToString("E").Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed to parse exponent: {0}", partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 3;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 3;

        //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
        var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

        //  �Ҽ� ��°�ڸ������� ����Ѵ�.
        formattedNumber = temp.ToString("F2");

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, formattedNumber, unityString);
    }

    /// <summary>
    /// Ŭ��Ŀ ������ ȭ�� ���� ���ڿ��� double �� ���ڷ� ��ȯ
    /// </summary>
    /// <param name="currencyString"></param>
    /// <returns></returns>
    public static double FromCurrencyString(this string currencyString)
    {
        if (string.IsNullOrEmpty(currencyString))
        {
            return 0.0;
        }

        string numberPart = string.Empty;
        string unitPart = string.Empty;

        // ���� �κа� ���� �κ��� �и�
        for (int i = 0; i < currencyString.Length; i++)
        {
            if (char.IsDigit(currencyString[i]) || currencyString[i] == '.' || currencyString[i] == '-')
            {
                numberPart += currencyString[i];
            }
            else
            {
                unitPart = currencyString.Substring(i);
                break;
            }
        }

        // ���� �Ľ�
        if (!double.TryParse(numberPart, out double number))
        {
            Debug.LogWarningFormat("Failed to parse number part: {0}", numberPart);
            return 0.0;
        }

        // ���� �Ľ�
        int unitIndex = Array.IndexOf(CurrencyUnits, unitPart);
        if (unitIndex == -1)
        {
            Debug.LogWarningFormat("Failed to find unit part: {0}", unitPart);
            return 0.0;
        }

        // ���� ���� ���
        double result = number * Math.Pow(10, unitIndex * 3);

        return result;
    }
}