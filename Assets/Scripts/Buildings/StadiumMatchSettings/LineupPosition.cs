using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineupPosition
{
    public PositionNames positionName;

    public Vector2 positionCoords;
}
public enum PositionNames
{
    Goalkeeper,
    Forward,
    Midfielder,
    Defender,
    Winger,
    Lateral
}



