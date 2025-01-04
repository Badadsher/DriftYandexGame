using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public LevelType type { get; private set; }
    private Slider hpBar;
    [SerializeField] private GameObject loseBar;
    [SerializeField] private GameObject loadLevelUI;
    void Start()
    {
        loadLevelUI.SetActive(true);
        if (type == LevelType.Zombie)
        {
         SaveManager.ResetKilledZombiesCount();
         hpBar = GameObject.Find("Health").GetComponent<Slider>();
        }
    }

    public void MinusHp()
    {
        hpBar.value -= 10;
        DeathChecker();
    }

    private void DeathChecker()
    {
        if (hpBar.value <= 0)
        {
            loseBar.SetActive(true);
        }
        else
        {
            return;
        }
    }
}
