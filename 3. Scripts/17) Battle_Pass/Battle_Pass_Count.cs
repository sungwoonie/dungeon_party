using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Battle_Pass_Count : MonoBehaviour
{
    public GameObject[] complete_objects;
    public GameObject[] default_objects;

    public TMP_Text amount_text;

    public void Set_Object(bool complete)
    {
        foreach (var complete_object in complete_objects)
        {
            complete_object.SetActive(complete);
        }

        foreach (var default_object in default_objects)
        {
            default_object.SetActive(!complete);
        }
    }

    public void Set_Amount(int amount)
    {
        amount_text.text = amount.ToString();
    }
}