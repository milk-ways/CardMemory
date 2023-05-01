using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    [SerializeField]
    private CardScene cardScene;
    [SerializeField]
    private HistoryScene historyScene;
    [SerializeField]
    private GameObject titleScene;

    private GameObject nowScene;

    public CardScene CardScene
    {
        get { return cardScene; }
    }

    protected override void Awake()
    {
        base.Awake();

        nowScene = titleScene;
    }

    public void InitTitleScene()
    {
        nowScene.SetActive(false);
        titleScene.SetActive(true);
        nowScene = titleScene.gameObject;
    }

    public void InitHistoryScene()
    {
        nowScene.SetActive(false);
        historyScene.gameObject.SetActive(true);
        nowScene = historyScene.gameObject;
        historyScene.Init();
    }

    public void InitCardScene()
    {
        nowScene.SetActive(false);
        cardScene.gameObject.SetActive(true);
        nowScene = cardScene.gameObject;
        cardScene.Init();
    }
}
