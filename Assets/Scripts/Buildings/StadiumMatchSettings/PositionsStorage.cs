using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "MatchData/PositionsStorage", order = 1)]
public class PositionsStorage : ScriptableObject
{
  //  [SerializeField]
    public PositionCodes positionCode;

  //  [SerializeField]
    public LineupPosition[] positionsStorage = new LineupPosition[11];
}