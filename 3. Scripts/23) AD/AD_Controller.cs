using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AD_Controller : SingleTon<AD_Controller>
{
    public bool test;

    private Unity_Ads_Controller unity_ads;
    private Google_AD_Controller admob;

    private bool show_admob = true;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    private void Start()
    {
        Initialize_AD();
    }

    #endregion

    #region "Initialize"

    private void Initialize_AD()
    {
        unity_ads.Initialize_Ads(test);
        admob.Initialize_AD(test);
    }

    private void Initialize_Component()
    {
        unity_ads = GetComponentInChildren<Unity_Ads_Controller>(true);
        admob = GetComponentInChildren<Google_AD_Controller>(true);
    }

    #endregion

    #region "Show AD"

    public void Show_Reward_AD()
    {
        if (show_admob)
        {
            admob.Show_Reward_AD();
        }
        else
        {
            unity_ads.Show_Reward_AD();
        }

        show_admob = !show_admob;
    }

    #endregion
}
