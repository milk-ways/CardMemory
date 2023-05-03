using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private AudioSource audioSource;

    public CardScene CardScene
    {
        get { return cardScene; }
    }

    public HistoryScene HistoryScene
    {
        get { return historyScene; }
    }

    private string path;
    private string historyFilename;
    private string logFilename;

    public static SerializableDictionary<string, HistoryInfo> History
    {
        get;
        private set;
    }
    public static SerializableDictionary<string, ReplayLog> ReplayLog
    {
        get;
        private set;
    }

    protected override void Awake()
    {
        base.Awake();

        nowScene = titleScene;

        audioSource = GetComponent<AudioSource>();

        path = Application.persistentDataPath + "/";
        historyFilename = "GameHistory";
        logFilename = "ReplayLog";

        if (!File.Exists(path + historyFilename))
        {
            History = new SerializableDictionary<string, HistoryInfo>();
            ReplayLog = new SerializableDictionary<string, ReplayLog>();
        }
        else
        {
            string data = File.ReadAllText(path + historyFilename);
            History = JsonUtility.FromJson<SerializableDictionary<string, HistoryInfo>>(data);
            data = File.ReadAllText(path + logFilename);
            ReplayLog = JsonUtility.FromJson<SerializableDictionary<string, ReplayLog>>(data);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (nowScene == titleScene)
            {
                Application.Quit();
            }
            else if (nowScene == historyScene.gameObject)
            {
                InitTitleScene();
            }
            else if (nowScene == cardScene.gameObject)
            {
                cardScene.BackButton();
            }
        }
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

    public void InitCardScene(ReplayLog replayLog)
    {
        nowScene.SetActive(false);
        cardScene.gameObject.SetActive(true);
        nowScene = cardScene.gameObject;
        cardScene.Init(replayLog);
    }

    public void SaveHistory()
    {
        string json = JsonUtility.ToJson(History);
        File.WriteAllText(path + historyFilename, json);
    }

    public void SaveReplayLog()
    {
        string json = JsonUtility.ToJson(ReplayLog);
        File.WriteAllText(path + logFilename, json);
    }

    public void PlayEffect(string audioName)
    {
        var audioClip = Resources.Load<AudioClip>($"Audio/{audioName}");

        if(audioClip != null)
            audioSource.PlayOneShot(audioClip);
    }

    public void ButtonClickSound()
    {
        PlayEffect("ButtonClick");
    }
}
