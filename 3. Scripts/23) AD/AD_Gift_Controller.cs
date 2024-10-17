using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Gift_Controller : MonoBehaviour
{
    public float appear_delay_minimum;
    public float appear_delay_maximum;

    private AD_Gift ad_gift;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    private void Start()
    {
        StartCoroutine(Timer_Coroutine());
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        ad_gift = GetComponentInChildren<AD_Gift>(true);
    }

    #endregion

    #region "Timer"

    private IEnumerator Timer_Coroutine()
    {
        while (true)
        {
            float current_delay = Random.Range(appear_delay_minimum, appear_delay_maximum);

            while (current_delay > 0.0f)
            {
                current_delay -= Time.deltaTime;
                yield return null;
            }

            ad_gift.Set_AD_Gift();

            yield return null;
        }
    }

    #endregion
}
