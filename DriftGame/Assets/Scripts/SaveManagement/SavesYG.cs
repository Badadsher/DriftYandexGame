using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
namespace YG
{
    public partial class SavesYG
    {  //деньги
        public int _money;
        //рекорд дрифта
        public int _recordDrift;
        //кол-во зомби
        public int _zombieCount = 0;
        //графон
        public bool _volumeFXStatus = true; 
        //звук
        public int _volumeCount = 100;
        //индекс выбранной машина
        public int _selectedCar;
        //выбранная машина
        public bool[] selectedCar = new bool[4];
        //купленные машины
        public bool[] purchasedCars = new bool[4]; // 4 машины

        // Индекс выбранной машины
        public int selectedCarIndex = 0;
 
        public SavesYG()
        {
            // Инициализация массива: первая машина выбрана, остальные нет
            selectedCar[0] = true; // Первая машина куплена
            for (int i = 1; i < selectedCar.Length; i++)
            {
                selectedCar[i] = false; // Остальные машины не куплены
            }
            // Инициализация массива: первая машина куплена, остальные нет
            purchasedCars[0] = true; // Первая машина куплена
            for (int i = 1; i < purchasedCars.Length; i++)
            {
                purchasedCars[i] = false; // Остальные машины не куплены
            }
        }
    }
}