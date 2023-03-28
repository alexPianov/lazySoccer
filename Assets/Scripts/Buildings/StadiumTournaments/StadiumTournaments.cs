using System;
using I2.Loc;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumTournaments : MonoBehaviour
    {
        [SerializeField] private StadiumDivisions.StadiumDivisions stadiumDivisions;
        [SerializeField] private StadiumGamesTree stadiumGamesTree;
        [SerializeField] private StadiumFriendlyMatches.StadiumFriendlyMatches stadiumFriendlyMatches;

        [SerializeField] private TMP_Text textTournamentName;
        
        private BuildingWindowStatus _buildingWindowStatus;
        private StadiumStatusTournament _stadiumStatusTournament;
        private void Start()
        {
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _stadiumStatusTournament = ServiceLocator.GetService<StadiumStatusTournament>();
        }

        private Tournament _currentTournament;

        public void Reopen()
        {
            if (stadiumDivisions.dontReopen)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.StadiumMain);
                return;
            }
            
            OpenTournament(_currentTournament);
        }

        public async void OpenTournament(Tournament tournament)
        {
            Debug.Log("OpenTournament: " + tournament);
            
            _currentTournament = tournament;

            textTournamentName.GetComponent<Localize>().SetTerm("Stadium-" + tournament);

            if (tournament == Tournament.National_Cup || tournament == Tournament.National_League)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.StadiumTournaments);
                _stadiumStatusTournament.SetAction(StatusStadiumTournament.NationalCountries);
                _buildingWindowStatus.OpenQuickLoading(true);

                await stadiumDivisions.SetTournament(tournament);
                
                //_buildingWindowStatus.SetAction(StatusBuilding.StadiumTournaments);
                //_stadiumStatusTournament.SetAction(StatusStadiumTournament.NationalCountries);
            }

            if (tournament == Tournament.Champions_Cup || tournament == Tournament.Leaders_Cup)
            {
                _buildingWindowStatus.OpenQuickLoading(true);
                await stadiumGamesTree.SetTournament(tournament);
                
                _buildingWindowStatus.SetAction(StatusBuilding.StadiumTournaments);
                _stadiumStatusTournament.SetAction(StatusStadiumTournament.GamesTree);
                
                stadiumGamesTree.SetGroup(MatchStep.one_4);
                stadiumGamesTree.SetGroup(MatchStep.Semifinal);
                stadiumGamesTree.SetGroup(MatchStep.Final);
            }
            
            if (tournament == Tournament.Friendly)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.StadiumTournaments);
                _stadiumStatusTournament.SetAction(StatusStadiumTournament.FriendlyMatches);
            }
        }
    }
}