using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MatchConfiguration
{
    public string gameId;
    public int style = -1;
    public int tactic = -1;
     public int? ticketPrice;

    public PlayerConfiguration[] players;
}


public class PlayerConfiguration
{
    public int playerId;

    public int playerPositionId;
    public int positionIndex;

    public int pressing;
    public int shotsAllow;
    public int defensiveLineHeight;
    public int mark;
    public bool isTakeBall;
    public int? markPlayerId;
}

