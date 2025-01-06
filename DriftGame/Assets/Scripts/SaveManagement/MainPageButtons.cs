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
}
