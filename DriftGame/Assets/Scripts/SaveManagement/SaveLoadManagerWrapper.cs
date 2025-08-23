using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManagerWrapper : MonoBehaviour
{
    private MirraSaveLoadManager _saveLoadManager;

    // Настройки для инспектора
    [Header("Advertisement Settings")]
    [SerializeField] private string _rewardAdId = "reward_ad";
    public Action<LeaderboardData> OnGetLeaderboadData;

    [Header("Leaderboard Settings")]
    [SerializeField] private string _leaderboardName = "leaderboard";
    [SerializeField] private int _topPlayersCount = 5;
    [SerializeField] private int _aroundPlayersCount = 2;

    private void Awake()
    {
        _saveLoadManager = new MirraSaveLoadManager();
        Debug.Log(_saveLoadManager);
    }
    
     public void SaveProgress() => _saveLoadManager.SaveProgress();

    // === Работа с машинами ===
    public bool[] PurchasedCarsArray() => _saveLoadManager.PurchasedCarsArray();
    public bool IsCarPurchased(int index) => _saveLoadManager.IsCarPurchased(index);
    public void SetCarPurchased(int index) => _saveLoadManager.SetCarPurchased(index);
    public void SaveSelectedCar(int index) => _saveLoadManager.SaveSelectedCar(index);
    public int LoadSelectedCar() => _saveLoadManager.LoadSelectedCar();

    // === Деньги ===
    public int LoadMoneyCount() => _saveLoadManager.LoadMoneyCount();
    public void SetMoneyCount(int amount = 500) => _saveLoadManager.SetMoneyCount(); // Фиксированное +500 (можно доработать)
    public void DeductMoney(int amount) => _saveLoadManager.DeductMoney(amount);

    // === Настройки звука ===
    public int LoadVolume() => _saveLoadManager.LoadVolume();
    public void SetVolume(int volume) => _saveLoadManager.SetVolume(volume);
    public bool LoadFXStatus() => _saveLoadManager.LoadFXStatus();
    public void SetVolumeFXStatus(bool status) => _saveLoadManager.SetVolumeFXStatus(status);

    // === Дрифт и рекорды ===
    public int LoadRecordDrift() => _saveLoadManager.LoadRecordDrift();
    public void SetRecordDrift(int record) => _saveLoadManager.SetRecordDrift(record);
    public int GetScoreDrift() => _saveLoadManager.GetScoreDrift();
    public void SetScoreDrift(int score) => _saveLoadManager.SetScoreDrift(score);

    // === Зомби-режим ===
    public int LoadKilledZombies() => _saveLoadManager.LoadKilledZombies();
    public void SetKilledZombiesCount() => _saveLoadManager.SetKilledZombiesCount();
    public void ResetKilledZombiesCount() => _saveLoadManager.ResetKilledZombiesCount();
    public bool GetZombieCompleteStatus() => _saveLoadManager.GetZombieCompleteStatus();
    public void SetZombieCompleteStatus() => _saveLoadManager.SetZombieCompleteStatus();

    // === Реклама ===
    public void RewardAdvShow() => _saveLoadManager.RewardAdvShow(_rewardAdId);
    public void FullscreenAdvShow() => _saveLoadManager.FullscreenAdvShow();
    public void PauseGame(bool pause) => _saveLoadManager.PauseGame(pause);

    // === Таблица лидеров ===
    public void GetLeaderboard(string name, int playerCountTop, int playerCountAround) => _saveLoadManager.GetLeaderboard(_leaderboardName, _topPlayersCount, _aroundPlayersCount);
    public void SetLeaderboard(string name,int score) => _saveLoadManager.SetLeaderboard(_leaderboardName, score);

    // === Язык ===
    public string GetLanguageCode() => _saveLoadManager.lang;
    public bool nowInterAdv;

    // ===Первый запуск ===


}
