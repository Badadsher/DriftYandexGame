using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Zenject;

public class LeaderboardYG : MonoBehaviour
{
    public string nameLB;
    public int maxQuantityPlayers = 20;
    public int quantityTop = 3;
    public int quantityAround = 6;

    public enum UpdateLBMethod { Start, OnEnable, DoNotUpdate };
    public UpdateLBMethod updateLBMethod = UpdateLBMethod.OnEnable;
    public Text entriesText;
    public bool advanced;
    public Transform rootSpawnPlayersData;
    public GameObject playerDataPrefab;

    public enum PlayerPhoto { NonePhoto, Small, Medium, Large };
    public PlayerPhoto playerPhoto = PlayerPhoto.Small;
    public Sprite isHiddenPlayerPhoto;
    public bool timeTypeConvert;
    public int decimalSize = 1;

    public UnityEvent onUpdateData;
    private SaveLoadManager _saveLoadManager;

    private LBPlayerDataYG[] players;

    [Inject]
    private void Construct(SaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager; 
    }

    private void OnEnable()
    {
        _saveLoadManager.OnGetLeaderboadData += OnUpdateLB;

        if (updateLBMethod == UpdateLBMethod.OnEnable)
            UpdateLB();
    }
    private void OnDisable()
    {
        _saveLoadManager.OnGetLeaderboadData -= OnUpdateLB;
    }

    private void Start()
    {
        if (updateLBMethod == UpdateLBMethod.Start)
            UpdateLB();
    }

    private void OnUpdateLB(LeaderboardData lbData)
    {
        Debug.Log($"ON UPDATE LB: {lbData.Name}");
        if (lbData.Name != nameLB)
            return;
        Debug.Log("ON UPDATE LB2");
        DestroyLBList();

        if (lbData.PlayerData.Length > maxQuantityPlayers)
        {
            Array.Resize(ref lbData.PlayerData, maxQuantityPlayers);
        }
        SpawnPlayersList(lbData.PlayerData);
        onUpdateData?.Invoke();
    }

    private void DestroyLBList()
    {
        int childCount = rootSpawnPlayersData.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(rootSpawnPlayersData.GetChild(i).gameObject);
        }
    }

    private void SpawnPlayersList(LeaderboardPlayerData[] lb)
    {
        Debug.Log("WTF SPAWN");
        Debug.Log(lb.Length);
        players = new LBPlayerDataYG[lb.Length];

        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerObj = Instantiate(playerDataPrefab, rootSpawnPlayersData);

            players[i] = playerObj.GetComponent<LBPlayerDataYG>();

            int rank = lb[i].Position;

            players[i].data.name = lb[i].Name;
            players[i].data.rank = rank.ToString();

            if (rank <= quantityTop)
            {
                players[i].data.inTop = true;
            }
            else
            {
                players[i].data.inTop = false;
            }
            
            players[i].data.currentPlayer = false;
            Debug.Log($"Player score: {lb[i].Score.ToString()}");
            players[i].data.score = lb[i].Score.ToString();

            if (playerPhoto != PlayerPhoto.NonePhoto)
            {
                if (isHiddenPlayerPhoto && lb[i].ImgUrl.Contains("/avatar/0/"))
                {
                    players[i].data.photoSprite = isHiddenPlayerPhoto;
                }
                else
                {
                    players[i].data.photoUrl = lb[i].ImgUrl;
                }
            }

            players[i].UpdateEntries();
        }
    }

    public void UpdateLB()
    {
        Debug.Log("UPDATE LB");
        string photoSize = "nonePhoto";

        switch (playerPhoto)
        {
            case PlayerPhoto.Small:
                photoSize = "small";
                break;
            case PlayerPhoto.Medium:
                photoSize = "medium";
                break;
            case PlayerPhoto.Large:
                photoSize = "large";
                break;
        }

        _saveLoadManager.GetLeaderboard(nameLB, quantityTop, quantityAround);
    }

    public void SetLeaderboard(int score) => _saveLoadManager.SetLeaderboard(nameLB, score);
}
