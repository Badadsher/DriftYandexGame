using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public LevelType type { get; private set; }
    private Slider hpBar;
    [SerializeField] private GameObject loseBar;
    [SerializeField] private GameObject loadLevelUI;
    private GameObject volume;
    void Start()
    {
        volume = GameObject.Find("Volume");
        Debug.Log(YG2.saves._volumeStatus);
      //  if (YG2.saves._volumeStatus)
       // {
       //     volume.SetActive(true);
      //  }
      //  else
      //  {
      //     volume.SetActive(false);
      //  }
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
