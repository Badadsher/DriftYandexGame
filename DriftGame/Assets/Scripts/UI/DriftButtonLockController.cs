using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DriftButtonLockController : MonoBehaviour
{
    [SerializeField] private GameObject targetImage;
    [SerializeField] private GameObject lockImage;

    private SaveLoadManager _saveLoadManager;
    private const int RequiredDriftScore = 1000;

    [Inject]
    public void Initialize(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }

    private void Start()
    {
        UpdateImageState();
    }

    private void UpdateImageState()
    {
        int driftScore = _saveLoadManager.GetScoreDrift();
        
        targetImage.SetActive(true);
        
        Image image = targetImage.GetComponent<Image>();

        if (driftScore >= RequiredDriftScore)
        {
            lockImage.SetActive(false);

            if (image != null)
                image.raycastTarget = true;
        }
        else
        {
            lockImage.SetActive(true);

            if (image != null)
                image.raycastTarget = false;
        }
    }
}