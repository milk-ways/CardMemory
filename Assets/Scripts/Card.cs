using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private Sprite cardBack;
    private Sprite cardFace;

    public int CardNum;
    public CardType cardType;

    private int index;

    private bool facedUp;

    public void Init(int num, int index, bool noReplay)
    {
        ResetCard(noReplay);

        this.index = index;
        cardType = (CardType)(num / 13);
        CardNum = num % 13 + 1;

        cardFace = Resources.Load<Sprite>($"CardImages/{cardType}{CardNum}");
        GetComponent<Button>().onClick.AddListener(() => FlipCard());
    }

    private void ResetCard(bool noReplay)
    {
        GetComponent<Button>().interactable = noReplay;
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
        GetComponent<Image>().sprite = cardBack;
        facedUp = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void DeActivateCard()
    {
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void FlipCard()
    {
        if (!facedUp && Manager.Instance.CardScene.flipedCount < 2)
        {
            Manager.Instance.CardScene.IsCardMOrF = true;
            StartCoroutine(FlipAnimation());
            Manager.Instance.CardScene.AddTouchInfo(index);
            Manager.Instance.PlayEffect("FlipCard");
        }
    }

    public IEnumerator FlipAnimation()
    {
        facedUp = !facedUp;

        if (facedUp)
        {
            for (float i = 0; i <= 180; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90f)
                {
                    GetComponent<Image>().sprite = cardFace;
                }
                yield return new WaitForSeconds(.01f);
            }

            Manager.Instance.CardScene.FlipCards(this);
        }
        else
        {
            for (float i = 180; i >= 0; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0, i, 0);
                if (i == 90f)
                {
                    GetComponent<Image>().sprite = cardBack;
                }
                yield return new WaitForSeconds(.01f);
            }
        }
    }
}

public enum CardType
{
    Spade,
    Diamond,
    Heart,
    Club
};
