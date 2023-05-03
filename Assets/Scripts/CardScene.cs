using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardScene : MonoBehaviour
{
    [SerializeField]
    private List<StageInfo> stageInfos = new List<StageInfo>();

    [SerializeField]
    private GridLayoutGroup RugPanel;
    [SerializeField]
    private GameObject card;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI stageText;
    [SerializeField]
    private Image timerImage;
    [SerializeField]
    private GameObject gamePlayUI;
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private TextMeshProUGUI gameOverScoreText;

    private List<Card> cards;
    private List<int> cardValues;

    private int totalScore;

    private int nowLevel;
    private int remainCards;

    private float maxTime;
    private float remainTime;
    private float lastClickTime;

    private bool IsGameDone;
    public bool IsCardMOrF;

    private Card firstCard, secondCard;

    private ReplayLog replayLog;
    private ReplayLog givenReplayLog;

    public int flipedCount
    {
        get
        {
            if (secondCard != null)
                return 2;
            else
            {
                if (firstCard != null)
                    return 1;
            }
            return 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (givenReplayLog == null)
                Manager.Instance.InitTitleScene();
            else
                Manager.Instance.InitHistoryScene();
        }

        if (IsGameDone)
            return;

        if (remainTime <= 0 && !IsCardMOrF)
        {
            if (givenReplayLog == null)
            {
                GameClear(false);
                Manager.Instance.PlayEffect("GameOver");
            }
            else
                GameClear();
        }

        remainTime -= Time.deltaTime;
        timerImage.fillAmount = remainTime / maxTime;
    }

    public void Init()
    {
        ResetAll();

        SetNormalStage();
    }

    public void Init(ReplayLog replayLog)
    {
        ResetAll();

        givenReplayLog = replayLog;

        StartCoroutine(Replay());
    }

    private void ResetAll()
    {
        //초기화 작업
        nowLevel = totalScore = 0;
        cards = new List<Card>();
        cardValues = new List<int>();
        gamePlayUI.SetActive(true);
        gameOverUI.SetActive(false);
        IsGameDone = false;
        replayLog = new ReplayLog();
        lastClickTime = 0;
        scoreText.text = "0";
        givenReplayLog = null;

        foreach (Transform card in RugPanel.transform)
        {
            Destroy(card.gameObject);
        }
    }

    private void SetStageCardAndInfo()
    {
        cardValues.Clear();
        RugPanel.constraintCount = stageInfos[nowLevel].Row;
        remainCards = stageInfos[nowLevel].Row * stageInfos[nowLevel].Col;
        stageText.text = $"Stage {nowLevel + 1}";

        int ToAddCard = remainCards - cards.Count;

        for (int i = 0; i < ToAddCard; i++)
        {
            cards.Add(Instantiate(card, RugPanel.transform, false).GetComponent<Card>());
        }

        maxTime = remainTime = stageInfos[nowLevel].ResetTime;
    }

    private void GetRandomCard()
    {
        for (int i = 0; i < remainCards / 2;)
        {
            int num = Random.Range(0, 52);
            if (cardValues.Contains(num))
            {
                continue;
            }
            else
            {
                cardValues.Add(num);
                cardValues.Add(num);
                i++;
            }
        }
    }

    private void SetCard(bool noReplay)
    {
        Manager.Instance.PlayEffect("Stage");
        List<int> cardMemory = new List<int>();
        for (int i = 0; i < cardValues.Count; i++)
        {
            var card = cards[i];
            card.Init(cardValues[i], i, noReplay);
            cardMemory.Add(cardValues[i]);
        }
        AddStartInfo(cardMemory);
    }

    private void GetReplayCard(List<int> cards)
    {
        foreach (var i in cards)
        {
            cardValues.Add(i);
        }
    }

    public void FlipCards(Card card)
    {
        if (flipedCount == 0)
        {
            firstCard = card;
            IsCardMOrF = false;
        }
        else if (flipedCount == 1)
        {
            secondCard = card;

            StartCoroutine(CheckCardMatches());
        }
    }

    private IEnumerator CheckCardMatches()
    {
        yield return new WaitForSeconds(.18f);
        if (firstCard.cardType == secondCard.cardType && firstCard.CardNum == secondCard.CardNum)
        {
            Manager.Instance.PlayEffect("Correct");

            yield return new WaitForSeconds(.75f);

            totalScore += stageInfos[nowLevel].MatchScore;
            scoreText.text = totalScore.ToString();

            firstCard.DeActivateCard();
            secondCard.DeActivateCard();

            remainCards -= 2;
            if (remainCards <= 0)
            {
                nowLevel++;
                lastClickTime = -1;
                if (givenReplayLog == null)
                {
                    if (nowLevel >= stageInfos.Count)
                    {
                        GameClear(true);
                        Manager.Instance.PlayEffect("GameClear");
                    }
                    else
                        SetNormalStage();
                }
                else
                {
                    if (nowLevel >= stageInfos.Count)
                        GameClear();
                    else
                        SetReplayStage();
                }
            }
        }
        else
        {
            Manager.Instance.PlayEffect("Wrong");

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(firstCard.FlipAnimation());
            StartCoroutine(secondCard.FlipAnimation());
        }

        ResetFlipedCard();
    }

    private void SetNormalStage()
    {
        ResetFlipedCard();
        SetStageCardAndInfo();
        GetRandomCard();
        Utils.Shuffle(cardValues);
        SetCard(true);
    }

    private void SetReplayStage()
    {
        ResetFlipedCard();
        SetStageCardAndInfo();
        GetReplayCard(givenReplayLog.startInfos[nowLevel].cards);
        SetCard(false);
    }

    private void GameClear()
    {
        IsGameDone = true;
        gamePlayUI.SetActive(false);
        gameOverUI.SetActive(true);
        gameOverScoreText.text = "Score : " + totalScore;
    }

    private void GameClear(bool isClear)
    {
        GameClear();

        if (totalScore > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", totalScore);

        string timeStr = System.DateTime.Now.ToString("yyyy/MM/dd HH:mmss");

        Manager.ReplayLog.Add(timeStr, replayLog);
        Manager.Instance.SaveReplayLog();

        HistoryInfo history = new HistoryInfo(isClear, totalScore);
        Manager.History.Add(timeStr, history);
        Manager.Instance.SaveHistory();
    }

    private void ResetFlipedCard()
    {
        firstCard = null;
        secondCard = null;
        IsCardMOrF = false;
    }

    private void AddStartInfo(List<int> temp)
    {
        replayLog.startInfos.Add(new ReplayLog.StartInfo(temp));
    }

    public void AddTouchInfo(int index)
    {
        float nowClickTime = maxTime - remainTime;
        replayLog.touchInfos.Add(new ReplayLog.TouchInfo(nowClickTime - lastClickTime, index));
        lastClickTime = nowClickTime;
    }

    private IEnumerator Replay()
    {
        SetReplayStage();

        foreach (var touchinfo in givenReplayLog.touchInfos)
        {
            float time = touchinfo.time;
            int index = touchinfo.cardIndex;

            yield return new WaitForSeconds(time);

            cards[index].FlipCard();
        }
    }

    public void BackButton()
    {
        if (givenReplayLog == null)
            Manager.Instance.InitTitleScene();
        else
            Manager.Instance.InitHistoryScene();
    }
}
