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
    private const int maxTotalZombies = 20; // Максимальное количество зомби за все время
    
    private DiContainer _container;

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }

    void Start()
    {
        // Запускаем корутину спавна
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        while (totalZombiesSpawned < maxTotalZombies)
        {
            if (currentZombieCount < maxZombiesAtOnce)
            {
             
                SpawnZombie();
                yield return new WaitForSeconds(spawnInterval); // Ждем перед следующим спавном
            }
            else
            {

                yield return null; // Ждем один кадр, если достигнуто максимальное количество зомби
            }
        }
    }

    private void SpawnZombie()
    {
        GameObject randomZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    
        Debug.Log(_container);
        GameObject newZombie = _container.InstantiatePrefab(randomZombiePrefab, spawnPoint);
    
        currentZombieCount++;
        totalZombiesSpawned++;

      
    
        ZombieLogic zombieLogic = newZombie.GetComponent<ZombieLogic>();
        if (zombieLogic != null)
        {
            zombieLogic.OnZombieDestroyed += HandleZombieDestroyed; // Подписка на событие

        }
    }

    private void HandleZombieDestroyed()
    {
        Debug.Log("Зомби уничтожен. Текущее количество: " + currentZombieCount);
        currentZombieCount--;
    }
}
