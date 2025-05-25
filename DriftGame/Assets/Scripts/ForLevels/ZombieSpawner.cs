using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZombieSpawner : MonoBehaviour
{
    
    public GameObject[] zombiePrefabs; // Массив префабов зомби
    public Transform[] spawnPoints; // Позиции для спавна (4 края карты)
    public float spawnInterval = 5f; // Интервал спавна
    private int currentZombieCount = 0; // Текущее количество заспавненных зомби
    private int totalZombiesSpawned = 0; // Общее количество заспавненных зомби
    private const int maxZombiesAtOnce = 4; // Максимальное количество зомби одновременно
    [SerializeField]  private int maxTotalZombies; // Максимальное количество зомби за все время
    [SerializeField] private GameObject winBar;
    private bool allZombiesSpawned = false;
    [SerializeField] private SaveLoadManagerWrapper _saveLoadManager;


    void Start()
    {
        // Запускаем корутину спавна
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (totalZombiesSpawned < maxTotalZombies)
        {
            if (currentZombieCount <= maxZombiesAtOnce)
            {
             
                SpawnZombie();
                yield return new WaitForSeconds(spawnInterval); // Ждем перед следующим спавном
            }
            else
            {
                yield return null; // Ждем один кадр, если достигнуто максимальное количество зомби
  
            }
        }
        allZombiesSpawned = true;
        CheckWinCondition();
 
    }
    private void CheckWinCondition()
    {
        Debug.Log("ended");
        // Победа только если все зомби заспавнены И все убиты
        if (allZombiesSpawned && currentZombieCount <= 0)
        {
            Debug.Log("onner");
            winBar.SetActive(true);
            _saveLoadManager.SetZombieCompleteStatus();
        }
    }
    

    private void SpawnZombie()
    {
        GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    

        GameObject newZombie = Instantiate(randomZombiePrefab, spawnPoint);
    
        currentZombieCount++;
        totalZombiesSpawned++;

      
    
        ZombieLogic zombieLogic = newZombie.GetComponent<ZombieLogic>();
        if (zombieLogic != null)
        {
            zombieLogic.OnZombieDestroyed += HandleZombieDestroyed; // Подписка на событие
            zombieLogic.OnZombieDestroyed += CheckWinCondition;

        }
    }

    private void HandleZombieDestroyed()
    {
        Debug.Log("Зомби уничтожен. Текущее количество: " + currentZombieCount);
        currentZombieCount--;
    }
}
