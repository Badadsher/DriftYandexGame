using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private int volume;
    [SerializeField] private Slider volumeSlider;
    private AudioSource[] audioSources;
    void Start()
    {
        
        volume = SaveManager.LoadVolume();
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
        Debug.Log("изменено");
    }
}
