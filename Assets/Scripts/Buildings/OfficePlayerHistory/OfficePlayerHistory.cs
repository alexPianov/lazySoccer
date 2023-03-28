using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayerHistory
{
    public class OfficePlayerHistory : MonoBehaviour
    {
        [SerializeField] private OfficePlayerHistoryMatchesList historyMatchesList;
        [SerializeField] private OfficePlayerHistoryTransferList historyTransferList;

        [Title("Player Data")]
        [SerializeField] private OfficePlayer.OfficePlayer OfficePlayer;
        
        private List<Match> playerMatches;
        private List<TeamPlayerTransfer> playerTransfers;
    
        public async void UpdateInfo()
        {
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);
            var season = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Season;
            
            var id = OfficePlayer.playerId;
            
            if (playerMatches == null)
            {
                playerMatches = await ServiceLocator.GetService<TeamTypesOfRequests>()
                    .GET_TeamPlayerMatches(id, season);
            }

            if (playerTransfers == null)
            {
                playerTransfers = await ServiceLocator.GetService<TeamTypesOfRequests>()
                    .GET_TeamPlayerTransfers(id);
            }

            historyMatchesList.CreateMatches(playerMatches);
            historyTransferList.CreateTransfer(playerTransfers);
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
        }
        
        public void ClearCache()
        {
            playerTransfers = null;
            playerMatches = null;
        }
    }
}