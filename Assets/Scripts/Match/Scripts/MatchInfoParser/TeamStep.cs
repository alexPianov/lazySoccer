using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class TeamStep
{
    [JsonConstructor]
    public TeamStep
    (
           [JsonProperty("status")] int status,
           [JsonProperty("ballCoordinateX")] int ballCoordinateX,
           [JsonProperty("ballCoordinateY")] int ballCoordinateY,
           [JsonProperty("gameStepIndex")] int gameStepIndex,
           [JsonProperty("startTimeTeamId")] int startTimeTeamId,
           [JsonProperty("stepPlayers")] List<PlayerStep> stepPlayers
    )
    {
        Status = status;
        BallCoordinateX = ballCoordinateX;
        BallCoordinateY = ballCoordinateY;
        GameStepIndex = gameStepIndex;
        StartTimeTeamId = startTimeTeamId;
        StepPlayers = stepPlayers;
    }

    [JsonProperty("status")]
    public int Status { get; }

    [JsonProperty("ballCoordinateX")]
    public int BallCoordinateX { get; }

    [JsonProperty("ballCoordinateY")]
    public int BallCoordinateY { get; }

    [JsonProperty("gameStepIndex")]
    public int GameStepIndex { get; }

    [JsonProperty("startTimeTeamId")]
    public int StartTimeTeamId { get; }

    [JsonProperty("stepPlayers")]
    public IReadOnlyList<PlayerStep> StepPlayers { get; }

    public int GetStepsCounts()
    {
        return StepPlayers.Count;
    }
}
