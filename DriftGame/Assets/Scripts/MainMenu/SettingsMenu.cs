using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class SettingsMenu : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle graphicToggle;
    [SerializeField] private TextMeshProUGUI volumeCount;
    
    [Header("Volume")]
    [SerializeField] private AudioClip toggleSound;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;
    


    
    private void Start()
    {
        volumeCount.text = _saveLoadManager.LoadVolume().ToString();
        graphicToggle.isOn = _saveLoadManager.LoadFXStatus();
        graphicToggle.onValueChanged.AddListener(OnGraphicToggleChanged);
        
        volumeSlider.value = _saveLoadManager.LoadVolume();
        
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }
    private void OnVolumeSliderChanged(float value)
    {
        volumeCount.text = volumeSlider.value.ToString();
        _saveLoadManager.SetVolume(Convert.ToInt32(value));
    }
    private void OnGraphicToggleChanged(bool isOn)
    {
        PlayToggleSound();
        if (isOn)
        {
            _saveLoadManager.SetVolumeFXStatus(true);
        }
        else
        {
            _saveLoadManager.SetVolumeFXStatus(false);
        }
    }
    
    private void PlayToggleSound()
    {
        if (audioSource != null && toggleSound != null)
        {
            audioSource.PlayOneShot(toggleSound); // Воспроизводим звук нажатия
        }
    }

    
}