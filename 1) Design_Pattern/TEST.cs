using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public string a;
    public string[] b;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(Tss("10000"));
            Debug.Log(double.Parse(2.3111E+89.ToString()).ToCurrencyString());
        }
    }

    private double Tss(string a)
    {
        return a.FromCurrencyString();
    }

    private bool Correct_Version()
    {
        string[] server_version = a.Split(".");
        string[] application_version = Application.version.Split(".");

        int[] compare_server_version = new int[] { int.Parse(server_version[0]), int.Parse(server_version[1]), int.Parse(server_version[2]) };
        int[] compare_application_version = new int[] { int.Parse(application_version[0]), int.Parse(application_version[1]), int.Parse(application_version[2]) };

        for (int i = 0; i < compare_server_version.Length; i++)
        {
            if (compare_application_version[i] > compare_server_version[i])
            {
                return true;
            }
        }

        for (int i = 0; i < compare_server_version.Length; i++)
        {
            if (compare_application_version[i] != compare_server_version[i])
            {
                return false;
            }
        }

        return true;
    }
}
