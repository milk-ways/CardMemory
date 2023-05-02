using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplayLog
{
    public List<StartInfo> startInfos = new List<StartInfo>();

    public List<TouchInfo> touchInfos = new List<TouchInfo>();

    [System.Serializable]
    public class StartInfo
    {
        public List<int> cards;

        public StartInfo(List<int> cards)
        {
            this.cards = cards;
        }
    }

    [System.Serializable]
    public class TouchInfo
    {
        public float time;
        public int cardIndex;

        public TouchInfo(float time, int cardIndex)
        {
            this.time = time;
            this.cardIndex = cardIndex;
        }
    }
}
