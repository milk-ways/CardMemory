using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static void Shuffle<T>(List<T> inputs)
    {
        for (int i = 0; i < inputs.Count - 1; i++)
        {
            T temp = inputs[i];
            int rand = Random.Range(i, inputs.Count);
            inputs[i] = inputs[rand];
            inputs[rand] = temp;
        }
    }
}

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; } = null;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}