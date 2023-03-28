using Newtonsoft.Json;
using System;

[Serializable]
public class PlayerStep
{
    [JsonConstructor]
    public PlayerStep
    (
            [JsonProperty("playerStepId")] int playerStepId,

            [JsonProperty("coordinateX")] int coordinateX,
            [JsonProperty("coordinateY")] int coordinateY,

            [JsonProperty("actionStart")] string actionStart,
            [JsonProperty("actionEnd")] string actionEnd,

            [JsonProperty("log")] string log,

            [JsonProperty("stamina")] double stamina,
            [JsonProperty("safeTime")] int safeTime,

            [JsonProperty("isYellowCard")] bool isYellowCard,
            [JsonProperty("isRedCard")] bool isRedCard,
            [JsonProperty("isBall")] bool isBall,

            [JsonProperty("status")] int status,

            [JsonProperty("playerStatId")] string playerStatId,
            [JsonProperty("interactionPlayerId")] string interactionPlayerId,
            [JsonProperty("markPlayerId")] string markPlayerId,

            [JsonProperty("teamId")] int teamId,

            [JsonProperty("position")] PlayerPosition position
    )
    {
        PlayerStepId = playerStepId;

        CoordinateX = coordinateX;
        CoordinateY = coordinateY;

        ActionStart = actionStart;
        ActionEnd = actionEnd;

        Log = log;

        Stamina = stamina;
        SafeTime = safeTime;

        IsYellowCard = isYellowCard;
        IsRedCard = isRedCard;
        IsBall = isBall;

        Status = status;

        PlayerStatId = playerStatId;
        InteractionPlayerId = interactionPlayerId;
        MarkPlayerId = markPlayerId;

        TeamId = teamId;

        Position = position;
    }

    [JsonProperty("playerStepId")]
    public int PlayerStepId { get; }

    [JsonProperty("coordinateX")]
    public int CoordinateX { get; }

    [JsonProperty("coordinateY")]
    public int CoordinateY { get; }

    [JsonProperty("actionStart")]
    public string ActionStart { get; }

    [JsonProperty("actionEnd")]
    public string ActionEnd { get; }

    [JsonProperty("log")]
    public string Log { get; }

    [JsonProperty("stamina")]
    public double Stamina { get; }

    [JsonProperty("safeTime")]
    public int SafeTime { get; }

    [JsonProperty("isYellowCard")]
    public bool IsYellowCard { get; }

    [JsonProperty("isRedCard")]
    public bool IsRedCard { get; }

    [JsonProperty("isBall")]
    public bool IsBall { get; }

    [JsonProperty("status")]
    public int Status { get; }

    [JsonProperty("playerStatId")]
    public string PlayerStatId { get; }

    [JsonProperty("interactionPlayerId")]
    public string InteractionPlayerId { get; }

    [JsonProperty("markPlayerId")]
    public string MarkPlayerId { get; }

    [JsonProperty("teamId")]
    public int TeamId { get; }

    [JsonProperty("position")]
    public PlayerPosition Position { get; }
}

[Serializable]
public class PlayerPosition
{
    [JsonConstructor]
    public PlayerPosition
    (
            [JsonProperty("playerPositionId")] int playerPositionId,
            [JsonProperty("position")] string position,
            [JsonProperty("fieldLocation")] string fieldLocation
    )
    {
        PlayerPositionId = playerPositionId;
        Position = position;
        FieldLocation = fieldLocation;
    }

    [JsonProperty("playerPositionId")]
    public int PlayerPositionId { get; }

    [JsonProperty("position")]
    public string Position { get; }

    [JsonProperty("fieldLocation")]
    public string FieldLocation { get; }
}
