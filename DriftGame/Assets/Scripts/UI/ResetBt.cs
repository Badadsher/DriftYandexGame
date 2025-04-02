using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetBt : MonoBehaviour
{
    [SerializeField] private GameObject loadermenuUI;
    [SerializeField] private GameObject reseterUi;
    private int level;
    private void Start()
    {
        level = SceneManager.GetActiveScene().buildIndex;
    }
    public void UpperCar()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = (transform.position - player.transform.position).normalized;

        // Применяем силу отталкивания к машинке
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * 200000f);
       
    }

    public void StartMenuON()
    {
        loadermenuUI.SetActive(true);
    }

    public void StartResetScene()
    {
        reseterUi.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(level);
    }
    public void MenuON()
    {
        SceneManager.LoadScene(0);
    }
}
