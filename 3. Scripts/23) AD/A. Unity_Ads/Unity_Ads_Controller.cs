using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Unity_Ads_Controller : MonoBehaviour, IUnityAdsInitializationListener
{
#if UNITY_ANDROID
    private string game_id = "5657763";
#else
    private string game_id = "5657762";
#endif

    private Unity_Ads_Reward reward_ad;

    #region "Unity"

    private void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        reward_ad = GetComponentInChildren<Unity_Ads_Reward>(true);
    }

    public void Initialize_Ads(bool test)
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(game_id, test, this);
        }
    }

    #endregion

    #region "Call Back"

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialize Complete");

        reward_ad.Load_Reward_AD();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Unity Ads Initialize failed" + error.ToString() + message);
    }

    #endregion

    #region "Show AD"

    public void Show_Reward_AD()
    {
        reward_ad.Show_AD();
    }

    #endregion
}
