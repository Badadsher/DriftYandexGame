using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using romanlee17.MirraGames.Interfaces;
using TMPro;
using UnityEngine;

public class LocalisationZombie : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wasd;
    [SerializeField] private TextMeshProUGUI drift;
    [SerializeField] private TextMeshProUGUI reset;
    [SerializeField] private TextMeshProUGUI menu;
    [SerializeField] private TextMeshProUGUI tab;
    
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI killed;
    
    [SerializeField] private TextMeshProUGUI carKilled;
    [SerializeField] private TextMeshProUGUI chanse;
    [SerializeField] private TextMeshProUGUI startAgain;
    [SerializeField] private TextMeshProUGUI exitMenu;
    
    [SerializeField] private TextMeshProUGUI winMenu;
    [SerializeField] private TextMeshProUGUI winAgain;
    [SerializeField] private TextMeshProUGUI winExit;
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
            
            health.text = "ЗДОРОВЬЕ";
            killed.text = "УНИЧТОЖЕНО ЗОМБИ";
            
            carKilled.text = "ЗОМБИ УНИЧТОЖИЛИ МАШИНУ";
            chanse.text = "ПОЛУЧИТЬ ШАНС";
            startAgain.text = "НАЧАТЬ ЗАНОВО";
            exitMenu.text = "ВЫЙТИ В МЕНЮ";
            winMenu.text = "ЗОМБИ РЕЖИМ ПРОЙДЕН";
            winAgain.text = "НАЧАТЬ ЗАНОВО";
            winExit.text = "ВЫЙТИ В МЕНЮ";
        }
        else if (languageType == LanguageType.Turkish)
        {
            wasd.text = "KONTROLLER";
            drift.text = "DRİFT";
            reset.text = "YENİDEN BAŞLAT";
            menu.text = "MENÜYE DÖN";
            tab.text = "İMLEÇİ GÖSTER/GİZLE";
    
            health.text = "CAN";
            killed.text = "ÖLDÜRÜLEN ZOMBİ";
    
            carKilled.text = "ZOMBİLER ARABAYI YOK ETTİ";
            chanse.text = "ŞANS KAZAN";
            startAgain.text = "YENİDEN BAŞLA";
            exitMenu.text = "MENÜYE ÇIK";
            winMenu.text = "ZOMBİ MODU TAMAMLANDI";
            winAgain.text = "YENİDEN BAŞLA";
            winExit.text = "MENÜYE ÇIK";
        }
        else // English (Default)
        {
            wasd.text = "CONTROLS";
            drift.text = "DRIFT";
            reset.text = "RESET";
            menu.text = "TO MENU";
            tab.text = "TOGGLE CURSOR";
    
            health.text = "HEALTH";
            killed.text = "ZOMBIES KILLED";
    
            carKilled.text = "ZOMBIES DESTROYED THE CAR";
            chanse.text = "GET A CHANCE";
            startAgain.text = "START AGAIN";
            exitMenu.text = "EXIT TO MENU";
            winMenu.text = "ZOMBIE MODE COMPLETED";
            winAgain.text = "START AGAIN";
            winExit.text = "EXIT TO MENU";
        }
    }
    
    
}
