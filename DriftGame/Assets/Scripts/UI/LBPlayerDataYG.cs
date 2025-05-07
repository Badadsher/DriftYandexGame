using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LBPlayerDataYG : MonoBehaviour
{
    public ImageLoadYG imageLoad;

    [Serializable]
    public struct TextLegasy
    {
        public TMP_Text rank, name, score;
    }
    public TextLegasy textLegasy;

    [Space(10)]
    public MonoBehaviour[] topPlayerActivityComponents = new MonoBehaviour[0];
    public MonoBehaviour[] currentPlayerActivityComponents = new MonoBehaviour[0];

    public class Data
    {
        public string rank;
        public string name;
        public string score;
        public string photoUrl;
        public bool inTop;
        public bool currentPlayer;
        public Sprite photoSprite;
    }

    [HideInInspector]
    public Data data = new Data();

    public void UpdateEntries()
    {
        if (textLegasy.rank && data.rank != null) textLegasy.rank.text = data.rank.ToString();
        if (textLegasy.name && data.name != null) textLegasy.name.text = data.name;
        if (textLegasy.score && data.score != null) textLegasy.score.text = data.score.ToString();

        if (imageLoad)
        {
            if (data.photoSprite)
            {
                imageLoad.SetTexture(data.photoSprite.texture);
            }
            else if (data.photoUrl == null)
            {
                imageLoad.ClearTexture();
            }
            else
            {
                imageLoad.Load(data.photoUrl);
            }
        }

        if (topPlayerActivityComponents.Length > 0)
        {
            if (data.inTop)
            {
                ActivityMomoObjects(topPlayerActivityComponents, true);
            }
            else
            {
                ActivityMomoObjects(topPlayerActivityComponents, true);
            }
        }

        if (currentPlayerActivityComponents.Length > 0)
        {
            if (data.currentPlayer)
            {
                ActivityMomoObjects(currentPlayerActivityComponents, true);
            }
            else
            {
                ActivityMomoObjects(currentPlayerActivityComponents, false);
            }
        }

        void ActivityMomoObjects(MonoBehaviour[] objects, bool activity)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].enabled = activity;
            }
        }
    }
}