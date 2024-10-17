using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Reward_AD_Button : MonoBehaviour
{
    private TMP_Text timer_text;
    private Button button;

    private float ad_duration = 15.0f;
    private float current_ad_duration;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        button = GetComponent<Button>();
        timer_text = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(() => On_Click_Button());
    }

    #endregion

    #region "Button"

    private void On_Click_Button()
    {
        button.interactable = false;
        timer_text.text = Localization_Manager.instance.Get_Localized_String("watch_ad");

        Reward_AD_Pop_Up.instance.Show_Reward_AD();
    }

    public IEnumerator Start_AD_Button_Count()
    {
        current_ad_duration = ad_duration;

        while (current_ad_duration > 0)
        {
            Set_Timer_Text();

            yield return new WaitForSeconds(1.0f);

            current_ad_duration--;
        }

        current_ad_duration = 0.0f;
        button.interactable = true;
        timer_text.text = Localization_Manager.instance.Get_Localized_String("watch_ad");
    }

    #endregion

    #region "Set"

    private void Set_Timer_Text()
    {
        int remaining_minute = Mathf.FloorToInt(current_ad_duration / 60);
        int remaining_second = Mathf.FloorToInt(current_ad_duration % 60);

        timer_text.text = remaining_minute.ToString() + " : " + remaining_second.ToString("D2");
    }

    #endregion
}
