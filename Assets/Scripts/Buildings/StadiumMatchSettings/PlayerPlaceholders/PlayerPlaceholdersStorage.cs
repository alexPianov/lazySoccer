using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using System.Linq;

public class PlayerPlaceholdersStorage : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private MatchSettingsManager matchSettingsManager;
    [SerializeField]
    private TeamRosterManager teamRosterManager;

    [Header("Placeholders")]
    public PlayerPlaceholder[] playerPlaceholders;

    [Header("Storages")]
    [SerializeField]
    private PositionInfoStorage positionInfoStorage;
    [SerializeField]
    private FieldStorage fieldStorage;

    private List<TeamPlayer> teamPlayers;
    private int pickedPlaceholderID;

    private int[] pickedPlayersId;

    private const int teamSize = 11;

    public void SetupPlayerPlaceholders(PositionsStorage positionsStorage)
    {
        pickedPlayersId = new int[teamSize];

        for (int i = 0; i < playerPlaceholders.Length; i++)
        {
            var flatPosition = fieldStorage.GetCellCoordinates((int)positionsStorage.positionsStorage[i].positionCoords.y, (int)positionsStorage.positionsStorage[i].positionCoords.x);
            var newPosition = new Vector3(flatPosition.x, flatPosition.y, 0);

            playerPlaceholders[i].SetRoleAndPosition(newPosition, positionInfoStorage.GetPositionInfo(positionsStorage.positionsStorage[i].positionName), positionsStorage.positionsStorage[i].positionCoords);
            playerPlaceholders[i].gameObject.SetActive(true);
        }

        matchSettingsManager.ValidateData();
    }

    public void LoadTeamRoster(List<TeamPlayer> teamPlayers)
    {
        this.teamPlayers = teamPlayers;
    }

    public void SetTeamRoster(int pickedButtonId)
    {
        List<TeamPlayer> availableTeamPlayers = teamPlayers.Where(p => !pickedPlayersId.Contains(p.playerId)).ToList();

        if(playerPlaceholders[pickedButtonId].RoleFullName == "Goalkeeper")
        {
            availableTeamPlayers = availableTeamPlayers.Where(p => p.position.position == "Goalkeeper").ToList();
        }
        else
        {
            availableTeamPlayers = availableTeamPlayers.Where(p => p.position.position != "Goalkeeper").ToList();
        }

        teamRosterManager.SetupList(availableTeamPlayers);
        teamRosterManager.gameObject.SetActive(true);

        pickedPlaceholderID = pickedButtonId;
    }

    public void SetPlayer()
    {
        var tempId = teamRosterManager.GetPickedPlayer();

        if(tempId != -1)
        {
            foreach (var teamPlayer in teamPlayers)
                if (teamPlayer.playerId == tempId)
                {
                    playerPlaceholders[pickedPlaceholderID].SetPlayerIdentificationInfo(teamPlayer);
                    pickedPlayersId[pickedPlaceholderID] = tempId;
                }
        }

        teamRosterManager.ResetRoster();
        matchSettingsManager.ValidateData();
    }

    public int[] GetPickedPlayers() => pickedPlayersId;

    public void ResetPlaceholders()
    {
        foreach(var placeholder in playerPlaceholders)
        {
            placeholder.ResetPlaceholder();
        }
    }
}
