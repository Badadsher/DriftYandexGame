using System;
using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static bool isInitialized { get; private set;}
    public event Action onInitialized;
    
    private void Awake()
    {
        StartCoroutine(Initialize());
    }
    
    private void Start()
    {
        MirraSDK.Analytics.GameIsReady();
    }
    
    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(1f);
        isInitialized = true;
        onInitialized?.Invoke();

    }
}
