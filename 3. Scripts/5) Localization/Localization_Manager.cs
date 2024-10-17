using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_Manager : SingleTon<Localization_Manager>
{
    private Dictionary<string, Dictionary<string, object>> localization_data = new Dictionary<string, Dictionary<string, object>>();

    private Localization_Text[] localize_texts;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Localization_Data();
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Localization_Data()
    {
        localization_data = CSVReader.Read("CSV/Localization_CSV");
    }

    private void Initialize_Component()
    {
        localize_texts = FindObjectsOfType<Localization_Text>();
    }

    #endregion

    #region "Localization"

    public void Localize_All()
    {
        foreach (var localize_text in localize_texts)
        {
            localize_text.Localize_Text();
        }
    }

    public string Get_Localized_String(string key)
    {
        if (localization_data.ContainsKey(key))
        {
            return localization_data[key][Local.Get_Current_Local()].ToString();
        }
        else
        {
            Debug.LogWarning($"Key '{key}' not found in localization data.");
            return key;
        }
    }

    #endregion
}