using System;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;
using static StatusStadiumTournament;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumGamesTreeDetail : CenterSlotList
    {
        [SerializeField] private StadiumGamesTree stadiumGamesTree;
        
        private BuildingWindowStatus _buildingWindowStatus;
        private StadiumStatusTournament _stadiumStatusTournament;
        
        private void Start()
        {
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _stadiumStatusTournament = ServiceLocator.GetService<StadiumStatusTournament>();
        }
        
        public void OpenGroup(AdditionClassGetRequest.MatchStep matchStep)
        {
            ServiceLocator.GetService<StadiumStatusTournament>().SetAction(GamesTreeDetail);
            
            var matches = stadiumGamesTree.Matches.FindAll(match => match.match == matchStep);

            DestroyAllSlots();

            foreach (var match in matches)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumSlotMatch slotMatch))
                {
                    slotMatch.SetInfo(match);
                }
            }
        }

        public void CloseDetails()
        {
            _buildingWindowStatus.SetAction(StatusBuilding.StadiumTournaments);
            _stadiumStatusTournament.SetAction(StatusStadiumTournament.GamesTree);
        }
    }
}