using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using romanlee17.MirraGames.Interfaces;
using TMPro;
using UnityEngine;

public class LocalisationDrift : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wasd;
    [SerializeField] private TextMeshProUGUI drift;
    [SerializeField] private TextMeshProUGUI reset;
    [SerializeField] private TextMeshProUGUI menu;
    [SerializeField] private TextMeshProUGUI tab;

    void Start()
    {
        LanguageType  languageType = MirraSDK.Language.Current;
        Debug.Log(languageType);
        
        if (languageType == LanguageType.Russian)
        {
            wasd.text = "УПРАВЛЕНИЕ";
            drift.text = "ДРИФТ";
            reset.text = "ПЕРЕЗАПУСК";
            menu.text = "В МЕНЮ";
            tab.text = "ВКЛЮЧИТЬ/СКРЫТЬ КУРСОР";
        }
        else if (languageType == LanguageType.Turkish)
        {
            wasd.text = "KONTROLLER";
            drift.text = "DRİFT";
            reset.text = "YENİDEN BAŞLAT";
            menu.text = "MENÜYE DÖN";
            tab.text = "İMLEÇİ GÖSTER/GİZLE";
        }
        else // English (Default)
        {
            wasd.text = "CONTROLS";
            drift.text = "DRIFT";
            reset.text = "RESET";
            menu.text = "TO MENU";
            tab.text = "TOGGLE CURSOR";
        }
    }

  
}
