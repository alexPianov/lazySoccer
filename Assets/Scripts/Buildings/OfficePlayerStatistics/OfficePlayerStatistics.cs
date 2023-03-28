using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerStatistics : MonoBehaviour
    {
        [SerializeField] private OfficePlayerStatisticsPerformance performanceList;
        [SerializeField] private OfficePlayerStatisticsRewardsList rewardsList;
        [SerializeField] private OfficePlayerStatisticsTrophyList trophyList;
        
        [Title("Player Data")]
        [SerializeField] private OfficePlayer OfficePlayer;

        private List<TeamPlayerReward> playerRewards = null;
        private List<TeamPlayerTrophy> playerTrophies = null;

        public async void UpdateInfo()
        {
            Debug.Log("Update Statistic Info");

            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);

            var id = OfficePlayer.playerId;
            
            if (playerRewards == null)
            {
                playerRewards = await ServiceLocator.GetService<TeamTypesOfRequests>()
                    .GET_TeamPlayerRewards(id);
            }

            if (playerTrophies == null)
            {
                playerTrophies = await ServiceLocator.GetService<TeamTypesOfRequests>()
                    .GET_TeamPlayerTrophies(id);
            }
            
            performanceList.SetInfo(OfficePlayer.CurrentPlayerStatistics, OfficePlayer.playerGoalsCount);

            rewardsList.CreateRewards(playerRewards);
            trophyList.CreateTrophy(playerTrophies);
            
            //ServiceLocator.GetService<BuildingWindowStatus>().SetAction(StatusBuilding.OfficePlayer);
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
        }

        public void ClearCache()
        {
            playerRewards = null;
            playerTrophies = null;
        }
    }
}