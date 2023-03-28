using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MatchData/LineupsStorage", order = 2)]
public class LineupsStorage : ScriptableObject
{
    [SerializeField]
    public PositionsStorage[] lineupsStorage;
}
