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
    private GameObject gameOverObject;
    [SerializeField]
    private TextMeshProUGUI gameOverScoreText;

    private List<Card> cards;
    private List<int> cardValues;

    private int totalScore;

    private int nowLevel;
    private int remainCards;

    private float maxTime;
    private float remainTime;

    private bool IsGameDone;

    private Card firstCard, secondCard;

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

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (IsGameDone)
            return;

        if (remainTime <= 0)
        {
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

    private void ResetAll()
    {
        //초기화 작업
        nowLevel = totalScore = 0;
        cards = new List<Card>();
        cardValues = new List<int>();
        RugPanel.gameObject.SetActive(true);
        gameOverObject.SetActive(false);
        IsGameDone = false;

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

    private void SetCard()
    {
        List<int> cardMemory = new List<int>();
        for (int i = 0; i < cardValues.Count; i++)
        {
            var card = cards[i];
            card.Init(cardValues[i], i);
            cardMemory.Add(cardValues[i]);
        }
    }

    public void FlipCards(Card card)
    {
        if (flipedCount == 0)
        {
            firstCard = card;
        }
        else if (flipedCount == 1)
        {
            secondCard = card;

            StartCoroutine(CheckCardMatches());
        }
    }

    private IEnumerator CheckCardMatches()
    {
        yield return new WaitForSeconds(1f);

        if (firstCard.cardType == secondCard.cardType && firstCard.CardNum == secondCard.CardNum)
        {
            totalScore += stageInfos[nowLevel].MatchScore;
            scoreText.text = totalScore.ToString();

            firstCard.DeActivateCard();
            secondCard.DeActivateCard();

            remainCards -= 2;
            if (remainCards <= 0)
            {
                nowLevel++;

                if (nowLevel >= stageInfos.Count)   GameClear();
                else  SetNormalStage();
            }
        }
        else
        {
            StartCoroutine(firstCard.FlipAnimation());
            StartCoroutine(secondCard.FlipAnimation());
        }

        firstCard = null;
        secondCard = null;
    }

    private void SetNormalStage()
    {
        firstCard = null;
        secondCard = null;

        SetStageCardAndInfo();
        GetRandomCard();
        Utils.Shuffle(cardValues);
        SetCard();
    }

    private void GameClear()
    {
        IsGameDone = true;
        RugPanel.gameObject.SetActive(false);
        gameOverObject.SetActive(true);
        gameOverScoreText.text = "Score : " + totalScore;

        if (totalScore > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", totalScore);
    }
}
