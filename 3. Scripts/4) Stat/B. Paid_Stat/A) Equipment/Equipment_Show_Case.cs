using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipment_Show_Case : MonoBehaviour
{
    public Image class_image;
    public Localization_Text class_title_text;
    public TMP_Text[] stat_texts;
    
    private Equipment_Stat_Manager stat_manager;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        stat_manager = GetComponentInParent<Equipment_Stat_Manager>(true);
    }

    #endregion

    #region "Set"

    public void Set_Show_Case(int class_type)
    {
        if (stat_manager == null)
        {
            Initialize_Component();
        }

        for (int i = 0; i < stat_texts.Length; i++)
        {
            double target_stat = stat_manager.Get_Equipment_Stat(class_type, i);
            string stat_text = Text_Change.ToCurrencyString(target_stat);

            stat_texts[i].text = stat_text;
        }

        class_title_text.key = $"class_{class_type}_title";
        class_title_text.Localize_Text();

        string class_name = Character_Manager.instance.Get_Current_Class_Name(class_type);
        class_image.sprite = Resources.Load<Sprite>("5. Image/Class/" + class_name + "_Image");
    }

    #endregion
}