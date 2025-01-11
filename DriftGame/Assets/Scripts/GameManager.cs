using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public LevelType type { get; private set; }
    private Slider hpBar;
    [SerializeField] private GameObject loseBar;
    [SerializeField] private GameObject loadLevelUI;
    [SerializeField] private GameObject mobileUI;
    
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
    private bool _cursorLocked = true;
    void Start()
    {
        selectedCarIndex = SaveManager.LoadSelectedCar(); // Загружаем индекс выбранной машины
        InitializeCar();
        // Блокируем курсор при старте сцены
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if(Application.isMobilePlatform == true)
        {
             mobileUI.SetActive(true);
        }
        
        volume = GameObject.Find("Volume"); 
        Debug.Log(SaveManager.LoadFXStatus());
        if (SaveManager.LoadFXStatus())
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
         SaveManager.ResetKilledZombiesCount();
         hpBar = GameObject.Find("Health").GetComponent<Slider>();
        }
    }

    private void Update()
    {
        // Проверяем нажатие клавиши Tab
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
      
        if (!SaveManager.IsCarPurchased(selectedCarIndex))
        {
            selectedCarIndex = 0; // Если сохраненная машина не куплена, выбираем первую по умолчанию
        }
        
        cars[selectedCarIndex].SetActive(true);
        carCamera.Follow = cars[selectedCarIndex].transform;
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
}
