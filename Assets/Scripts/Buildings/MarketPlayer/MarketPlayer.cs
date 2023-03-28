using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Status;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayer : OfficePlayer.OfficePlayer
    {
        [SerializeField] private MarketPlayerInfo _marketPlayerInfo;
        [SerializeField] private MarketPlayerButtonsEdit _marketPlayerButtonsEdit;
        [SerializeField] private MarketPlayerButtonsOffer _marketPlayerButtonsOffer;
        public List<MarketOffer> Offers { get; set; }
        public MarketPlayerTransfer Transfer { get; set; }
        
        private MarketPlayerStatus _marketPlayerStatus;
        private MarketTypesOfRequests _marketTypesOfRequests;
        private ManagerTeamData _managerTeamData;
        private void Start()
        {
            _marketPlayerStatus = ServiceLocator.GetService<MarketPlayerStatus>();
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            _managerTeamData = ServiceLocator.GetService<ManagerTeamData>();
            
            base.Start();
        }

        public async void ShowPlayer(MarketPlayerTransfer transfer)
        {
            Transfer = transfer;

            playerId = transfer.player.playerId;
            playerGoalsCount = transfer.player.goalCount;
            playerPosition = transfer.player.position;
            playerSalary = transfer.player.dailySalary;
            playerTraits = transfer.player.traits;
            playerStatus = TeamPlayerStatus.OnTransferMarket;
            
            _buildingWindowStatus.SetAction(StatusBuilding.QuickLoading);
            
            Offers = await _marketTypesOfRequests.GET_Offers(transfer.playerTransferId);
            
            CurrentPlayerStatistics = await _team
                .GET_TeamPlayerStatistics(playerId);

            skills = await _team.GET_PlayerSkill(transfer.player.playerId);
            
            playerStatus = (TeamPlayerStatus)CurrentPlayerStatistics.status;
            
            //await _team.GET_TeamPlayers(false);

            if (CurrentPlayerStatistics == null)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.MarketMain);
                return;
            }
            
            _buildingWindowStatus.SetAction(StatusBuilding.MarketPlayer);
            _marketPlayerStatus.SetAction(StatusMarketPlayer.TraitsMarket);
            
            officePlayerHistory.ClearCache();
            officePlayerStatistics.ClearCache();
            
            _marketPlayerInfo.UpdateInfo();
            
            SetButtons(transfer);
        }

        private void SetButtons(MarketPlayerTransfer transfer)
        {
            if (OwnPlayer(transfer.player.playerId))
            {
                _marketPlayerButtonsOffer.ActiveButtons(false);
                _marketPlayerButtonsEdit.ActiveButtons(true);
            }
            else
            {
                _marketPlayerButtonsOffer.ActiveButtons(true);
                _marketPlayerButtonsEdit.ActiveButtons(false);
            }
        }
        
        private bool OwnPlayer(int playerId)
        {
            var player = _managerTeamData.teamPlayersList
                .Find(player => player.playerId == playerId);

            return player != null;
        }
    }
}