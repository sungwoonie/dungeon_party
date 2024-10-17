using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Character_Health_Bar : MonoBehaviour, IPointerClickHandler
{
    public Image health_bar;
    public TMP_Text health_text;

    private bool is_revive_bar;
    public int class_type;

    private readonly Vector3 offset_position = new Vector3(0, 0.6f, 0);

    #region "Set"

    public void Set_Health_Bar_Position(Transform target)
    {
        transform.position = target.position + offset_position;
    }

    public void Set_Health_Bar_Class_Type(int class_type)
    {
        this.class_type = class_type;
    }

    public void Set_Health_bar(double max_health, double current_health, bool revive = false)
    {
        if (is_revive_bar)
        {
            if (!revive)
            {
                return;
            }
        }

        is_revive_bar = false;

        float health_ratio = (float)(current_health / max_health);

        health_text.text = Text_Change.ToCurrencyString(current_health);

        StopAllCoroutines();
        StartCoroutine(Slider_Lerp((float)health_ratio));
    }

    private IEnumerator Slider_Lerp(float lerp_amount)
    {
        float current_time = 0.0f;

        while (health_bar.fillAmount != lerp_amount)
        {
            current_time += Time.deltaTime;
            health_bar.fillAmount = Mathf.Lerp(health_bar.fillAmount, lerp_amount, current_time / 2 * 0.8f);

            yield return null;
        }
    }

    #endregion

    #region "Revive"

    public void Set_To_Revive_Bar()
    {
        is_revive_bar = true;

        health_bar.fillAmount = 1.0f;
        health_text.text = "10";

        StartCoroutine(Revive_Bar());
    }

    private IEnumerator Revive_Bar()
    {
        float revive_time = 5.0f;
        float current_revive_time = revive_time;

        while (is_revive_bar && current_revive_time > 0)
        {
            current_revive_time -= Time.deltaTime;

            health_bar.fillAmount = current_revive_time / revive_time;
            health_text.text = $"{Mathf.FloorToInt(current_revive_time)}";

            if (current_revive_time <= 0)
            {
                is_revive_bar = false;
            }

            yield return null;
        }

        health_bar.fillAmount = 0;
        health_text.text = "0";

        if (Event_Bus.Get_Current_State() != Game_State.All_Dead)
        {
            Character_Manager.instance.Revive(class_type);

            Debug_Manager.Debug_In_Game_Message($"{class_type}'s revive timer is over. revive {class_type}");
        }
    }

    #endregion

    #region "On Click"

    public void OnPointerClick(PointerEventData eventData)
    {
        if (is_revive_bar)
        {
            //make ad popup
        }
    }

    #endregion
}