using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelsOpenerChanger : MonoBehaviour
{
    private int score;
    [SerializeField] private GameObject openedLevelTwo;
    [SerializeField] private GameObject closedLevelTwo;
    [SerializeField] private GameObject openedLevelThird;
    [SerializeField] private GameObject closedLevelThird;

    private void Start()
    {
       // score = YandexGame.savesData.record;

       if(score >= 5000 && score < 10000)
        {
            closedLevelTwo.SetActive(false);
            openedLevelTwo.SetActive(true);
        }
       else if(score >= 10000)
        {
            closedLevelTwo.SetActive(false);
            openedLevelTwo.SetActive(true);
            closedLevelThird.SetActive(false);
            openedLevelThird.SetActive(true);
        }
            
    }
}
