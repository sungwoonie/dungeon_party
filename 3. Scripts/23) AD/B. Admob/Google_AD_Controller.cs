using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Google_AD_Controller : MonoBehaviour
{
    private Google_Reward reward;

    #region "Unity"

    private  void Awake()
    {
        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    public void Initialize_AD(bool test)
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            reward.Load_Reward_Ad(test);
        });
    }

    private void Initialize_Component()
    {
        reward = GetComponent<Google_Reward>();
    }

    #endregion

    #region "Show AD"

    public void Show_Reward_AD()
    {
        reward.Show_Reward_AD();
    }

    #endregion
}