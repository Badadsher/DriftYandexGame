using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageLogic : MonoBehaviour
{
     [Header("Toggle Settings")]
    [SerializeField] private Toggle[] toggles; // Массив переключателей 
    [Header("Colors")]
    [SerializeField] private Color activeColor; // Цвет активного переключателя
    [SerializeField] private Color inactiveColor ; // Цвет неактивных переключателей
    [Header("Cars")]
    [SerializeField] private GameObject[] carList;
   
    void Start()
    {
        // Устанавливаем обработчики событий для каждого переключателя
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleChanged(toggle); });
        }
    }

    private void OnToggleChanged(Toggle changedToggle)
    {
        // Если переключатель включен, отключаем остальные
        if (changedToggle.isOn)
        {
            foreach (var toggle in toggles)
            {
                if (toggle != changedToggle)
                {
                    toggle.isOn = false; // Отключаем другие переключатели
                    SetToggleAppearance(toggle, false); // Меняем внешний вид
                }
            }

            // Меняем цвет и текст у нажатого переключателя
            SetToggleAppearance(changedToggle, true);
            // Выполняем уникальную логику для каждого переключателя
            ExecuteToggleLogic(changedToggle);
        }
    }

    private void SetToggleAppearance(Toggle toggle, bool isActive)
    {
        Image toggleImage = toggle.GetComponent<Image>();

        if (toggleImage != null)
        {
            toggleImage.color = isActive ? activeColor : inactiveColor; // Устанавливаем цвет
        }
    }
    private void ExecuteToggleLogic(Toggle activeToggle)
    {
        if (activeToggle == toggles[0])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            carList[0].SetActive(true);
        }
        else if (activeToggle == toggles[1])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            carList[1].SetActive(true);
        }
        else if (activeToggle == toggles[2])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            carList[2].SetActive(true);
        }
        if (activeToggle == toggles[3])
        {
            foreach (var gameobject in carList)
            {
                gameobject.SetActive(false);
            }
            carList[3].SetActive(true);
        }
    }
}
