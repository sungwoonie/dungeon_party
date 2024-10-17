using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Loading_Bar : SingleTon<Loading_Bar>
{
    private Slider loading_bar;
    private Localization_Text loading_text;

    private float loading_limit = 5;
    private float current_loading = 0;

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
        loading_bar = GetComponentInChildren<Slider>(true);
        loading_text = GetComponentInChildren<Localization_Text>(true);
    }

    #endregion

    #region "Set Slider"

    public void Set_Off()
    {
        current_loading = 0;
        StopAllCoroutines();
        loading_bar.gameObject.SetActive(false);
    }

    public void Set_Loading_Bar()
    {
        if (!loading_bar.gameObject.activeSelf)
        {
            loading_bar.gameObject.SetActive(true);
        }

        current_loading++;

        if (current_loading > loading_limit)
        {
            return;
        }

        loading_text.Set_Localization_Key($"Loading_Text_{current_loading}");
        loading_text.Localize_Text();

        float slider_amount = current_loading / loading_limit;

        StopAllCoroutines();
        StartCoroutine(Set_Loading_Bar(slider_amount));
    }

    private IEnumerator Set_Loading_Bar(float amount)
    {
        float timer = 0.0f;
        while (!Mathf.Approximately(loading_bar.value, amount))
        {
            timer += Time.deltaTime;
            loading_bar.value = Mathf.Lerp(loading_bar.value, amount, 2 / timer);

            yield return null;
        }
    }

    #endregion
}
