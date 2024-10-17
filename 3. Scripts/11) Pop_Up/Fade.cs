using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : SingleTon<Fade>
{
    private Image fade;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        fade = GetComponent<Image>();
    }

    #endregion

    #region "Fade"

    public IEnumerator Fade_In()
    {
        float fade_a = 0.0f;
        Color fade_color = Color.clear;

        while (fade_a < 1)
        {
            fade_a += Time.deltaTime;
            fade_color.a = fade_a;

            fade.color = fade_color;

            yield return null;
        }
    }

    public IEnumerator Fade_Out()
    {
        float fade_a = 1.0f;
        Color fade_color = Color.white;

        while (fade_a > 0)
        {
            fade_a -= Time.deltaTime;
            fade_color.a = fade_a;

            fade.color = fade_color;

            yield return null;
        }
    }

    public void Set_Fade(float a)
    {
        Color fade_color = new Color(1, 1, 1, a);
        fade.color = fade_color;
    }

    #endregion
}