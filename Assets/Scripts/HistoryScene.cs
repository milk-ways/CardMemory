using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class HistoryScene : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private HistoryBase[] historyBases;
    public Sprite[] ClearImage;

    public void Init()
    {
        bestScoreText.text = "Best Score : " + PlayerPrefs.GetInt("BestScore").ToString();

        SetHistoryBases();
    }

    private void SetHistoryBases()
    {
        int cnt = 0;

        var history = Manager.History.OrderByDescending(x => x.Key);
        foreach (KeyValuePair<string, HistoryInfo> historyInfo in history)
        {
            if (cnt >= historyBases.Length)
                break;

            historyBases[cnt].Init(historyInfo);
            cnt++;
        }
    }
}
