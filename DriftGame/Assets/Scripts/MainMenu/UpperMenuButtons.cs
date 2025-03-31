using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UpperMenuButtons : MonoBehaviour
{
    [Header("Toggle Settings")]
    [SerializeField] private Toggle[] toggles; // Массив переключателей 
    [SerializeField] private Color activeColor; // Цвет активного переключателя
    [SerializeField] private Color inactiveColor ; // Цвет неактивных переключателей
    [SerializeField] private Color activeColorText ; // Цвет активного переключателя
    [SerializeField] private Color inactiveColorText; // Цвет неактивных переключателей
    
    [Header("Pages")]
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject garagePage;
    
    [Header("Objects")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject cars;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject adButton;
    
    [Header("Volume")]
    [SerializeField] private AudioClip toggleSound;
    [SerializeField]  private AudioSource audioSource;
    
    private SaveLoadManager _saveLoadManager;
    
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
        Debug.Log("initialization complete");
    }
    
    
    
    void Start()
    {
        Debug.Log("initialization complete2");
        UpdateMoney();
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
    }

    public void UpdateMoney()
    {
        Debug.Log(_saveLoadManager);
        moneyText.text = _saveLoadManager.LoadMoneyCount().ToString();
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        // Если переключатель включен, отключаем остальные
        if (changedToggle.isOn)
        {
            PlayToggleSound();
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
    
    private void PlayToggleSound()
    {
        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound); // Воспроизводим звук нажатия
        }
    }

    private void SetToggleAppearance(Toggle toggle, bool isActive)
    {
        Image toggleImage = toggle.GetComponent<Image>();

        if (toggleImage != null)
        {
            toggleImage.color = isActive ? activeColor : inactiveColor; // Устанавливаем цвет
        }

        // Изменяем текст внутри Toggle (если есть)
        Text labelText = toggle.GetComponentInChildren<Text>();
        if (labelText != null)
        {
            labelText.color = isActive ? activeColorText : inactiveColorText;
        }
    }
    private void ExecuteToggleLogic(Toggle activeToggle)
    {
        // Проверяем, какой именно Toggle активирован и выполняем соответствующую логику
        if (activeToggle == toggles[0])
        {
            mainPage.SetActive(true);
            settingsPage.SetActive(false);
            garagePage.SetActive(false);
            playerModel.SetActive(true);
            cars.SetActive(false);
            adButton.SetActive(false);
        }
        else if (activeToggle == toggles[1])
        {
            mainPage.SetActive(false);
            settingsPage.SetActive(true);
            garagePage.SetActive(false);
            playerModel.SetActive(true);
            cars.SetActive(false);
            adButton.SetActive(false);
        }
        else if (activeToggle == toggles[2])
        {
            mainPage.SetActive(false);
            settingsPage.SetActive(false);
            garagePage.SetActive(true);
            playerModel.SetActive(false);
            cars.SetActive(true);
            adButton.SetActive(true);
        }
    }
}
