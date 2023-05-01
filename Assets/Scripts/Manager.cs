using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    [SerializeField]
    private CardScene cardScene;

    public CardScene CardScene
    {
        get { return cardScene; }
    }
}
