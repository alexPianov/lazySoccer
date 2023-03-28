using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Status;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivision
{
    public class StadiumDivision : MonoBehaviour
    {
        public List<Match> MatchesUpcoming = new();
        public List<DivisionTournament> MatchesStatistics = new();
        public Tournament Tournament;
        public DivisionStadium DivisionStadium { get; set; }

        [SerializeField] private StadiumDivisionUpcomingMatches.StadiumDivisionSchedule ScheduleMatches;
        [SerializeField] private StadiumDivisionGroupStandings.StadiumDivisionGroupStandings GroupStandings;
        
        
        public async void SetDivisionInfo(DivisionStadium divisionStadium, Tournament tournament, int countryId)
        {
            Debug.Log("SetDivisionInfo: " + tournament + " | CountryId: " + countryId);
            
            Tournament = tournament;
            DivisionStadium = divisionStadium;
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);

            var currentSeason = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Season;

            if (tournament == Tournament.National_League)
            {
                MatchesUpcoming = await ServiceLocator
                    .GetService<StadiumTypesOfRequests>()
                    .GetNationalLeague(divisionStadium.divisionId, currentSeason);

                MatchesStatistics = await ServiceLocator
                    .GetService<StadiumTypesOfRequests>()
                    .GetNationalLeagueStatistic(divisionStadium.divisionId,  currentSeason);
            }
            
            if (tournament == Tournament.National_Cup)
            {
                MatchesUpcoming = await ServiceLocator
                    .GetService<StadiumTypesOfRequests>()
                    .GetNationalCup(countryId, currentSeason);

                MatchesStatistics = await ServiceLocator
                    .GetService<StadiumTypesOfRequests>()
                    .GetNationalCupStatistics(countryId, currentSeason);
            }
            
            ScheduleMatches.UpdateScheduleMatches();
            GroupStandings.UpdateGroupStandings();

            ServiceLocator.GetService<BuildingWindowStatus>()
                .SetAction(StatusBuilding.StadiumTournaments);
            
            ServiceLocator.GetService<StadiumStatusTournament>()
            .SetAction(StatusStadiumTournament.NationalUpcomingMatches);
        }
    }
}