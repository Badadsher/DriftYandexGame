using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public LevelType type { get; private set; }
    void Start()
    {
        if (type == LevelType.Zombie)
        {
         SaveManager.ResetKilledZombiesCount();
        
        }
    }
}
