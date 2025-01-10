using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
            if (i < SaveManager.PurchasedCarsArray().Length) // Проверяем границы массива
            {
                if (i > 0)
                {
                    if (SaveManager.IsCarPurchased(i))
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
        selectedCarIndex = SaveManager.LoadSelectedCar(); // Загружаем индекс выбранной машины

        if (!SaveManager.IsCarPurchased(selectedCarIndex))
        {
            selectedCarIndex = 0; // Если сохраненная машина не куплена, выбираем первую по умолчанию
        }

        else
        {
            carList[selectedCarIndex].SetActive(true);
            carButtons[selectedCarIndex].gameObject.SetActive(true);
        }
        
        UpdateButtonTexts(); // Обновляем текст кнопок при инициализации
    }

    private void OnCarButtonClicked(int index)
    {
        if (SaveManager.IsCarPurchased(index))
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
                SaveManager.SaveSelectedCar(selectedCarIndex); 
                UpdateButtonTexts(); // Обновляем текст кнопок после выбора
            }
        }
    }

    private void UpdateButtonTexts()
    {
        for (int i = 0; i < carButtons.Length; i++)
        {
            Text buttonText = carButtons[i].GetComponentInChildren<Text>();
            if (i == selectedCarIndex)
            {
                buttonText.text = "Выбрано"; // Текст для выбранной машины
                carTextDesk[i].text = "ДОСТУПНО";
            }
            else if (SaveManager.IsCarPurchased(i))
            {
                buttonText.text = "Выбрать"; // Текст для доступных машин
                carTextDesk[i].text = "Доступно";
            }
            else
            {
                buttonText.text = "Купить";
                carTextDesk[i].text = $"Цена: {carPrices[i]}"; 
            }
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
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
