using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingScript : MonoBehaviour
{
    [SerializeField] private Button _buttonResume;

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
        Time.timeScale = 1.0f;
        var trainAnim = gameObject.GetComponent<Animator>();
        trainAnim.Play("CloseTrain");
    }
}
