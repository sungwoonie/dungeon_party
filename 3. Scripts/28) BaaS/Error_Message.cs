using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error_Message : SingleTon<Error_Message>
{
    private Localization_Text message_text;
    private GameObject pop_up;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initiailize"

    private void Initialize_Component()
    {
        message_text = GetComponentInChildren<Localization_Text>(true);
        pop_up = transform.GetChild(0).gameObject;
    }

    #endregion

    #region "Set"

    public void Set_Error_Message(string localization_key)
    {
        Debug_Manager.Debug_Server_Message(localization_key);

        message_text.Set_Localization_Key(localization_key);
        message_text.Localize_Text();

        pop_up.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float timer = 2.0f;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        pop_up.SetActive(false);
    }

    #endregion
}
