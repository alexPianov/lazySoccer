using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MatchData/PositionInfoStorage", order = 3)]
public class PositionInfoStorage : ScriptableObject
{
    [SerializeField]
    private PositionInfo[] positionInfo;

    public PositionInfo GetPositionInfo(PositionNames positionName)
    {
        return positionInfo[(int)positionName];
    }
}
