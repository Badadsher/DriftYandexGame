using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Linq;
using UnityEditor.Rendering;
using Zenject;
using romanlee17.MirraGames;

public class TimerBeforeAdsYG : MonoBehaviour
{
    [SerializeField]
    private GameObject secondsPanelObject;

    [SerializeField]
    private GameObject[] secondObjects;

    [Space(20)]
    [SerializeField] public UnityEvent onShowTimer;
    [SerializeField] private UnityEvent onHideTimer;

    [SerializeField] private float checkDelay = 60;
    private SaveLoadManager _saveLoadManager;

    [SerializeField] private bool _isCrazy = true;

    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }

    private int objSecCounter;

    private void Start()
    {
        if (_isCrazy)
        {
            Destroy(gameObject);
        }
        if (secondsPanelObject)
            secondsPanelObject.SetActive(false);

        for (int i = 0; i < secondObjects.Length; i++)
            secondObjects[i].SetActive(false);

        if (secondObjects.Length > 0)
            StartCoroutine(CheckTimerAd());
        else
            Debug.LogError("Fill in the array 'secondObjects'");
    }

    IEnumerator CheckTimerAd()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkDelay);
            if (!_saveLoadManager.nowInterAdv)
            {

                onShowTimer?.Invoke();
                objSecCounter = 0;
                if (secondsPanelObject)
                    secondsPanelObject.SetActive(true);

                _saveLoadManager.PauseGame(true);
                StartCoroutine(TimerAdShow());
                yield break;
            }
        }
    }

    IEnumerator TimerAdShow()
    {
        while (true)
        {
            if (objSecCounter < secondObjects.Length)
            {
                for (int i2 = 0; i2 < secondObjects.Length; i2++)
                    secondObjects[i2].SetActive(false);

                secondObjects[objSecCounter].SetActive(true);
                objSecCounter++;

                yield return new WaitForSecondsRealtime(1.0f);
            }

            if (objSecCounter == secondObjects.Length)
            {
                _saveLoadManager.FullscreenAdvShow();
                StartCoroutine(BackupTimerClosure());

                while (!_saveLoadManager.nowInterAdv)
                    yield return null;

                Restart();
                yield break;
            }
        }
    }

    IEnumerator BackupTimerClosure()
    {
        yield return new WaitForSecondsRealtime(2f);

        if (objSecCounter != 0)
        {
            Restart();
            _saveLoadManager.PauseGame(false);
        }
    }

    private void Restart()
    {
        secondsPanelObject.SetActive(false);
        onHideTimer?.Invoke();
        objSecCounter = 0;
        StartCoroutine(CheckTimerAd());
    }
}