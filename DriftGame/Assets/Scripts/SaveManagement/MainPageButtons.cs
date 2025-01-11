using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainPageButtons : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button[] buttons; // Массив кнопок

    [SerializeField] private GameObject activatorZombieScene;
    [SerializeField] private GameObject activatorDriftScene;
    [SerializeField] private GameObject activatorRampScene;
    
    [Header("Volume")]
    [SerializeField] private AudioClip levelSound; // Звук нажатия на уровень
    [SerializeField]  private AudioSource audioSource; // Компонент AudioSource

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }
    
    private void OnButtonClicked(int buttonIndex)
    {
        PlayToggleSound();
        switch (buttonIndex)
        {
            case 0:
                activatorZombieScene.SetActive(true);
                break;
            case 1:
                activatorDriftScene.SetActive(true);
                break;
            case 2:
                activatorRampScene.SetActive(true); ;
                break;
            default:
                break;
        }
    }
    
    private void PlayToggleSound()
    {
        if (audioSource != null && levelSound != null)
        {
            audioSource.PlayOneShot(levelSound); // Воспроизводим звук нажатия
        }
    }
}
