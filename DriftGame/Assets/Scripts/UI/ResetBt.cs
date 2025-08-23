using System;
using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetBt : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _toMenuButtons;
    [SerializeField] private Button[] _toResetButtons;
    [SerializeField] private Button  _resetCar;
    [SerializeField] private Button[] _getChanseButton;
    [Header("UI")]
    [SerializeField] private GameObject _loadermenuUI;
    [SerializeField] private GameObject _reseterUi;
    [SerializeField] private GameManager _gameManager;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private GameObject player;
    private int level;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCar();
        }
    }
    private void Awake()
    {
        if (Bootstrap.isInitialized)
        {
            OnSDKDataReceived();
        }
    }

    private void OnSDKDataReceived()
    {
        if (_getChanseButton != null)
        {
            foreach (Button chanseButton in _getChanseButton)
            {
                chanseButton.onClick.AddListener(ChanseReset);
            }
        }
    }

    public void InitializeReseter()
    {
        player = GameObject.FindObjectOfType<PrometeoCarController>().gameObject;
        initialPosition = player.transform.position;
        initialRotation = player.transform.rotation;
        if (_toMenuButtons != null)
        {
            foreach (Button menuButton in _toMenuButtons)
            {
                menuButton.onClick.AddListener(StartMenuON);
            }
        }

        if (_resetCar != null)
        {
            _resetCar.onClick.AddListener(ResetCar);
        }

        if (_getChanseButton != null)
        {
            foreach (Button chanseButton in _getChanseButton)
            {
                chanseButton.onClick.AddListener(ChanseReset);
            }
        }

        if (_toResetButtons != null)
        {
            foreach (Button resetButton in _toResetButtons)
            {
                resetButton.onClick.AddListener(StartResetScene);
            }
        }
        level = SceneManager.GetActiveScene().buildIndex;
    }
    public void UpperCar()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = (transform.position - player.transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * 200000f);
    }

    private void ResetCar()
    {
        player.GetComponent<PrometeoCarController>().enabled = false;

        // Телепортируем машину в начальную позицию и поворот
        player.transform.position = initialPosition;
        player.transform.rotation = initialRotation; // Восстанавливаем начальный поворот

        // Сразу же сбрасываем скорость до нуля (или небольшого значения), чтобы избежать неконтролируемого движения после телепортации
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;  // или  player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0.1f);
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        // Включаем управление снова (с небольшой задержкой)
        Invoke("EnableCarController", 0.1f); // Задержка нужна, чтобы все правильно перезагрузилось
    
    }

    private void ChanseReset()
    {
        Debug.Log("reset car");
        MirraSDK.Ads.InvokeRewarded(
            onSuccess: () =>
            {
                _gameManager.loseBar.SetActive(false);
                _gameManager.ResetChanseScene();
                ResetCar();
            },
            onNotReady: () => {   Debug.Log("Rewarded Ad not READY"); },
            onAnyClose: () => { return; },
            rewardTag: "CHANSE"
        );
    }

    void EnableCarController()
    {
        player.GetComponent<PrometeoCarController>().enabled = true;
    }

    public void StartMenuON()
    {
        _loadermenuUI.SetActive(true);
    }

    public void StartResetScene()
    {
        _reseterUi.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(level);
    }
    public void MenuON()
    {
        SceneManager.LoadScene(0);
    }
}
