using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public static class  SaveManager
{
  // сохренение
  public static void SaveProgress()
  {
    YG2.SaveProgress();
  }

  //выгрузка массива купленный машин
  public static bool[] PurchasedCarsArray()
  {
    return YG2.saves.purchasedCars;
  }
  
  //получение статуса куплена ли машина по индексу
    public static bool IsCarPurchased(int index)
    {
      return YG2.saves.purchasedCars[index];
    }
    
    // Сохраняем индекс выбранной машины
    public static void SaveSelectedCar(int index)
    {
      YG2.saves.selectedCarIndex = index; 
      // Здесь добавьте код для фактического сохранения данных (например, в файл или базу данных)
    }
    
    // Загружаем индекс выбранной машины
    public static int LoadSelectedCar()
    {
      return YG2.saves.selectedCarIndex; 
    }
    
  // загрузка уровня громкости
  public static int LoadVolume()
  {
    return YG2.saves._volumeCount;
  }
  // загрузка статуса крутой графики
  public static bool LoadFXStatus()
  {
    return YG2.saves._volumeFXStatus;
  }
  //загрузка кол-ва денег
  public static int LoadMoneyCount()
  {
    return YG2.saves._money;
  }
  
  //загрузка рекорда
  public static int LoadRecordDrift()
  {
    return YG2.saves._recordDrift;
  }
  
  //загрузка убитых зомби
  public static int LoadKilledZombies()
  {
    return YG2.saves._zombieCount;
  }

  //установка нового рекорда
  public static void SetRecordDrift(int record)
  {
    YG2.saves._recordDrift = record;
    SaveProgress();
  }
  
  //установка графики
  public static void SetVolumeFXStatus(bool status)
  {
    YG2.saves._volumeFXStatus = status;
    SaveProgress();
  }
  
  //установка звука
  public static void SetVolume(int count)
  {
    YG2.saves._volumeCount = count;
    SaveProgress();
  }

  //открытие машин
  public static void SetLoadUnlockedCount()
  {
    ///////
    SaveProgress();
  }

  //запись убитых зомби
  public static void SetKilledZombiesCount()
  {
    YG2.saves._zombieCount += 1;
    SaveProgress();
  }
  
  //сброс убитых зомби
  public static void ResetKilledZombiesCount()
  {
    YG2.saves._zombieCount = 0;
    SaveProgress();
  }
  
  
  //добавление денег
  public static void SetMoneyCount()
  {
    YG2.saves._money += 500;
    SaveProgress();
  }

 
  
}
