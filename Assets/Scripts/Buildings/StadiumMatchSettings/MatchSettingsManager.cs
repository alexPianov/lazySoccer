using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using LazySoccer.Network;
using LazySoccer.Utils;
using UnityEngine.UI;
using System.Linq;
using LazySoccer.SceneLoading;
using LazySoccer.Windows;

public class MatchSettingsManager : MonoBehaviour
{
    [SerializeField]
    private MatchConfiguration matchConfiguration;

    [Header("Teams Info")]
    [SerializeField]
    private  TMP_Text homeTeamName;
    [SerializeField]
    private TMP_Text guestTeamName;
    [SerializeField]
    private TMP_Text homeTeamInfo;
    [SerializeField]
    private TMP_Text guestTeamInfo;
    [SerializeField]
    private TMP_Text homeTeamPower;
    [SerializeField]
    private TMP_Text guestTeamPower;

    private List<TeamPlayer> hostPlayers;
    private List<TeamPlayer> guestPlayers;

    [Header("Warnings")]
    [SerializeField]
    private GameObject settingsWarning;
    [SerializeField]
    private GameObject teamRosterWarning;

    [SerializeField]
    private GameSettingsManager gameSettingsManager;
    [SerializeField]
    private PlayerPlaceholdersStorage playerPlaceholdersStorage;

    [SerializeField]
    private Button applySettings;

    private int pressingID;
    private int passRangeID;
    private int defensiveLineID;
    private int markID = (int)Mark.Zonal;
    private int takeBallSlothID;

    public void GetMatchData(Match match)
    {
        matchConfiguration.gameId = match.gameId;

        SetupMatchHeader(match.hostTeam, match.guestTeam);

        GetTeamInfo(match.hostTeam.teamId, match.guestTeam.teamId);

        SetupMatchSettingsMenu();
    }

    private void SetupMatchHeader(Team hostTeam, Team guestTeam)
    {
        homeTeamName.text = hostTeam.name;
        guestTeamName.text = guestTeam.name;

        homeTeamInfo.text = DataUtils.GetDivisionDetailsInfo(hostTeam.division);
        guestTeamInfo.text = DataUtils.GetDivisionDetailsInfo(guestTeam.division);

        //addEmblems
    }

    private async void GetTeamInfo(int hostTeamId, int guestTeamId)
    {
        ServiceLocator.GetService<BuildingWindowStatus>()
            .OpenQuickLoading(true);
        
        hostPlayers = await ServiceLocator.GetService<TeamTypesOfRequests>()
            .GET_TeamPlayers(hostTeamId);
        
        guestPlayers = await ServiceLocator.GetService<TeamTypesOfRequests>()
            .GET_TeamPlayers(guestTeamId);

        await ServiceLocator.GetService<TeamTypesOfRequests>().GET_TeamPlayers(false);
        
        var teamPlayers = ServiceLocator.GetService<ManagerTeamData>().teamPlayersList;
        
        ServiceLocator.GetService<BuildingWindowStatus>()
            .OpenQuickLoading(false);
        
        playerPlaceholdersStorage.LoadTeamRoster(teamPlayers);
        SetPowerLevels();

        if (ServiceLocator.GetService<ManagerPlayerData>().PlayerData.TeamId == hostTeamId)
            SetTicketPrice(1);
        else
            matchConfiguration.ticketPrice = null;
    }

    private void SetPowerLevels()
    {
        int hostPower = 0;
        foreach (var teamMemeber in hostPlayers)
            hostPower += teamMemeber.power;

        homeTeamPower.text = hostPower.ToString();

        int guestPower = 0;
        foreach (var teamMemeber in guestPlayers)
            guestPower += teamMemeber.power;

        guestTeamPower.text = guestPower.ToString();
    }

    public void BackToList()
    {
        ServiceLocator.GetService<StadiumStatus>().SetAction(StatusStadium.UpcomingMatches);
    }

    public void OpenGameSettings()
    {
        List<string> listOfNames = new List<string>();
        List<int> listOfID = new List<int>();
        foreach (var player in playerPlaceholdersStorage.playerPlaceholders)
        {
            if(player.PlayerName != "")
            {
                listOfNames.Add(player.PlayerName);
                listOfID.Add(player.PlayerID);
            }
        }

        gameSettingsManager.SetupTakeBallDropdown(listOfNames, listOfID);

        gameSettingsManager.gameObject.SetActive(true);
    }


    public void SetupMatchSettingsMenu()
    {
        playerPlaceholdersStorage.ResetPlaceholders();
        ValidateData();
    }

    public void ValidateData()
    {
        applySettings.interactable = true;

        if (matchConfiguration.style == -1 || matchConfiguration.tactic == -1)
        {
            settingsWarning.SetActive(true);
            applySettings.interactable = false;
        }
        else
            settingsWarning.SetActive(false);

        foreach (PlayerPlaceholder player in playerPlaceholdersStorage.playerPlaceholders)
            if (player.PlayerID <= 0)
                applySettings.interactable = false;

    }

    #region MATCH CONFIGURATION GROUPING

    public void SetMatchSettings(int styleValue, int tacticsValue, int pressingValue, int passRangeValue, int defensiveLineValue, int takeBallPlayerIndex)
    {
        matchConfiguration.style = styleValue;
        matchConfiguration.tactic = tacticsValue;

        pressingID = pressingValue;
        passRangeID = passRangeValue;
        defensiveLineID = defensiveLineValue;
        takeBallSlothID = takeBallPlayerIndex;

        ValidateData();
    }

    private void SetPlayers(int[] playersId)
    {
        matchConfiguration.players = new PlayerConfiguration[playersId.Length];
        for(int i = 0; i < playersId.Length; i++)
        {
            matchConfiguration.players[i] = new PlayerConfiguration
            {
                playerId = playersId[i]
            };
        }
    }

    public void SetTicketPrice(int ticketPrice)
    {
        matchConfiguration.ticketPrice = ticketPrice;
    }

    private void FinalizeMatchConfiguration()
    {
        SetPlayers(playerPlaceholdersStorage.GetPickedPlayers());

        foreach(var player in matchConfiguration.players)
        {
            var placeholder = playerPlaceholdersStorage.playerPlaceholders.Where(p => p.PlayerID == player.playerId).ToList()[0];

            if(placeholder == null)
            {
                Debug.Log("something went wrong");
                break;
            }

            if (placeholder.RoleFullName == "Lateral" || placeholder.RoleFullName == "Defender")
            {
                Debug.Log("huh");
                player.defensiveLineHeight = defensiveLineID;
                player.mark = markID;
            }

            player.pressing = pressingID;
            player.shotsAllow = passRangeID;

            player.playerPositionId = placeholder.PlayerPositionId;
            player.positionIndex = placeholder.PositionIndex;

            player.isTakeBall = player.playerId == takeBallSlothID ? true : false;

            player.markPlayerId = null;
        }
    }

    public async void SetConfiguration()
    {
        FinalizeMatchConfiguration();

        ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);

        var result = await ServiceLocator.GetService<StadiumTypesOfRequests>().PostMatchLineup(matchConfiguration);
        
        ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
        
        if (result)
        {
            matchConfiguration = new MatchConfiguration();
            BackToList();
            
            ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Match lineup is set");
        }
    }

    #endregion
}
