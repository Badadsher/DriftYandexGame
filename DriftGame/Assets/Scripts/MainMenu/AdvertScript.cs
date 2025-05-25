using System;
using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class AdvertScript : MonoBehaviour
{
    [SerializeField] int AdID;
    [SerializeField] private UpperMenuButtons _managerMoney;
    [SerializeField] private Button _rewardButton;
    private const string ADD_MONEY_REWARD_ID = "MENU";
    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;
  
    [Inject]


    private void Start()
    {
        if (Bootstrap.isInitialized)
        {
            OnSDKDataReceived();
        }
    }

    private void OnSDKDataReceived()
    {
        Debug.Log("Initializing Advert Script" + _saveLoadManager);
        _rewardButton.onClick.AddListener(() =>
        {
            ShowRewardedAd();
        });
    }

    private void OnEnable()
    {

    }

    private void ShowRewardedAd()
    {
        MirraSDK.Ads.InvokeRewarded(
            onSuccess: () =>
            {
                AddRewardMoney();
            },
            onNotReady: () => {   Debug.Log("Rewarded Ad notr"); },
            onAnyClose: () => { return; },
            rewardTag: "MENU"
        );
    }
    
    public void AddRewardMoney()
    {
        Debug.Log("Adding reward money to advert");
            Debug.Log("Adding reward money");
            _saveLoadManager.SetMoneyCount();
            _saveLoadManager.LoadMoneyCount();
            _managerMoney.UpdateMoney();
    }

    private void OnDisable()
    {
    }
}
