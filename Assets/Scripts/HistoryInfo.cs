using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HistoryInfo
{
    public bool IsClear;
    public int totalScore;

    public HistoryInfo(bool clear, int score)
    {
        IsClear = clear;
        totalScore = score;
    }
}
