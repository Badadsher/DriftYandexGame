using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MainPageButtons : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button[] buttons; // Массив кнопок

    [SerializeField] private GameObject activatorZombieScene;
    [SerializeField] private GameObject driftScenesLists;
    [SerializeField] private GameObject activatorRampScene;
    
    [Header("Volume")]
    [SerializeField] private AudioClip levelSound; // Звук нажатия на уровень
    [SerializeField]  private AudioSource audioSource; // Компонент AudioSource

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private AnimationCurve showCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve hideCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float hideYOffset = -50f; 
    
    [SerializeField] private Button closeDriftScenes;
    
    private Coroutine currentAnimation;
    private bool isDriftSceneVisible = false;
    private Vector3 originalPosition;
    
    private void Start()
    {
        closeDriftScenes.onClick.AddListener(ToggleDriftScene);
        originalPosition = driftScenesLists.transform.localPosition;
        driftScenesLists.transform.localScale = Vector3.zero;
        driftScenesLists.SetActive(false);
        
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
                ToggleDriftScene();
                break;
            case 2:
                activatorRampScene.SetActive(true);
                break;
        }
    }
    
    private void ToggleDriftScene()
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        // Запускаем анимацию в зависимости от текущего состояния
        if (isDriftSceneVisible)
            currentAnimation = StartCoroutine(AnimateDriftScene(false));
        else
        {
            driftScenesLists.SetActive(true);
            currentAnimation = StartCoroutine(AnimateDriftScene(true));
        }
    }
    
    private IEnumerator AnimateDriftScene(bool show)
    {
        float elapsed = 0f;
        Vector3 startScale = show ? Vector3.zero : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.zero;
        Vector3 startPosition = show ? originalPosition + new Vector3(0, hideYOffset, 0) : originalPosition;
        Vector3 endPosition = show ? originalPosition : originalPosition + new Vector3(0, hideYOffset, 0);
        AnimationCurve curve = show ? showCurve : hideCurve;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / animationDuration);
            
            driftScenesLists.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            
            if (!show)
                driftScenesLists.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            else
                driftScenesLists.transform.localPosition = originalPosition;
            
            yield return null;
        }

        // Финализируем состояние
        driftScenesLists.transform.localScale = endScale;
        driftScenesLists.transform.localPosition = endPosition;

        if (!show)
            driftScenesLists.SetActive(false);
        
        // Меняем состояние только после завершения анимации
        isDriftSceneVisible = show;
    }
    
    private void PlayToggleSound()
    {
        if (audioSource != null && levelSound != null)
        {
            audioSource.PlayOneShot(levelSound); // Воспроизводим звук нажатия
        }
    }
}
