using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Reward_Label : MonoBehaviour
{
    private TMP_Text label_text;
    private Image label;
    private Color offset_color;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        label = GetComponent<Image>();
        label_text = GetComponentInChildren<TMP_Text>();

        offset_color = label.color;
    }

    #endregion

    #region "Set"

    public void Set_Label(string text)
    {
        if (label_text == null)
        {
            Initialize_Component();
        }

        label_text.text = text;
        label.color = offset_color;

        label_text.color = Color.white;

        gameObject.SetActive(true);
        StartCoroutine(Fade());
    }

    #endregion

    #region "Fade"

    private IEnumerator Fade()
    {
        float fade_a = label.color.a;
        Color fade_color = label.color;

        float text_fade = 1.0f;
        Color text_color = label_text.color;

        while (fade_a > 0)
        {
            fade_a -= Time.deltaTime * 0.5f;
            fade_color.a = fade_a;

            text_fade -= Time.deltaTime;
            text_color.a = text_fade;

            label_text.color = text_color;

            label.color = fade_color;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    #endregion
}