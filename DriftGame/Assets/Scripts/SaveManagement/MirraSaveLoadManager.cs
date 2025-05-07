using System;
using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using Unity.VisualScripting;
using UnityEngine;

public class MirraSaveLoadManager : SaveLoadManager
{
  private Action OnOpenAnyAdv;
  private Action OnCloseAnyAdv;
  private Action<string> OnRewardAdv;
  

  public override string lang
  {
    get
    {
      var langCode = MirraSDK.Language.Current.ToString();
      switch (langCode)
      {
        case "Turkish": return "tr";
        case "Spanish": return "es";
        case "German": return "de";
        case "Polish": return "pl";
        case "Swedish": return "sv";
        default: return langCode.Substring(0, 2).ToLower();
      }
    }
  }

  public override bool nowInterAdv => _nowInterAdv;
  private bool _nowInterAdv;

  public override void SaveProgress()
  {
    MirraSDK.Prefs.Save();
  }

  public override bool[] PurchasedCarsArray()
  {
    string savedData = PlayerPrefs.GetString("PurchasedCars", "");
    return DeserializeBoolArray(savedData);
  }

  public override bool IsCarPurchased(int index)
  {
    return MirraSDK.Prefs.GetInt("PurchasedCars_" + index, 0) == 1;
  }

  public override void SaveSelectedCar(int index)
  {
    MirraSDK.Prefs.SetInt("SelectedCarIndex", index);
    SaveProgress();
  }


  public override int LoadSelectedCar()
  {
    return MirraSDK.Prefs.GetInt("SelectedCarIndex", 0);
  }

  public override void DeductMoney(int amount)
  {
    int currentMoney = LoadMoneyCount();
    currentMoney -= amount;
    MirraSDK.Prefs.SetInt("Money", currentMoney);
    SaveProgress();
  }

  public override void SetCarPurchased(int index)
  {
    if (index >= 0 && index < 4)
    {
      MirraSDK.Prefs.SetInt("PurchasedCars_" + index, 1);
      SaveProgress();
    }
    else
    {
      Debug.LogError("Index out of bounds for purchasedCars array.");
    }
  }

  public override int LoadVolume()
  {
    return MirraSDK.Prefs.GetInt("VolumeCount", 100);
  }

  public override bool LoadFXStatus()
  {
    return MirraSDK.Prefs.GetInt("VolumeFXStatus", 1) == 1;
  }

  public override int LoadMoneyCount()
  {
    return MirraSDK.Prefs.GetInt("Money", 0);
  }

  public override int LoadRecordDrift()
  {
    return MirraSDK.Prefs.GetInt("RecordDrift", 0);
  }

  public override int LoadKilledZombies()
  {
    return MirraSDK.Prefs.GetInt("ZombieCount", 0);
  }

  public override void SetRecordDrift(int record)
  {
    MirraSDK.Prefs.SetInt("RecordDrift", record);
    SaveProgress();
  }

  public override void SetVolumeFXStatus(bool status)
  {
    MirraSDK.Prefs.SetInt("VolumeFXStatus", status ? 1 : 0);
    SaveProgress();
  }

  public override void SetVolume(int count)
  {
    MirraSDK.Prefs.SetInt("VolumeCount", count);
    SaveProgress();
  }

  public override void SetKilledZombiesCount()
  {
    int currentCount = LoadKilledZombies();
    currentCount += 1;
    MirraSDK.Prefs.SetInt("ZombieCount", currentCount);
    SaveProgress();
  }

  public override void ResetKilledZombiesCount()
  {
    MirraSDK.Prefs.SetInt("ZombieCount", 0);
    SaveProgress();
  }

  public override void SetMoneyCount()
  {
    int currentMoney = LoadMoneyCount();
    currentMoney += 500;
    MirraSDK.Prefs.SetInt("Money", currentMoney);
    SaveProgress();
  }

  public override void RewardAdvShow(string id)
  {
    MirraSDK.Ads.InvokeRewarded(onSuccess: () => { OnRewardAdv?.Invoke(id); }, rewardTag: id);

  }

  public override void RewardAdvAddListener(Action<string> action)
  {
    OnRewardAdv += action;
  }

  public override void RewardAdvRemoveListener(Action<string> action)
  {
    OnRewardAdv -= action;
  }

  public override void OnOpenAnyAdvAddListener(Action action)
  {
    OnOpenAnyAdv += action;
  }

  public override void OnOpenAnyAdvRemoveListener(Action action)
  {
    OnOpenAnyAdv -= action;
  }

  // public override void OnCloseAnyAdvAddListener(Action action)
  // {
  //   OnCloseAnyAdv += action;
  // }
  //
  // public override void OnCloseAnyAdvRemoveListener(Action action)
  // {
  //   OnCloseAnyAdv -= action;
  // }


public override string SerializeBoolArray(bool[] array)
  {
    return string.Join(",", System.Array.ConvertAll(array, item => item ? "1" : "0"));
  }
  
  public override bool[] DeserializeBoolArray(string savedData)
  {
    if (string.IsNullOrEmpty(savedData))
      return new bool[4];

    string[] splitData = savedData.Split(',');
    bool[] result = new bool[splitData.Length];
    for (int i = 0; i < splitData.Length; i++)
    {
      result[i] = splitData[i] == "1";
    }
    return result;
  }
  
  public override void PauseGame(bool state)
  {
    MirraSDK.Time.Scale = state ? 0 : 1;
  }
  
  public override void FullscreenAdvShow()
  {
    MirraSDK.Ads.InvokeInterstitial(onAnyClose: () =>
    { 
      _nowInterAdv = false;
      OnCloseAnyAdv?.Invoke();
      MirraSDK.Time.Scale = 1;
    });
    _nowInterAdv = true;
    OnOpenAnyAdv?.Invoke();
    // Cursor.lockState = CursorLockMode.None;
    // Cursor.visible = true;
  }

  public override void GetLeaderboard(string name, int playerCountTop, int playerCountAround)
  {
    Debug.Log($"Get Board: {name}");

    MirraSDK.Socials.GetScoreTable(name, playerCountTop, true, playerCountAround, (data) =>
    {
      Debug.Log($"Success get board");
      var playersData = new List<LeaderboardPlayerData>();
      for (int i = 0; i < data.Count; i++)
      {
        var playerDt = data[i];
        playersData.Add(new LeaderboardPlayerData(
          name: playerDt.name,
          score: playerDt.score,
          position: playerDt.position,
          imgUrl: playerDt.pictureURL
        ));
      }

      var lbData = new LeaderboardData(name, playersData.ToArray());
      OnGetLeaderboadData?.Invoke(lbData);
    }, () => { Debug.Log("Error getting leaderboard"); });
  }
  
  public override void SetLeaderboard(string boardName, int value)
  {
    Debug.Log($"Set Score: {boardName} {value}");
    MirraSDK.Socials.SetScore(boardName, value);
  }

  
}
