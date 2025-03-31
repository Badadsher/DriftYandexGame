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
    [SerializeField] private UpperMenuButtons _managerMoney;
    [SerializeField] private Button _rewardButton;
    private const string ADD_MONEY_REWARD_ID = "menuAddMoney";
    private SaveLoadManager _saveLoadManager;
  
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
        Initialize();
    }
    
    private void Initialize()
    {
        _saveLoadManager.RewardAdvAddListener(AddRewardMoney);
        _rewardButton.onClick.AddListener(() =>
        {
            MirraSDK.Ads.InvokeRewarded(
                onSuccess: () =>
                {
                    Debug.Log("начислено!!");
                    _saveLoadManager.SetMoneyCount();
                    _saveLoadManager.LoadMoneyCount();
                    _managerMoney.UpdateMoney();
                },
                onNotReady: () => {  Debug.Log("notready!!"); },
                onAnyClose: () => {  Debug.Log("anyclose!!"); },
                rewardTag: "menuAddMoney"
            );
        });
    }
    
    public void AddRewardMoney(string id)
    {
        if (id != ADD_MONEY_REWARD_ID)
            return;
           
        _saveLoadManager.SetMoneyCount();
    }
}
