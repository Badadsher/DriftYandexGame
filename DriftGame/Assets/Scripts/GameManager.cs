using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private AudioClip heatClip;
    [SerializeField] private AudioClip explosionClip;

    [Header("CarSounds")]
    [SerializeField] private AudioSource carEngine;
    private GameObject volume;
    void Start()
    {
        
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
