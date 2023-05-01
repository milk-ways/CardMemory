using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HistoryScene : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    public void Init()
    {
        bestScoreText.text = "Best Score : " + PlayerPrefs.GetInt("BestScore").ToString();
    }
}
