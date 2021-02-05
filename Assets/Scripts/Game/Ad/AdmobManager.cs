using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdmobManager : MonoBehaviour
{
    // [SerializeField] string App_ID = "ca-app-pub-5176018929650163~4495770289";
    [SerializeField] string rewardedAd_ID = "ca-app-pub-5176018929650163/5597739590";

    [SerializeField] Button rewardedVideoButton;
    [SerializeField] GameObject rewardRecievedPanel;
    private RewardedAd rewardedAd;
    bool isRewarded, isAdClosed;
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestRewardBasedVideo();
        rewardedVideoButton.onClick.AddListener(delegate { ShowRewardedVideoAd(); });        
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isAdClosed)
        {
            if (isRewarded)
            {
                rewardedVideoButton.gameObject.SetActive(false);
                Debug.Log("rewarded");
                // do all the actions
                // reward the player
                isRewarded = false;
            }
            else
            {
                // Ad closed but user skipped ads, so no reward 
                // Ad your action here 
            }
            isAdClosed = false;  // to make sure this action will happen only once.
        }

    }

    public void RequestRewardBasedVideo()
    {
        // Get singleton reward based video ad reference.
        this.rewardedAd = new RewardedAd(rewardedAd_ID);

        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedVideoAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        else
        {
            Debug.Log("Rewarded Ad is not ready and couldnt Show it");
        }
    }
    //Events
    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }
    //EVENTS AD DELEGATES FOR REWARD BASED VIDEO
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardBasedVideoClosed event received");

        RequestRewardBasedVideo(); // to load Next Videoad
        isAdClosed = true;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        print("HandleRewardBasedVideoRewarded event received for 300 leaf ");
        GameDataManager.instance.AddLeaf(100);
        isRewarded = true;
        rewardRecievedPanel.gameObject.SetActive(true);
    }
}
