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

  // загрузка кол-ва открытых машин
  public static int LoadUnlockedCarCount()
  {
    return YG2.saves._unlockedCarCount;
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

  //открытие машин
  public static void SetLoadUnlockedCount()
  {
    YG2.saves._unlockedCarCount += 1;
    SaveProgress();
  }

  //запись убитых зомби
  public static void SetKilledZombiesCount()
  {
    YG2.saves._zombieCount += 1;
    SaveProgress();
  }
  
  
  
  //добавление денег
  public static void SetMoneyCount()
  {
    YG2.saves._money += 500;
    SaveProgress();
  }

 
  
}
