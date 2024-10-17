using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage_Text : MonoBehaviour
{
    private TMP_Text damage_text;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        damage_text = GetComponent<TMP_Text>();
    }

    #endregion

    #region "Set"

    public void Set_Damage_Text(double damage, bool critical)
    {
        damage_text.color = critical ? Color.red : Color.white;
        damage_text.fontStyle = critical ? FontStyles.Bold : FontStyles.Normal;
        damage_text.fontSize = critical ? 50 : 40;

        damage_text.transform.localScale = Vector3.one;

        string damage_amount_text = damage.ToCurrencyString();

        damage_text.text = damage_amount_text;

        damage_text.gameObject.SetActive(true);

        StartCoroutine(Floating_With_Fade_Out());
    }

    private IEnumerator Floating_With_Fade_Out()
    {
        Color fade_color = damage_text.color;

        Vector3 fade_size = Vector3.zero;

        while (fade_color.a > 0)
        {
            fade_color.a -= Time.deltaTime;
            damage_text.color = fade_color;

            damage_text.transform.Translate(Vector3.up * Time.deltaTime * 0.5f);

            damage_text.transform.localScale = Vector3.Lerp(damage_text.transform.localScale, fade_size, Time.deltaTime);

            yield return null;
        }

        damage_text.gameObject.SetActive(false);
    }

    #endregion
}