using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class MainPageButtons : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button[] buttons; // Массив кнопок
    [SerializeField] private Button[] driftButtons;

    [SerializeField] private GameObject driftBlockator;
    [SerializeField] private GameObject driftUnlocker;
    [SerializeField] private GameObject activatorZombieScene;
    [SerializeField] private GameObject driftScenesLists;
    [SerializeField] private GameObject activatorRampScene;
    [SerializeField] private GameObject activatorDriftSpringScene;
    [SerializeField] private GameObject activatorDriftWinterScene;
    
    [Header("Volume")]
    [SerializeField] private AudioClip levelSound; // Звук нажатия на уровень
    [SerializeField]  private AudioSource audioSource; // Компонент AudioSource
    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private float targetScale = 1.1f;
    private SaveLoadManager _saveLoadManager;
 
    private CanvasGroup driftCanvasGroup;
    private Vector3 originalScale;
    
    [SerializeField] private Button closeDriftScenes;
    private bool isActivated = false;
    
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
        Debug.Log(_saveLoadManager.GetScoreDrift());
    }
    private void Start()
    {
        if (driftScenesLists != null)
        {
            driftCanvasGroup = driftScenesLists.GetComponent<CanvasGroup>();
            if (driftCanvasGroup == null)
            {
                driftCanvasGroup = driftScenesLists.AddComponent<CanvasGroup>();
            }
            
            originalScale = driftScenesLists.transform.localScale;
        }

        closeDriftScenes.onClick.AddListener(ToggleDriftScene);

        if (!_saveLoadManager.GetZombieCompleteStatus())
        {
            buttons[1].interactable = false;
            driftBlockator.SetActive(true);
            driftUnlocker.SetActive(false);
        }
        else
        {
            buttons[1].interactable = true;
            driftBlockator.SetActive(false);
            driftUnlocker.SetActive(true);
        }
        
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; 
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
        for (int i = 0; i < driftButtons.Length; i++)
        {
            int index = i; 
            driftButtons[i].onClick.AddListener(() => OnDriftClicked(index));
        }
    }

    private void OnDriftClicked(int index)
    {
        PlayToggleSound();
        switch (index)
        {
            case 0:
                activatorDriftSpringScene.SetActive(true);
                break;
            case 1:
                activatorDriftWinterScene.SetActive(true);
                break;
        }
    }
    
    private void OnButtonClicked(int buttonIndex)
    {
        PlayToggleSound();
        
        Debug.Log(buttonIndex);
        switch (buttonIndex)
        {
            case 0:
                activatorZombieScene.SetActive(true);
                break;
            case 1:
                ToggleDriftScene();
                break;
            case 2:
                activatorRampScene.SetActive(true);
                break;
        }
    }
    
    private void ToggleDriftScene()
    {
        PlayToggleSound();
        isActivated = !isActivated;
        if (isActivated)
        {
            // Анимация открытия
            driftScenesLists.SetActive(true);
            
            // Сброс перед анимацией
            driftCanvasGroup.alpha = 0f;
            driftScenesLists.transform.localScale = originalScale * 0.8f;
            
            // Параллельные анимации
            Sequence openSequence = DOTween.Sequence();
            openSequence.Join(driftCanvasGroup.DOFade(1f, fadeDuration));
            openSequence.Join(driftScenesLists.transform.DOScale(originalScale * targetScale, scaleDuration).SetEase(Ease.OutBack));
            openSequence.Append(driftScenesLists.transform.DOScale(originalScale, scaleDuration * 0.5f));
        }
        else
        {
            // Анимация закрытия
            Sequence closeSequence = DOTween.Sequence();
            closeSequence.Join(driftCanvasGroup.DOFade(0f, fadeDuration));
            closeSequence.Join(driftScenesLists.transform.DOScale(originalScale * 0.8f, scaleDuration)
                .SetEase(Ease.InBack));
            closeSequence.OnComplete(() => driftScenesLists.SetActive(false));
        }
    }
    
 
    
    private void PlayToggleSound()
    {
        if (audioSource != null && levelSound != null)
        {
            audioSource.PlayOneShot(levelSound); 
        }
    }
}
