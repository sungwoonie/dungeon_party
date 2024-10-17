using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Localization_Text : MonoBehaviour
{
    public string key;

    private TMP_Text target_text;

    public bool on_start;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        if (on_start)
        {
            Localize_Text();
        }
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        if (!target_text)
        {
            target_text = GetComponent<TMP_Text>();
        }
    }

    #endregion

    #region "Localization"

    public void Set_Localization_Key(string localization_key)
    {
        key = localization_key;
    }

    public void Localize_Text()
    {
        Initialize_Component();

        if (!string.IsNullOrEmpty(key))
        {
            string localize_text = Localization_Manager.instance.Get_Localized_String(key);
            localize_text = localize_text.Replace("^", "\n");

            target_text.text = localize_text;
        }
    }

    public void Set_Text(string text)
    {
        Initialize_Component();

        text = text.Replace("^", "\n");

        target_text.text = text;
    }

    #endregion
}