using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gamble_Got_Stat_Content : MonoBehaviour
{
    public TMP_Text rank_text;
    public GameObject[] grade;
    public Image icon_Image;

    private Localization_Text title_text;

    private Paid_Stat current_stat;
    private ParticleSystem rare_particle;
    private RectTransform rect_transform;

    #region "Initialize"

    public void Initialize_Component()
    {
        title_text = GetComponentInChildren<Localization_Text>(true);
        rect_transform = GetComponent<RectTransform>();
        rare_particle = GetComponentInChildren<ParticleSystem>(true);
    }

    #endregion

    #region "Set"

    public void Set_Get_Stat_Content(Paid_Stat target_stat)
    {
        if (rare_particle == null)
        {
            Initialize_Component();
        }

        current_stat = target_stat;

        icon_Image.sprite = current_stat.stat_icon;
        rank_text.text = current_stat.rank.ToString();
        title_text.Set_Localization_Key(target_stat.name);
        title_text.Localize_Text();

        for (int i = 0; i < 4; i++)
        {
            if (i <= target_stat.grade)
            {
                grade[i].SetActive(true);
            }
            else
            {
                grade[i].SetActive(false);
            }
        }

        if ((int)current_stat.rank >= 4)
        {
            rare_particle.gameObject.SetActive(true);
        }
        else
        {
            rare_particle.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);

        //StartCoroutine(Show_Rotate());
    }

    #endregion

    #region "Animation"

    private IEnumerator Show_Rotate()
    {
        float animation_time = 1.6f;
        float current_animation_time = 0f;

        float animation_speed = 1500.0f;
        float currnent_animation_speed = animation_speed;

        float angle = 0f;

        while (current_animation_time < animation_time)
        {
            float t = current_animation_time / animation_time;
            currnent_animation_speed = Mathf.Lerp(animation_speed, 0, t);
            angle += currnent_animation_speed * Time.deltaTime;
            rect_transform.localEulerAngles = new Vector3(0, angle % 360, 0);
            current_animation_time += Time.deltaTime;
            yield return null;
        }

        float final_time = 0.35f;
        current_animation_time = 0f;
        float finalAngle = rect_transform.localEulerAngles.y;

        while (current_animation_time < final_time)
        {
            float t = current_animation_time / final_time;
            rect_transform.localEulerAngles = new Vector3(0, Mathf.Lerp(finalAngle, 0, t), 0);
            current_animation_time += Time.deltaTime;
            yield return null;
        }

        rect_transform.localEulerAngles = new Vector3(0, 0, 0);
    }


    #endregion
}