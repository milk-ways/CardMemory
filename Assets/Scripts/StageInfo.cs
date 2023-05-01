using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageInfo : ScriptableObject
{
    public int Row;
    public int Col;

    public int MatchScore;

    public float ResetTime;
}
