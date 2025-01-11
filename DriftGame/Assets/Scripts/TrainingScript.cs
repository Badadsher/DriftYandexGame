using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScript : MonoBehaviour
{
    public void StartPause()
    {
        Time.timeScale = 0.0f;
    }

    public void StopPause()
    {
        Time.timeScale = 1.0f;
        var trainAnim = gameObject.GetComponent<Animator>();
        trainAnim.Play("CloseTrain");
        
    }
}
