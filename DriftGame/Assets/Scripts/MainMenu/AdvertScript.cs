using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using romanlee17.MirraGames;

public class AdvertScript : MonoBehaviour
{
    [SerializeField] int AdID;
    [SerializeField] private UpperMenuButtons _managerMoney;
    [SerializeField] private Button _rewardButton;
    private const string ADD_MONEY_REWARD_ID = "MENU";
    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;

    private void Awake()
    {
        // сразу подписываем кнопку
        _rewardButton.onClick.AddListener(OnRewardButtonClicked);

        // если SDK уже инициализировано – сразу выполняем
        if (Bootstrap.isInitialized)
        {
            OnSDKDataReceived();
        }
        else
        {
            // ждём событие инициализации
            FindObjectOfType<Bootstrap>().onInitialized += OnSDKDataReceived;
        }
    }

    private void OnDestroy()
    {
        // убираем подписку, если объект уничтожается
        var bootstrap = FindObjectOfType<Bootstrap>();
        if (bootstrap != null)
        {
            bootstrap.onInitialized -= OnSDKDataReceived;
        }
    }

    private void OnSDKDataReceived()
    {
        Debug.Log("AdvertScript: SDK инициализировано");
    }

    private void OnRewardButtonClicked()
    {
        Debug.Log("Reward Button Pressed");
        ShowRewardedAd();
    }

    private void ShowRewardedAd()
    {
        // if (!Bootstrap.isInitialized)
        // {
        //     Debug.LogWarning("SDK ещё не инициализировано, реклама недоступна");
        //     return;
        // }
        //
        // MirraSDK.Ads.InvokeRewarded(
        //     onSuccess: AddRewardMoney,
        //     onNotReady: () => {   return; },
        //     onAnyClose: () => { return; },
        //     rewardTag: ADD_MONEY_REWARD_ID
        // );
        
        AddRewardMoney();
    }

    public void AddRewardMoney()
    {
        Debug.Log("Adding reward money");
        _saveLoadManager.SetMoneyCount();
        _saveLoadManager.LoadMoneyCount();
        _managerMoney.UpdateMoney();
    }
}
