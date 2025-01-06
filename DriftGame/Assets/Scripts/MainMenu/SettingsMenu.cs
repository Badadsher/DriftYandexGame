using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle graphicToggle;
    [SerializeField] private TextMeshProUGUI volumeCount;
    private void Start()
    {
        // Устанавливаем состояние Toggle при запуске сцены
        graphicToggle.isOn = SaveManager.LoadFXStatus();
        // Подписываемся на событие изменения состояния Toggle
        graphicToggle.onValueChanged.AddListener(OnGraphicToggleChanged);
        
        // Устанавливаем начальное значение слайдера и подписываемся на его изменение
        volumeSlider.value = SaveManager.LoadVolume(); // Предполагается, что вы используете AudioListener для регулировки громкости
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }
    private void OnVolumeSliderChanged(float value)
    {
        volumeCount.text = volumeSlider.value.ToString();
        SaveManager.SetVolume(Convert.ToInt32(value));
    }
    private void OnGraphicToggleChanged(bool isOn)
    {
        if (isOn)
        {
           SaveManager.SetVolumeFXStatus(true);
        }
        else
        {
            SaveManager.SetVolumeFXStatus(false);
        }
    }
    
}