using System;
using System.Collections.Generic;
using System.Linq;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayer : MonoBehaviour
    {
        public TeamPlayer CurrentPlayer { get; protected set; }
        public int playerId { get; protected set; }
        public int playerGoalsCount { get; protected set; }
        public int playerSalary { get; protected set; }
        public Position playerPosition { get; protected set; }
        public TeamPlayerStatus playerStatus { get; protected set; }
        public List<Trait> playerTraits { get; protected set; }
        public int playerAge { get; protected set; }
        public Team playerTeam { get; protected set; }
        public List<TeamPlayerSkill> skills { get; protected set; }
        
        public TeamPlayerStatistics CurrentPlayerStatistics { get; protected set; }
        public ManagerTeamData TeamStatistic { get; protected set; }

        [SerializeField]
        protected OfficePlayerHistory.OfficePlayerHistory officePlayerHistory;
        
        [SerializeField]
        protected OfficePlayerStatistics officePlayerStatistics;
        
        protected TeamTypesOfRequests _team;
        protected BuildingWindowStatus _buildingWindowStatus;
        private OfficePlayerStatus _officePlayerStatus;

        public void Start()
        {
            _team = ServiceLocator.GetService<TeamTypesOfRequests>();
            _officePlayerStatus = ServiceLocator.GetService<OfficePlayerStatus>();
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            TeamStatistic = ServiceLocator.GetService<ManagerTeamData>();
        }

        public async void SetPlayer(TeamPlayer player)
        {
            Debug.Log("OFFICE PLAYER - SetPlayer: " + player.playerId);
            CurrentPlayer = player;
            
            playerId = player.playerId;
            playerGoalsCount = player.goalCount;
            playerPosition = player.position;
            playerSalary = player.dailySalary;
            playerTraits = player.traits;
            playerAge = player.age;
            playerStatus = player.status;
            playerTeam = playerTeam;
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);
            
            CurrentPlayerStatistics = await _team
                .GET_TeamPlayerStatistics(player.playerId);

            skills = await _team.GET_PlayerSkill(player.playerId);

            if (CurrentPlayerStatistics == null)
            {
                Debug.LogError("Failed to find current player statistic: " + player.name);
                _buildingWindowStatus.SetAction(StatusBuilding.Office);
                return;
            }
            
            _buildingWindowStatus.SetAction(StatusBuilding.OfficePlayer);
            _officePlayerStatus.SetAction(StatusOfficePlayer.Traits);
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
            
            officePlayerHistory.ClearCache();
            officePlayerStatistics.ClearCache();
        }
    }
}