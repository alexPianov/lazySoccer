using System;
using System.Linq;
using I2.Loc;
using LazySoccer.SceneLoading.Buildings.Stadium;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivisionUpcomingMatches
{
    public class StadiumDivisionSchedule : CenterSlotList
    {
        [SerializeField] private StadiumDivision.StadiumDivision StadiumDivision;
        [SerializeField] private GameObject prefabTourTitle;

        private int matchNumber;
        private int currentTour;
        private const int nextTourTitle = 8;
        public void UpdateScheduleMatches()
        {
            DestroyAllSlots();

            ScrollbarVerticalReset();
            
            currentTour = 1;
            matchNumber = 0;

            var gamesGroup = StadiumDivision.MatchesUpcoming
                .GroupBy(p => p.gameDate.Date);

            foreach (var group in gamesGroup)
            {
                Debug.Log("--- group: " + group.Key);
                
                CreateTourTitle();
                
                foreach (var match in group)
                {
                    Debug.Log("match: " + match.gameDate);
                    
                    var slotInstance = CreateSlot();

                    if (slotInstance.TryGetComponent(out StadiumSlotMatch slot))
                    {
                        slot.SetInfo(match, true);
                        slot.SetTournament(StadiumDivision.Tournament);
                    }
                }
                
                matchNumber++;

                if (matchNumber >= nextTourTitle) matchNumber = 0;
            }
            
            return;

            foreach (var match in StadiumDivision.MatchesUpcoming)
            {
                if (matchNumber == 0) CreateTourTitle(); 
                
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumSlotMatch slot))
                {
                    slot.SetInfo(match, true);
                    slot.SetTournament(StadiumDivision.Tournament);
                }
                
                matchNumber++;

                if (matchNumber >= nextTourTitle) matchNumber = 0;
            }
        }

        private void Output()
        {
            
        }

        private void CreateTourTitle()
        {
            var slotTour = CreateSlot(prefabTourTitle);
                    
            slotTour.GetComponentInChildren<LocalizationParamsManager>()
                .SetParameterValue("param", currentTour.ToString());
            
            currentTour++;
        }
        
    }
}