using System.Collections;
using System.Collections.Generic;
using romanlee17.MirraGames;
using romanlee17.MirraGames.Interfaces;
using TMPro;
using UnityEngine;

public class LocalisationMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI challengeText;
    [SerializeField] private TextMeshProUGUI zombieText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private TextMeshProUGUI nextUpdate;
    [SerializeField] private TextMeshProUGUI arcadeText;
    [SerializeField] private TextMeshProUGUI driftText;
    [SerializeField] private TextMeshProUGUI lockedDrift;
    
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI settingsText;
    [SerializeField] private TextMeshProUGUI garageText;
    
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private TextMeshProUGUI graphicsText;
    
    [SerializeField] private TextMeshProUGUI moneyText;
    
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private TextMeshProUGUI summerText;
    [SerializeField] private TextMeshProUGUI winterText;
    [SerializeField] private TextMeshProUGUI lockLocationText;
    void Start()
    {
        LanguageType  languageType = MirraSDK.Language.Current;
        Debug.Log(languageType);
        
       if (languageType == LanguageType.Russian)
       {
           arcadeText.text = "АРКАДНЫЙ РЕЖИМ";
           driftText.text = "ВЕСЕЛЫЙ ДРИФТ";
           lockedDrift.text = "РАЗБЛОКИРУЕТСЯ ПОСЛЕ ПРОХОЖДЕНИЯ ЗОМБИ РЕЖИМА";
           
           locationText.text = "ВЫБЕРИТЕ ЛОКАЦИЮ";
           summerText.text = "ЛЕТНИЙ ДЕНЬ";
           winterText.text = "ЗИМНИЙ СЛУЧАЙ";
           lockLocationText.text = "РАЗБЛОКИРУЕТСЯ ПОСЛЕ РЕКОРДА В 10000 ОЧКОВ ДРИФТА";
    
    
            challengeText.text = "ЧЕЛЛЕНДЖ РЕЖИМ";
            zombieText.text = "ЗОМБИ АПОКАЛИПСИС";

            blockText.text = "РАЗБЛОКИРУЕТСЯ ПОСЛЕ ПРОХОЖДЕНИЯ ЗОМБИ РЕЖИМА";
            nextUpdate.text = "В СЛЕДУЮЩИХ ОБНОВЛЕНИЯХ";

            mainText.text = "ОСНОВНАЯ";
            settingsText.text = "НАСТРОЙКИ";
            garageText.text = "ГАРАЖ";

            volumeText.text = "Уровень громкости";
            graphicsText.text = "РЕЖИМ КАЧЕСТВЕННОЙ ГРАФИКИ";

            moneyText.text = "ПОЛУЧИТЬ +500 МОНЕТ";
        }
        else if (languageType == LanguageType.Turkish)
        {
            arcadeText.text = "ARCAD MODU";
            driftText.text = "EĞLENCELİ DRİFT";
            lockedDrift.text = "ZOMBI MODUNU TAMAMLADIKTAN SONRA AÇILIR";
            
            locationText.text = "BİR KONUM SEÇİN";
            summerText.text = "YAZ GÜNÜ";
            winterText.text = "KIŞ VAKASI";
            lockLocationText.text = "10.000 SÜRÜKLENME PUANI KAYDINDAN SONRA KİLİDİ AÇILIR";

            
            challengeText.text = "MEYDAN OKUMA MODU";
            zombieText.text = "ZOMBI KIYAMETİ";

            blockText.text = "ZOMBI MODUNU TAMAMLADIKTAN SONRA AÇILIR";
            nextUpdate.text = "SONRAKİ GÜNCELLEMELERDE";

            mainText.text = "ANA MENÜ";
            settingsText.text = "AYARLAR";
            garageText.text = "GARAJ";

            volumeText.text = "Ses seviyesi";
            graphicsText.text = "YÜKSEK KALİTELİ GRAFİK MODU";

            moneyText.text = "+500 JETON KAZAN";
        }
        else
        {
            arcadeText.text = "ARCADE MODE";
            driftText.text = "FUN DRIFT";
            lockedDrift.text = "UNLOCKED AFTER COMPLETING ZOMBIE MODE";
            
            locationText.text = "choose a location";
            summerText.text = "SUMMER DAY";
            winterText.text = "WINTER CASE";
            lockLocationText.text = "UNLOCKS AFTER SETTING A RECORD OF 10,000 DRIFT POINTS";
            
            challengeText.text = "CHALLENGE MODE";
            zombieText.text = "ZOMBIE APOCALYPSE";

            blockText.text = "UNLOCKED AFTER COMPLETING ZOMBIE MODE";
            nextUpdate.text = "IN NEXT UPDATES";

            mainText.text = "MAIN";
            settingsText.text = "SETTINGS";
            garageText.text = "GARAGE";

            volumeText.text = "Volume level";
            graphicsText.text = "HIGH QUALITY GRAPHICS MODE";

            moneyText.text = "GET +500 COINS";
        }
    }
}
