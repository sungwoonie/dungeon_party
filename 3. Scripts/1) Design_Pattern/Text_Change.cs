using System;
using UnityEngine;

public static class Text_Change
{
    static readonly string[] CurrencyUnits = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };

    /// <summary>
    /// double 형 데이터를 클리커 게임의 화폐 단위로 표현
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

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string formattedNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed to parse exponent: {0}", partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
        var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

        //  소수 둘째자리까지만 출력한다.
        formattedNumber = temp.ToString("F2");

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, formattedNumber, unityString);
    }

    /// <summary>
    /// 클리커 게임의 화폐 단위 문자열을 double 형 숫자로 변환
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

        // 숫자 부분과 단위 부분을 분리
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

        // 숫자 파싱
        if (!double.TryParse(numberPart, out double number))
        {
            Debug.LogWarningFormat("Failed to parse number part: {0}", numberPart);
            return 0.0;
        }

        // 단위 파싱
        int unitIndex = Array.IndexOf(CurrencyUnits, unitPart);
        if (unitIndex == -1)
        {
            Debug.LogWarningFormat("Failed to find unit part: {0}", unitPart);
            return 0.0;
        }

        // 실제 숫자 계산
        double result = number * Math.Pow(10, unitIndex * 3);

        return result;
    }
}