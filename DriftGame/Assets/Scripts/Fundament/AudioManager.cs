using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class AudioManager : MonoBehaviour
{
    private int volume;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;
    
  
    
    private void Start()
    {
        
        volume = _saveLoadManager.LoadVolume();
        // Получаем все аудиоисточники на сцене
        audioSources = FindObjectsOfType<AudioSource>();

        Debug.Log(volume);
        // Устанавливаем громкость для каждого аудиоисточника
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume /100f;
        }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
         
            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        }
    }
    
    private void OnVolumeSliderChanged(float value)
    {
        // Устанавливаем громкость для каждого аудиоисточника
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = value/100f;
           
        }
    }
}
