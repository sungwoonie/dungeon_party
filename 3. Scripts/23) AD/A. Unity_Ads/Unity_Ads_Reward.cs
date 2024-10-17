using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Unity_Ads_Reward : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
#if UNITY_ANDROID
    private string ad_unit_id = "Rewarded_Android";
#else
    private string ad_unit_id = "Rewarded_iOS";
#endif

    #region "Load"

    public void Load_Reward_AD()
    {
        Debug.Log("Loading Ad: " + ad_unit_id);
        Advertisement.Load(ad_unit_id, this);
    }

    #endregion

    #region "Call back"

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded: " + ad_unit_id);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Loading Unity ads failed");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Fade.instance.Set_Fade(0.0f);

        Reward_AD_Pop_Up.instance.Give_Reward();

        Load_Reward_AD();

        Debug.Log("OnUnityAdsShowComplete");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Fade.instance.Set_Fade(0.0f);
        Debug.Log("Showing Unity ads failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    #endregion

    #region "Show"

    public void Show_AD()
    {
        Fade.instance.Set_Fade(1.0f);
        Advertisement.Show(ad_unit_id, this);
    }

    #endregion
}
