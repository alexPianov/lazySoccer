using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchStatistics
{
    public class StadiumMatchStatisticsRow : MonoBehaviour
    {
        [Header("Text")]
        [SerializeField] private TMP_Text textPosession;
        [SerializeField] private TMP_Text textShots;
        [SerializeField] private TMP_Text textSuccessfulPasses;
        [SerializeField] private TMP_Text textUnsuccesfulPasses;
        [SerializeField] private TMP_Text textFaults;
        [SerializeField] private TMP_Text textCornerStrikes;
        [SerializeField] private TMP_Text textOffsidesQuantity;
        [SerializeField] private TMP_Text textTotalYellowCards;
        [SerializeField] private TMP_Text textBallPositionPercent;
        
        [Header("Button")]
        [SerializeField] private Button buttonLineup;
        [SerializeField] private Button buttonReplaces;

        [Header("Refs")] 
        [SerializeField] private StadiumMatchStatisticsLineup StatisticsLineup;
        [SerializeField] private StadiumMatchStatisticsReplaces StatisticsReplaces;

        private ManagerTeamData _managerTeamData;
        private TeamGameStats _teamGameStats;
        private List<PlayerGameStats> _teamReplaces;

        private void Start()
        {
            _managerTeamData = ServiceLocator.GetService<ManagerTeamData>();
            buttonLineup.onClick.AddListener(ShowLineup);
            buttonReplaces.onClick.AddListener(ShowReplaces);
        }

        public void SetStatistics(TeamGameStats stats, List<PlayerGameStats> playerGameStats)
        {
            if(stats == null) { return; }

            _teamGameStats = stats;
            _teamReplaces = playerGameStats;

            textPosession.text = $"{stats.possessionOnBallPercent}";
            textShots.text = $"{stats.strikesQuantity}"; // Shots = strikes ?
            textSuccessfulPasses.text = $"{stats.successfulPassesQuantity}";
            textUnsuccesfulPasses.text = $"{stats.unsuccessfulPassesQuantity}";
            textFaults.text = $"{stats.fallsQuantity}"; // Faults = falls ?
            textCornerStrikes.text = $"{stats.quantityOfCornerStrikes}";
            textOffsidesQuantity.text = $"{stats.offsidesQuantity}";
            textTotalYellowCards.text = $"{stats.yellowCards}";
            textBallPositionPercent.text = $"{null}"; // ?

            var isOwnTeam = _teamGameStats.teamId == _managerTeamData.teamStatisticData.teamId;
            
            buttonLineup.interactable = isOwnTeam;
            buttonReplaces.interactable = isOwnTeam;
            
            buttonReplaces.interactable = StatisticsReplaces.HasRelegatedPlayers(_teamReplaces);
        }

        private void ShowLineup()
        {
            StatisticsLineup.OpenTeamList(_teamReplaces);
        }

        private void ShowReplaces()
        {
            StatisticsReplaces.SetTeamReplaces(_teamReplaces);
        }
    }
}