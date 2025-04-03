using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetBt : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button[] _toMenuButtons;
    [SerializeField] private Button[] _toResetButtons;
    [SerializeField] private Button[] _getChanseButton;
    [Header("UI")]
    [SerializeField] private GameObject _loadermenuUI;
    [SerializeField] private GameObject _reseterUi;
    
    
    private int level;
    private void Start()
    {
        if (_toMenuButtons != null)
        {
            foreach (Button menuButton in _toMenuButtons)
            {
                menuButton.onClick.AddListener(StartMenuON);
            }
        }

        if (_toResetButtons != null)
        {
            foreach (Button resetButton in _toResetButtons)
            {
                resetButton.onClick.AddListener(StartResetScene);
            }
        }
        level = SceneManager.GetActiveScene().buildIndex;
    }
    public void UpperCar()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = (transform.position - player.transform.position).normalized;
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * 200000f);
    }

    public void StartMenuON()
    {
        _loadermenuUI.SetActive(true);
    }

    public void StartResetScene()
    {
        _reseterUi.SetActive(true);
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
