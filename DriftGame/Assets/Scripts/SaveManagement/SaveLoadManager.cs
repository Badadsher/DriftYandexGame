using System;
using System.Numerics;
using YG;

public abstract class SaveLoadManager
{
    public abstract bool nowInterAdv { get; }
    public abstract string lang { get; }

    public abstract void SaveProgress();

    public abstract bool[] PurchasedCarsArray();

    public abstract bool IsCarPurchased(int carId);
    
    public abstract void SaveSelectedCar(int carId);

    public abstract int LoadSelectedCar();
    
    public abstract void DeductMoney(int amount);

    public abstract void SetCarPurchased(int index);

    public abstract int LoadVolume();

    public abstract bool LoadFXStatus();

    public abstract int LoadMoneyCount();

    public abstract int LoadRecordDrift();

    public abstract int LoadKilledZombies();

    public abstract void SetRecordDrift(int record);

    public abstract void SetVolumeFXStatus(bool status);

    public abstract void SetVolume(int count);
    
    public abstract void SetKilledZombiesCount();

    public abstract void ResetKilledZombiesCount();

    public abstract void SetMoneyCount();

    public abstract void RewardAdvAddListener(Action<string> action);

    public abstract void RewardAdvRemoveListener(Action<string> action);
    
    public abstract void OnOpenAnyAdvAddListener(Action action);
    public abstract void OnOpenAnyAdvRemoveListener(Action action);

    public abstract void RewardAdvShow(string id);
    public abstract string SerializeBoolArray(bool[] array);

    public abstract bool[] DeserializeBoolArray(string array);
}


