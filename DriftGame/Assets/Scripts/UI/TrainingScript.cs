using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TrainingScript : MonoBehaviour
{
    [SerializeField] private Button _buttonResume;
    [SerializeField] private SaveLoadManager _saveLoadManager;
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }
    
    private void Awake()
    {
        _buttonResume.onClick.AddListener(StopPause);
    }

    public void StartPause()
    {
        Time.timeScale = 0.0f;
    }

    public void StopPause()
    {
        _saveLoadManager.SetFirstEnter();
        Time.timeScale = 1.0f;
        var trainAnim = gameObject.GetComponent<Animator>();
        trainAnim.Play("CloseTrain");
    }
}
