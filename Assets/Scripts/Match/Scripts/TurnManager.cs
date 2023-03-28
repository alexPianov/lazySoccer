using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using LazySoccer.SceneLoading.Buildings.StadiumMatch;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private StatusPopupManager statusPopupManager;

    //goal visualization
    [SerializeField]
    private StadiumMatch stadiumMatch;
    private int hostGoals;
    private int guestGoals;

    [Header("Teams")]
    [SerializeField]
    private TeamStorage teamOneStorage;
    [SerializeField]
    private TeamStorage teamTwoStorage;

    [Header("Game Field")]
    [SerializeField]
    private FieldStorage fieldStorage;
    [SerializeField]
    private BallManager ballManager;

    [Header("Data Parser")]
    [SerializeField]
    private MatchDataParser matchDataParser;

    [Header("Speed Settings")]
    [SerializeField]
    private float turnTime = 1.2f;

    private bool specialTurn = false;
    private PlayerStep specialStatusPlayer;
    private PlayerManager pastBallHavingPlayer;

    [Header("Acceleration settings")]
    [SerializeField]
    private float[] accelerationSpeed;
    private int activeAccelerationIndex;

    private float cellWidth = 0.9f;

    private int firstTeamIndex;
    
    public void StartMatch(int accelerationIndex)
    {
        PrepareTeamsForReset();
        ResetSpecialStatus();
        ResetGoals();

        ballManager.ResetBall();

        SetMatchAcceleration(accelerationIndex);
        StartCoroutine(TurnIteration());
    }

    public bool IsPaused;
    public void PauseMatch()
    {
        IsPaused = !IsPaused;
        
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void CancelAndExit()
    {
        StopAllCoroutines();
        Time.timeScale = 1;
        IsPaused = false;
        statusPopupManager.DeactivatePopup();
        ResetSpecialStatus();
    }

    public void SetMatchAcceleration(int accelerationIndex)
        => activeAccelerationIndex = accelerationIndex;

    public UnityEvent<string> turnCounter = new ();
    public UnityEvent finish = new ();

    private IEnumerator TurnIteration()
    {
        int turnNumber = 0;

        yield return new WaitForSeconds(turnTime / accelerationSpeed[activeAccelerationIndex]);

        if (matchDataParser.GetStepsSize() == 0) yield break;

        while (turnNumber < matchDataParser.GetStepsSize())
        {
            Debug.Log("Turn = " + turnNumber);
            turnCounter.Invoke($"Turn {turnNumber}");
            TeamStep currentStep = matchDataParser.GetStep(turnNumber);
            TeamStep futureStep = matchDataParser.GetStep(turnNumber + 1);

            if (specialTurn && futureStep != null)
            {
                Debug.Log("Match status: " + (Status)currentStep.Status);
                turnCounter.Invoke($"Turn {turnNumber} ({(Status)currentStep.Status})");

                yield return SpecialStatusHandler(specialStatusPlayer.Status, currentStep, futureStep);      
            }
            else
            {
                if (turnNumber == 0)
                    InitialMatchSetup(currentStep, futureStep);
                else
                {
                    NormalStep(currentStep, futureStep);
                }

                yield return new WaitForSeconds(turnTime / accelerationSpeed[activeAccelerationIndex]);
            }

            turnNumber++;
        }

        matchDataParser.TeamSteps = null;

        teamOneStorage.ResetTeamTargets();
        teamTwoStorage.ResetTeamTargets();

        Debug.Log("Finish");
        turnCounter.Invoke($"Finish");
        finish.Invoke();

        yield break;
    }

    public void Reboot()
    {
        PrepareTeamsForReset();

        if (matchDataParser)
        {
            TeamStep currentStep = matchDataParser.GetStep(0);
            TeamStep futureStep = matchDataParser.GetStep(1);

            InitialMatchSetup(currentStep, futureStep);
        }
    }

    #region SPECIAL TURNS

    private IEnumerator SpecialStatusHandler(int statusCode, TeamStep currentStep, TeamStep futureStep)
    {
        teamOneStorage.ResetTeamTargets();
        teamTwoStorage.ResetTeamTargets();

        statusPopupManager.ActivatePopup(statusCode);
        yield return new WaitForSeconds(2f);

        switch ((Status)statusCode)
        {
            case Status.Goal: yield return GoalHandler(futureStep); UpdateGoals(specialStatusPlayer.TeamId == ServiceLocator.GetService<ManagerPlayerData>().PlayerData.TeamId); break;
            case Status.ThrowIn: yield return ThrowInHandler(currentStep, futureStep); break;
            case Status.Corner: yield return ThrowInHandler(currentStep, futureStep); break;
            case Status.GoalKick: yield return ThrowInHandler(currentStep, futureStep); break;
            case Status.Penalty: yield return GoalHandler(futureStep); break;
            case Status.Freekick: yield return GoalHandler(futureStep); break;
            default: yield return null; break;
        }

        statusPopupManager.DeactivatePopup();

        ResetSpecialStatus();
    }

    private void ResetSpecialStatus()
    {
        specialStatusPlayer = null;
        specialTurn = false;
    }

    private void ResetGoals()
    {
        hostGoals = 0;
        guestGoals = 0;

        stadiumMatch.UpdateGoalCount(hostGoals, guestGoals);
    }

    private void UpdateGoals(bool isHost)
    {
        if (isHost)
            hostGoals++;
        else
            guestGoals++;

        stadiumMatch.UpdateGoalCount(hostGoals, guestGoals);
    }


    private IEnumerator GoalHandler(TeamStep futureStep)
    {
        ChangeTeamAcceleration();

        SetTeamPositions(futureStep, false);

        ballManager.transform.position = FindPlayerById(futureStep.StepPlayers.FirstOrDefault(player => player.IsBall == true).PlayerStatId).transform.position;

        yield return new WaitForSeconds((turnTime / accelerationSpeed[activeAccelerationIndex]) * 2f);
    }

    private IEnumerator ThrowInHandler(TeamStep currentStep, TeamStep futureStep)
    {
        var ballHandlerTeam = specialStatusPlayer.TeamId == firstTeamIndex ? teamTwoStorage : teamOneStorage;

        PlayerManager ballHandler = ballHandlerTeam.GetGoalkeeper();
        PlayerManager ballHaver = FindPlayerById(futureStep.StepPlayers.FirstOrDefault(player => player.IsBall == true).PlayerStatId);

        bool isBallAtRightSide = ballManager.transform.position.x > 0;

        //ballHandler.SetPosition(ballManager.transform.position);��
        //ballHandler.Rotate(isBallAtRightSide);
        ballManager.ChangeBallCoords(ballHandler.transform.position);

        yield return new WaitForSeconds(turnTime / accelerationSpeed[activeAccelerationIndex]);

        ballManager.PassBetweenPlayers(ballHaver.transform, turnTime / accelerationSpeed[activeAccelerationIndex]);

        yield return new WaitForSeconds(turnTime / accelerationSpeed[activeAccelerationIndex]);
    }

    #endregion

    private void InitialMatchSetup(TeamStep currentStep, TeamStep futureStep)
    {
        if(currentStep == null || futureStep == null) return;

        firstTeamIndex = currentStep.StepPlayers[0].TeamId;

        SetTeamPositions(currentStep, true);

        ChangeTeamAcceleration();

        if (futureStep != null)
        {
            var ballHaver = futureStep.StepPlayers.FirstOrDefault(player => player.IsBall == true);
            ballManager.PassBetweenPlayers(FindPlayerById(ballHaver.PlayerStatId).transform, turnTime / accelerationSpeed[activeAccelerationIndex]);
        }
    }

    public void PrepareTeamsForReset()
    {
        teamOneStorage.ResetTeam();
        teamTwoStorage.ResetTeam();

        teamOneStorage.ChangeTeamAcceleration(accelerationSpeed[0]);
        teamTwoStorage.ChangeTeamAcceleration(accelerationSpeed[0]);

        teamOneStorage.ResetTeamTargets();
        teamTwoStorage.ResetTeamTargets();
    }

    private void ChangeTeamAcceleration()
    {
        teamOneStorage.ChangeTeamAcceleration(accelerationSpeed[activeAccelerationIndex]);
        teamTwoStorage.ChangeTeamAcceleration(accelerationSpeed[activeAccelerationIndex]);
    }

    private void NormalStep(TeamStep currentStep, TeamStep futureStep)
    {
        SetTeamTargets(currentStep);

        if (futureStep != null)
        {
            CheckStepForSpecialStatus(futureStep);
            if (!specialTurn)
                DetermineBallMovement(currentStep, futureStep);
            else
                ballManager.PassBetweenPlayers(fieldStorage.GetCellTransform(futureStep.BallCoordinateY, futureStep.BallCoordinateX), turnTime);
        }
    }

    private void SetTeamTargets(TeamStep currentStep)
    {
        var playerSteps = currentStep.StepPlayers;

        int firstTeamCounter = 0;
        int secondTeamCounter = 0;

        ChangeTeamAcceleration();

        for (int i = 0; i < playerSteps.Count; i++)
        {
            if (playerSteps[i].TeamId == firstTeamIndex)
            {
                teamOneStorage.SetPlayerTarget(playerSteps[i].PlayerStatId, fieldStorage.GetCellTransform(playerSteps[i].CoordinateY, playerSteps[i].CoordinateX), turnTime);
                firstTeamCounter++;
            }
            else
            {
                teamTwoStorage.SetPlayerTarget(playerSteps[i].PlayerStatId, fieldStorage.GetCellTransform(playerSteps[i].CoordinateY, playerSteps[i].CoordinateX), turnTime);
                secondTeamCounter++;
            }
            Debug.Log(playerSteps[i].Log);
        }
    }

    private void SetTeamPositions(TeamStep step, bool isStart)
    {
        var playerSteps = step.StepPlayers;

        int firstTeamCounter = 0;
        int secondTeamCounter = 0;

        Dictionary<Vector2, List<string>> coordsStorage = new();

        foreach(var playerStep in playerSteps)
        {
            var key = new Vector2(playerStep.CoordinateX, playerStep.CoordinateY);
            if (coordsStorage.ContainsKey(key))
                coordsStorage[key].Add(playerStep.PlayerStatId);
            else
                coordsStorage.TryAdd(key, new List<string> { playerStep.PlayerStatId });
        }          

        for (int i = 0; i < playerSteps.Count; i++)
        {
            bool isTeamOne = playerSteps[i].TeamId == firstTeamIndex;

            if (isStart)
                SetPlayerPosition(playerSteps[i], true, isTeamOne ? teamOneStorage : teamTwoStorage, isTeamOne ? firstTeamCounter : secondTeamCounter, coordsStorage);
            else
                SetPlayerPosition(playerSteps[i], false, isTeamOne ? teamOneStorage : teamTwoStorage, isTeamOne ? firstTeamCounter : secondTeamCounter, coordsStorage);

            if (isTeamOne)
                firstTeamCounter++;
            else
                secondTeamCounter++;
        }
    }

    private void SetPlayerPosition(PlayerStep playerStep, bool isStart, TeamStorage teamStorage, int index, Dictionary<Vector2, List<string>> coordsStorage)
    {
        if (isStart)
            teamStorage.SetPlayerStartingInfo(index, playerStep.PlayerStatId, fieldStorage.GetCellCoordinates(playerStep.CoordinateY, playerStep.CoordinateX));
        else
        {
            List<string> value;
            if (coordsStorage.TryGetValue(new Vector2(playerStep.CoordinateX, playerStep.CoordinateY), out value) && value.Count > 1)
            {
                float adjustments = (cellWidth / (value.Count + 1)) * value.IndexOf(playerStep.PlayerStatId);
                Debug.Log(adjustments);

                Vector2 cellCoords = fieldStorage.GetCellCoordinates(playerStep.CoordinateY, playerStep.CoordinateX);

                var thisPlayer = FindPlayerById(playerStep.PlayerStatId);

                teamStorage.SetPlayerPosition(playerStep.PlayerStatId, cellCoords);
                teamStorage.SetPlayerLocalPosition(playerStep.PlayerStatId, new Vector2(thisPlayer.transform.localPosition.x - 0.45f + adjustments, thisPlayer.transform.localPosition.y));
            }
            else
                teamStorage.SetPlayerPosition(playerStep.PlayerStatId, fieldStorage.GetCellCoordinates(playerStep.CoordinateY, playerStep.CoordinateX));
        }
    }

    private void CheckStepForSpecialStatus(TeamStep futureStep)
    {
        var playerSteps = futureStep.StepPlayers;

        for (int i = 0; i < playerSteps.Count; i++)
            CheckPlayerForSpecialStatus(playerSteps[i]);
    }

    private void CheckPlayerForSpecialStatus(PlayerStep playerStep)
    {
        if (playerStep.Status != 0)
        {
            specialStatusPlayer = playerStep;
            Debug.Log("Player status: " + (Status)playerStep.Status);

            specialTurn = true;
        }
    }

    private void DetermineBallMovement(TeamStep currentStep, TeamStep futureStep)
    {
        var playerCurrent = currentStep.StepPlayers.FirstOrDefault(player => player.IsBall == true);
        var playerFuture = futureStep.StepPlayers.FirstOrDefault(player => player.IsBall == true);

        //player loses ball
        if (playerFuture == null)
        {   
            if(pastBallHavingPlayer)
                pastBallHavingPlayer.Rotate(futureStep.BallCoordinateX < pastBallHavingPlayer.transform.position.x);

            ballManager.PassBetweenPlayers(fieldStorage.GetCellTransform(futureStep.BallCoordinateY, futureStep.BallCoordinateX), turnTime / accelerationSpeed[activeAccelerationIndex]);
        }
        else if(playerCurrent == null || playerCurrent.PlayerStatId != playerFuture.PlayerStatId)
        {
            if (pastBallHavingPlayer)
                pastBallHavingPlayer.Rotate(FindPlayerById(playerFuture.PlayerStatId).transform.position.x < pastBallHavingPlayer.transform.position.x);
            ballManager.PassBetweenPlayers(FindPlayerById(playerFuture.PlayerStatId).transform, turnTime / accelerationSpeed[activeAccelerationIndex]);
            pastBallHavingPlayer = FindPlayerById(playerFuture.PlayerStatId);
        }
        else
        {
            ballManager.StickToPlayer(FindPlayerById(playerCurrent.PlayerStatId).transform, turnTime / accelerationSpeed[activeAccelerationIndex]);
            pastBallHavingPlayer = FindPlayerById(playerCurrent.PlayerStatId);
        }
    }

    private PlayerManager FindPlayerById(string id)
    {
        var playerFromTeamOne = teamOneStorage.GetPlayerManager(id);
        var playerFromTeamTwo = teamTwoStorage.GetPlayerManager(id);

        return playerFromTeamOne == null ? playerFromTeamTwo : playerFromTeamOne;
    }
}


public enum Status
{
    None,
    Goal,
    ThrowIn,
    Corner,
    GoalKick,
    Penalty,
    Freekick
}