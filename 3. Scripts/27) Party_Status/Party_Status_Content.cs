using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Party_Status_Content : MonoBehaviour
{
    public TMP_Text ratio_text;
    public int stat_type;
    public int class_type;

    #region "Unity"

    private void OnEnable()
    {
        Set_Ratio_Text();
    }

    #endregion

    #region "Set"

    public void Set_Ratio_Text()
    {
        if (Stat_Manager.instance != null)
        {
            if (stat_type.Equals(100))
            {
                double current_stat = Stat_Manager.instance.Combat_Power();
                ratio_text.text = current_stat.ToCurrencyString();
            }
            else
            {
                if (class_type != 4)
                {
                    double current_stat = Stat_Manager.instance.Calculate_Stat(class_type, stat_type);
                    ratio_text.text = current_stat.ToCurrencyString();
                }
                else
                {
                    double current_stat = Stat_Manager.instance.Calculate_Stat(stat_type);
                    ratio_text.text = current_stat.ToCurrencyString();
                }
            }
        }
    }

    #endregion
}
