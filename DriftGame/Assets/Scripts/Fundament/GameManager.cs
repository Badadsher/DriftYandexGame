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
    [SerializeField] private GameObject loseBar;
    [SerializeField] private GameObject loadLevelUI;
    [SerializeField] private GameObject mobileUI;
    [SerializeField] private GameObject carUI;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject trainingUI;
    [SerializeField] private GameObject eeButton;
    [SerializeField] private bool _isRace;
    
    [Header("Audio")]
    [SerializeField] private AudioSource mainAudioSource;
    
    [Header("For Car")]
    [SerializeField] private AudioClip heatClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private GameObject[] cars;
    private int selectedCarIndex;
    private bool isCar;
    [SerializeField] private CinemachineVirtualCamera carCamera;

    [Header("For Character")]
    [SerializeField] private bool initCharacter;
    [SerializeField] private GameObject characterObj;
    [SerializeField] private Vector3 exitOffset;
    [SerializeField] private float nearDistance;

    [Header("CarSounds")]
    [SerializeField] private AudioSource carEngine;
    private GameObject volume;
    private SaveLoadManager _saveLoadManager;
    private bool _cursorLocked = true;
    
    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }

    private void LateUpdate()
    {
        if (!Application.isMobilePlatform && Input.GetKeyDown(KeyCode.E))
        {
            ChangePlayer(0);
        }
        else
        {
            eeButton.SetActive(isCar || isCharNear2Car());
        }
    }

    void Start()
    {
        selectedCarIndex = _saveLoadManager.LoadSelectedCar(); 
        InitializeCar();
        CheckPlatform();
        InitializeMode();

        ChangePlayer(initCharacter ? 3 : 2);
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
            trainingUI.SetActive(false);
        }
        else
        {
            trainingUI.SetActive(true);
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
            StartCoroutine(CleanScene());
        }
    }

    private IEnumerator CleanScene()
    {
        yield return new WaitForSeconds(2.5f);
        DestroyObjectsByTag("Zombie"); // Удаляем всех зомби
        DestroyObjectsByTag("Player"); // Удаляем игрока
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
    /// <summary>
    /// 0 - Swap, 1 - To Character, 2 - To Car, 3 - Initialize Character
    /// </summary>
    /// <param name="optionID"></param>
    public void ChangePlayer(int optionID) 
    {
        if (optionID != 0)
        {
            bool toCar = optionID % 2 == 0;

            carUI.SetActive(toCar);
            cars[selectedCarIndex].GetComponent<PrometeoCarController>().enabled = toCar;
            characterUI.SetActive(!toCar);
            characterObj.SetActive(!toCar);
            carCamera.Follow = toCar ? cars[selectedCarIndex].transform : characterObj.transform;

            characterObj.transform.position = optionID < 3 ? cars[selectedCarIndex].transform.position + exitOffset : characterObj.transform.position;

            cars[selectedCarIndex].GetComponent<Player>().isTarget = toCar;
            characterObj.GetComponent<Player>().isTarget = !toCar;

            isCar = toCar;
        }
        else if (optionID == 0 && (isCar || isCharNear2Car()))
        {
            ChangePlayer(isCar ? 1 : 2);
        }
    }

    private bool isCharNear2Car()
    {
        float distance = Vector3.Distance(cars[selectedCarIndex].transform.position, characterObj.transform.position);
        return distance < nearDistance;
    }
}
