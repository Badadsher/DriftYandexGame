using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public LevelType type { get; private set; }
    private Slider hpBar;
    
    [Header("UI")]
    [SerializeField] public GameObject loseBar;
    [SerializeField] private GameObject loadLevelUI;
    [SerializeField] private GameObject mobileUI;
    [SerializeField] private GameObject trainingUI;
    [SerializeField] private bool _isRace;
    
    [Header("Audio")]
    [SerializeField] private AudioSource mainAudioSource;
    
    [Header("ForCar")]
    [SerializeField] private AudioClip heatClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private GameObject[] cars;
    private int selectedCarIndex;
    [SerializeField] private CinemachineVirtualCamera carCamera;
    
    [Header("CarSounds")]
    [SerializeField] private AudioSource carEngine;
    private GameObject volume;
    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;
    private bool _cursorLocked = true;
    [SerializeField] private ResetBt _reseter;



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log(( _saveLoadManager.LoadSelectedCar()));
        selectedCarIndex = _saveLoadManager.LoadSelectedCar(); 
        InitializeCar();
        CheckPlatform();
        InitializeMode();
    }

  

    private void InitializeMode()
    {
        volume = GameObject.Find("Volume"); 
        
        if (_saveLoadManager.LoadFXStatus())
        {
            volume.SetActive(true);
        }
        else
        {
            volume.SetActive(false);
        }
        
        loadLevelUI.SetActive(true);
        if (type == LevelType.Zombie)
        {
            _saveLoadManager.ResetKilledZombiesCount();
            hpBar = GameObject.Find("Health").GetComponent<Slider>();
        }
    }

    private void CheckPlatform()
    {
        if(Application.isMobilePlatform)
        {
            mobileUI.SetActive(true);
        }
        else
        {
            mobileUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Переключаем состояние курсора
            _cursorLocked = !_cursorLocked;

            // Обновляем состояние курсора
            if (_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void InitializeCar()
    {
      Debug.Log(_saveLoadManager);
        if (!_saveLoadManager.IsCarPurchased(selectedCarIndex))
        {
            Debug.Log("Выбрана машина:" + selectedCarIndex);
            selectedCarIndex = 0; // Если сохраненная машина не куплена, выбираем первую по умолчанию
        }
        
        cars[selectedCarIndex].SetActive(true);
        if (_isRace)
        {
            var currentCar = FindObjectOfType<PrometeoCarController>();
            var cm = currentCar.transform.Find("cm");
            carCamera.LookAt = cm.transform;
            carCamera.Follow = cm.transform;
        }
        else
        {
            carCamera.Follow = cars[selectedCarIndex].transform;
        }

        _reseter.InitializeReseter();
    }

    public void MinusHp()
    {
        mainAudioSource.PlayOneShot(heatClip);
        hpBar.value -= 10;
        DeathChecker();
    }

    private void DeathChecker()
    {
        if (hpBar.value <= 0)
        {
            var particle =  cars[selectedCarIndex].transform.Find("VFX_Fire").GetComponent<ParticleSystem>();
            particle.Play();
            mainAudioSource.PlayOneShot(explosionClip);
            loseBar.SetActive(true);
        }
    }

    public void ResetChanseScene()
    {
        var particle =  cars[selectedCarIndex].transform.Find("VFX_Fire").GetComponent<ParticleSystem>();
        particle.Stop();
        hpBar.value = 100;
    }
    
    // Метод для удаления всех объектов с указанными тегами
    private void DestroyObjectsByTag(string tag)
    {
        carEngine.enabled = false;
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag); // Находим все объекты с указанным тегом
        
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj); // Удаляем объект
        }
    }
}
