using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HistoryBase : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI DateText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private Image ClearImage;
    [SerializeField]
    private TextMeshProUGUI ClearText;
    [SerializeField]
    private Button replayButton;

    public void Init(KeyValuePair<string, HistoryInfo> historyInfo)
    {
        gameObject.SetActive(true);

        DateText.text = historyInfo.Key.Substring(0,16);
        ScoreText.text = historyInfo.Value.totalScore.ToString();

        ClearImage.sprite = historyInfo.Value.IsClear ? Manager.Instance.HistoryScene.ClearImage[0] : Manager.Instance.HistoryScene.ClearImage[1];
        ClearText.text = historyInfo.Value.IsClear ? "Clear" : "Fail";

        void LogReplay()
        {
            var replay = Manager.ReplayLog[historyInfo.Key];
            Manager.Instance.InitCardScene(replay);
        }

        replayButton.onClick.AddListener(() => LogReplay());
        replayButton.onClick.AddListener(() => Manager.Instance.ButtonClickSound());
    }
}
