using System.Collections.Generic;
using UnityEngine;

public class TeamStorage : MonoBehaviour
{
    [SerializeField]
    private PlayerManager[] playerTeamPlaceholders;
    private Dictionary<string, PlayerManager> playerTeam;

    [Header("Team Colors")]
    [SerializeField]
    private Color teamShirtColor;
    [SerializeField]
    private Color teamPatternColor;

    [Header("Goalkeeper colors")]
    [SerializeField]
    private Color goalkeeperShirtColor;
    [SerializeField]
    private Color goalkeeperPatternColor;

    public int TeamSize
    {
        get => playerTeamPlaceholders.Length;
    }

    public void ResetTeam()
    {
        playerTeam = new Dictionary<string, PlayerManager>();

        foreach (PlayerManager playerTeamPlaceholder in playerTeamPlaceholders)
            playerTeamPlaceholder.gameObject.SetActive(false);
    }

    public PlayerManager GetGoalkeeper()
    {
        return playerTeamPlaceholders[0];
    }

    public PlayerManager GetPlayerManager(string id)
    {
        PlayerManager playerManager;
        playerTeam.TryGetValue(id, out playerManager);

        return playerManager;
    }


    public void SetPlayerStartingInfo(int index, string id, Vector2 target)
    {
        SetPlayerId(index, id);

        ActivateTeamColor();
        ActivateGoalkeeperColor();

        SetPlayerPosition(id, target);
        playerTeam[id].gameObject.SetActive(true);
    }

    public void SetPlayerPosition(string id, Vector2 target)
    {
        playerTeam[id].SetPosition(target);
    }
    public void SetPlayerLocalPosition(string id, Vector2 target)
    {
        playerTeam[id].SetLocalPosition(target);
    }

    public void SetPlayerId(int index, string id)
    {
        playerTeam.TryAdd(id, playerTeamPlaceholders[index]);
        playerTeamPlaceholders[index].PlayerID = id;
    }

    public void SetPlayerTarget(string id, Transform target, float turnTime)
    {
        playerTeam[id].SetTarget(target, turnTime);
    }

    public void ChangeTeamAcceleration(float acceleration)
    {
        foreach (PlayerManager playerManager in playerTeamPlaceholders)
        {
            playerManager.MatchAcceleration = acceleration;
            playerManager.ChangeAnimatorAcceleration();
        }
    }

    public void ResetTeamTargets()
    {
        foreach (PlayerManager playerManager in playerTeam.Values)
        {
            playerManager.SetTarget(null, 0);
            playerManager.DisableRunningAnimation(true);
        }
    }

    public void ChangeTeamColor(Color shirtColor, Color patternColor)
    {
        teamShirtColor = shirtColor;
        teamPatternColor = patternColor;

        ActivateTeamColor();
    }

    public void ChangeGoalKeeperColor(Color shirtColor, Color patternColor)
    {
        goalkeeperShirtColor = shirtColor;
        goalkeeperPatternColor = patternColor;

        ActivateGoalkeeperColor();
    }

    private void ActivateGoalkeeperColor()
    {
        playerTeamPlaceholders[0].SetColor(goalkeeperShirtColor, goalkeeperPatternColor);
    }

    private void ActivateTeamColor()
    {
        for (int i = 1; i < playerTeam.Values.Count; i++)
            playerTeamPlaceholders[i].SetColor(teamShirtColor, teamPatternColor);
    }
}
