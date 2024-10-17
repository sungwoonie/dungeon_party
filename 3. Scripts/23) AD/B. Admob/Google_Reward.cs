using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Google_Reward : MonoBehaviour
{
    private string test_ad_unit_id = "ca-app-pub-3940256099942544/5224354917";
    private string ad_unit_id = "ca-app-pub-5287933802997442/9860103483";

    private RewardedAd current_reward_ad;

    private bool test_mode;

    #region "Load"

    public void Load_Reward_Ad(bool test)
    {
        test_mode = test;

        // Clean up the old ad before loading a new one.
        if (current_reward_ad != null)
        {
            current_reward_ad.Destroy();
            current_reward_ad = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest.Builder().Build();

        // send the request to load the ad.
        RewardedAd.Load(test_mode ? test_ad_unit_id : ad_unit_id, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);

                    //

                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                current_reward_ad = ad;
                Register_Event_Handler(current_reward_ad);
            });
    }

    #endregion

    #region "Show"

    public void Show_Reward_AD()
    {
        if (current_reward_ad != null && current_reward_ad.CanShowAd())
        {
            Fade.instance.Set_Fade(1.0f);

            current_reward_ad.Show((Reward reward) =>
            {
            });
        }
        else
        {
            Debug_Manager.Debug_Server_Message($"Can't load reward ad. not ready");
        }
    }

    #endregion

    #region "Event Handler"

    private void Register_Event_Handler(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            // Reload the ad so that we can show another as soon as possible.
            Fade.instance.Set_Fade(0.0f);

            Load_Reward_Ad(test_mode);
            Reward_AD_Pop_Up.instance.Give_Reward();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            Fade.instance.Set_Fade(0.0f);

            // Reload the ad so that we can show another as soon as possible.
            Load_Reward_Ad(test_mode);
        };
    }

    #endregion
}
