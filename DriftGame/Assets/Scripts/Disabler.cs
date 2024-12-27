using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
public class Disabler : MonoBehaviour
{
    [SerializeField] private GameObject warnningAD;
    public void Disable()
    {
        warnningAD.SetActive(false);
    }
}
