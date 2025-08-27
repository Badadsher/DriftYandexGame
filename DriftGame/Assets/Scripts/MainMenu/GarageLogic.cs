using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using romanlee17.MirraGames.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class GarageLogic : MonoBehaviour
{
     [Header("Toggle Settings")]
    [SerializeField] private Toggle[] toggles; // Массив переключателей 
     
    [Header("Colors")]
    [SerializeField] private Color activeColor; // Цвет активного переключателя
    [SerializeField] private Color inactiveColor ; // Цвет неактивных переключателей
    
    [Header("Cars")]
    [SerializeField] private GameObject[] carList;
    
    [Header("Lockers")]
    [SerializeField] private GameObject[] lockers;
    
    [Header("ButtonsForCar")]
    [SerializeField] private Button[] carButtons;
    
    [Header("CarTextDesc")]
    [SerializeField] private Text[] carTextDesk;
    
    private int selectedCarIndex = 0; // Индекс выбранной машины, по умолчанию первая (0)
    // Массив цен для каждой машины
    [Header("Car Prices")]
    [SerializeField] private int[] carPrices; // Цены на машины
    
    [Header("ManagerMoney")]
    [SerializeField] private UpperMenuButtons managerMoney;

    [SerializeField] private GameObject noMoney;
    [SerializeField] private GameObject succesBuying;
    
    [Header("Volume")]
    [SerializeField] private AudioClip toggleSound; // Звук нажатия на кнопку
    [SerializeField] private AudioClip paySound; // Звук оплаты
    [SerializeField] private AudioClip carClick; // Звук клика выбора
    [SerializeField] private AudioSource audioSource; // Компонент AudioSource

    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;
    
   
    
    void Start()
    {
     
        UpdateCarStates();  
        
        // Устанавливаем обработчики событий для каждого переключателя
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
        
        InitializeCarSelection(); // Инициализация выбора первой машины

        // Устанавливаем обработчики событий для каждой кнопки
        for (int i = 0; i < carButtons.Length; i++)
        {
            int index = i; // Локальная переменная для замыкания
            carButtons[i].onClick.AddListener(() => OnCarButtonClicked(index));
        }
        
        UpdateButtonTexts(); // Обновляем текст кнопок при старте
    }
    
    private void UpdateCarStates()
    {

        for (int i = 0; i < carList.Length; i++)
        {
            if (i < _saveLoadManager.PurchasedCarsArray().Length) // Проверяем границы массива
            {
                if (i > 0)
                {
                    if (_saveLoadManager.IsCarPurchased(i))
                    {
                        lockers[i - 1].SetActive(false);
                    }
                    else
                    {
                        lockers[i - 1].SetActive(true);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Index " + i + " is out of bounds for purchasedCars array.");
            }
        }
    }
    
    private void InitializeCarSelection()
    {
        selectedCarIndex = _saveLoadManager.LoadSelectedCar(); // Загружаем индекс выбранной машины

        if (!_saveLoadManager.IsCarPurchased(selectedCarIndex))
        {
            selectedCarIndex = 0; // Если сохраненная машина не куплена, выбираем первую по умолчанию
        }

        else
        {
            carList[selectedCarIndex].SetActive(true);
            carButtons[selectedCarIndex].gameObject.SetActive(true);
        }
        // Устанавливаем состояние переключателей
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = (i == selectedCarIndex); // Включаем переключатель, если индекс совпадает с выбранным
            SetToggleAppearance(toggles[i], toggles[i].isOn); // Устанавливаем внешний вид переключателя
        }
        
        UpdateButtonTexts(); // Обновляем текст кнопок при инициализации
    }

    private void OnCarButtonClicked(int index)
    {
        PlayToggleSound(2);
        if (_saveLoadManager.IsCarPurchased(index))
        {
            if (selectedCarIndex == index)
            {
                // Если уже выбрана эта машина, ничего не делаем или можно добавить логику сброса выбора
                Debug.Log("Машина уже выбрана: " + index);
             
            }
            else
            {
                // Выбираем новую машину
                selectedCarIndex = index;
                Debug.Log("Выбрана машина: " + index);
                _saveLoadManager.SaveSelectedCar(selectedCarIndex); 
                UpdateButtonTexts(); // Обновляем текст кнопок после выбора
            }
        }
        else
        {
            TryPurchaseCar(index); 
        }
    }
    
    private void TryPurchaseCar(int index)
    {
        int price = carPrices[index]; // Получаем цену выбранной машины

        if (_saveLoadManager.LoadMoneyCount() >= price) // Проверяем, достаточно ли денег
        {
            _saveLoadManager.DeductMoney(price); // Уменьшаем количество денег на цену машины
            _saveLoadManager.SetCarPurchased(index); // Помечаем машину как купленную

            managerMoney.UpdateMoney();
            
            Debug.Log("Машина " + (index + 1) + " куплена!");
            succesBuying.SetActive(true);
            PlayToggleSound(1);
            
            UpdateCarStates(); // Обновляем состояние машин после покупки
            UpdateButtonTexts(); // Обновляем текст кнопок после покупки
            
            if (selectedCarIndex == -1) 
            {
                selectedCarIndex = index; // Если ничего не выбрано, устанавливаем только что купленную машину как выбранную.
                _saveLoadManager.SaveSelectedCar(selectedCarIndex); 
                UpdateButtonTexts(); 
            }
        }
        else
        {
            noMoney.SetActive(true);
            Debug.Log("Недостаточно денег для покупки машины " + (index + 1));
        }
    }


    private void UpdateButtonTexts()
    {
        LanguageType languageType = MirraSDK.Language.Current;
        Debug.Log(languageType);
        for (int i = 0; i < carButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = carButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (i == selectedCarIndex)
            {
                if (languageType == LanguageType.Russian)
                {
                    buttonText.text = "Выбрано"; // Текст для выбранной машины
                    carTextDesk[i].text = "ВЫБРАНА";
                }
              else if (languageType == LanguageType.Turkish)
              {
                  buttonText.text = "SEÇME"; // Текст для выбранной машины
                  carTextDesk[i].text = "SEÇME";
              }
              else
              {
                  buttonText.text = "SELECTED"; // Текст для выбранной машины
                  carTextDesk[i].text = "SELECTED";
              }
            }
            else if (_saveLoadManager.IsCarPurchased(i))
            {
                if (languageType == LanguageType.Russian)
                {
                    buttonText.text = "Выбрать"; // Текст для доступных машин
                    carTextDesk[i].text = "Доступно";
                }
                else if (languageType == LanguageType.Turkish)
                {
                    buttonText.text = "SEÇMEK"; // Текст для выбранной машины
                    carTextDesk[i].text = "MEVCUT";
                }
                else
                {
                    buttonText.text = "SELECT"; // Текст для выбранной машины
                    carTextDesk[i].text = "SELECT";
                }
            }
            else
            {
              
                
                if (languageType == LanguageType.Russian)
                {
                    buttonText.text = "Купить";
                    carTextDesk[i].text = $"Цена: {carPrices[i]}"; 
                }
                else if (languageType == LanguageType.Turkish)
                {
                    buttonText.text = "ALMAK"; // Текст для выбранной машины
                    carTextDesk[i].text = $"FİYAT: {carPrices[i]}";                 }
                else
                {
                    buttonText.text = "BUY"; // Текст для выбранной машины
                    carTextDesk[i].text = $"PRICE: {carPrices[i]}";      
                }
            }
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        PlayToggleSound(0);
        // Если переключатель включен, отключаем остальные
        if (changedToggle.isOn)
        {
            foreach (var toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false; // Отключаем другие переключатели
                    SetToggleAppearance(toggle, false); // Меняем внешний вид
                }
            }

            // Меняем цвет и текст у нажатого переключателя
            SetToggleAppearance(changedToggle, true);
            // Выполняем уникальную логику для каждого переключателя
            ExecuteToggleLogic(changedToggle);
     
        }
    }
    
    private void PlayToggleSound(int variant)
    {
        if (variant == 0)
        {
            if (audioSource != null && toggleSound != null)
            {
                audioSource.PlayOneShot(toggleSound); // Воспроизводим звук нажатия
            }
        }
        else if(variant == 1)
        {
            if (audioSource != null && paySound != null)
            {
                audioSource.PlayOneShot(paySound); // Воспроизводим звук покупки
            }
        }
        else
        {
            if (audioSource != null && paySound != null)
            {
                audioSource.PlayOneShot(carClick); // Воспроизводим звук клика на кнопку машины
            }
        }
    }


    private void SetToggleAppearance(Toggle toggle, bool isActive)
    {
        Image toggleImage = toggle.GetComponent<Image>();

        if (toggleImage != null)
        {
            toggleImage.color = isActive ? activeColor : inactiveColor; // Устанавливаем цвет

        }
    }
    private void ExecuteToggleLogic(Toggle activeToggle)
    {
        if (activeToggle == toggles[0])
        {
            
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            foreach (var button in carButtons)
            {
                button.gameObject.SetActive(false);
            }
            carButtons[0].gameObject.SetActive(true);
            UpdateButtonTexts();
            carList[0].SetActive(true);
        }
        else if (activeToggle == toggles[1])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            foreach (var button in carButtons)
            {
                button.gameObject.SetActive(false);
            }
            carButtons[1].gameObject.SetActive(true);
            UpdateButtonTexts();
            carList[1].SetActive(true);
        }
        else if (activeToggle == toggles[2])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            foreach (var button in carButtons)
            {
                button.gameObject.SetActive(false);
            }
            carButtons[2].gameObject.SetActive(true);
            UpdateButtonTexts();
            carList[2].SetActive(true);
        }
        if (activeToggle == toggles[3])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            foreach (var button in carButtons)
            {
                button.gameObject.SetActive(false);
            }
            carButtons[3].gameObject.SetActive(true);
            UpdateButtonTexts();
            carList[3].SetActive(true);
        
        }
    }
}
