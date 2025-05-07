using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriftScoreUI : MonoBehaviour
{
    private TextMeshProUGUI driftScoreText;

    void Awake()
    {
        // Получаем компонент Text с объекта, к которому прикреплен скрипт
        driftScoreText = GetComponent<TextMeshProUGUI>();
        if (driftScoreText == null)
        {
            Debug.LogError("DriftScoreUI: Text component not found on this GameObject!");
        }
    }

    void Start()
    {
        // Инициализируем текст при старте
        UpdateScore(0);
    }

    // Публичный метод для обновления текста очков
    public void UpdateScore(float score)
    {
        
        if (driftScoreText != null)
        {
            driftScoreText.text = "Очки дрифта: " + Mathf.RoundToInt(score).ToString();
        }
        else
        {
            Debug.Log("!isupdated");
        }
    }
}