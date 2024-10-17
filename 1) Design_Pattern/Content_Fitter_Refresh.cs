using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content_Fitter_Refresh : MonoBehaviour
{
    public bool refresh_on_start;

    private ContentSizeFitter content_size_fitter;

    #region "Unity"

    private void Start()
    {
        Check_On_Start();
    }

    #endregion

    #region "Check Start"

    private void Check_On_Start()
    {
        if (refresh_on_start)
        {
            Refresh_Content_Fitters();
        }
    }

    #endregion

    #region "Refresh"

    public void Refresh_Content_Fitters()
    {
        StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        yield return new WaitForEndOfFrame();

        if (content_size_fitter == null)
        {
            content_size_fitter = GetComponent<ContentSizeFitter>();
        }

        if (content_size_fitter != null)
        {
            content_size_fitter.enabled = false;
            content_size_fitter.enabled = true;
        }
    }

    #endregion
}
