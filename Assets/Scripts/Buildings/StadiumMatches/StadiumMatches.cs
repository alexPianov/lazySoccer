using System.Collections.Generic;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumMatches : CenterSlotList
    {
        [Header("Title")]
        public Tournament Tournament;
        public MatchType Type;
        
        [Header("Refs")]
        public TMP_Dropdown dropdownSeason; 
        
        [SerializeField] private StadiumMatch.StadiumMatch _stadiumMatchDisplay;

        private int season = 2;
        private int dropdownValue = 1;
        public enum MatchType
        {
            History, Upcoming
        }

        private void Start()
        {
            season = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Season;
            Debug.Log("season: " + season);
            if (dropdownSeason)
            {
                dropdownSeason.onValueChanged.AddListener(SetSeason);
                dropdownSeason.SetValueWithoutNotify(season - 1);
            }  
        }

        private void SetSeason(int value)
        {
            Debug.Log("SetSeason: " + value);
            season = value + 1;
            UpdateList();
        }

        public List<string> openedMatches = new();
        [ContextMenu("Update List")]
        public async void UpdateList()
        {
            var matchList = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetMatches(Type, Tournament, season);
 
            DestroyAllSlots();

            DisableScrollRect(matchList == null || matchList.Count == 0);
            
            foreach (var match in matchList)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumSlotMatch matchSlot))
                {
                    Debug.Log("Match: " + match.gameId + " | Status " + match.status + " | " + match.category);
                    
                    if (openedMatches.Contains(match.gameId))
                    {
                        matchSlot.SetInfo(match, false);
                    }
                    else
                    {
                        matchSlot.SetInfo(match, true);
                    }
                    
                    matchSlot.SetAttendance(match.attendance);
                    matchSlot.SetMaster(this);
                    matchSlot.SetMatchType(Type);
                    matchSlot.SetTournament(match.category);
                    matchSlot.CheckMatchLineup(match);
                }
            }
        }

        public void ShowMatchInfo(Match match, MatchType matchType)
        {
            Debug.Log("ShowMatchInfo: " + match.gameId);
            
            if (matchType == MatchType.Upcoming)
            {
                _stadiumMatchDisplay.SetMatchFuture(match);
            }
            
            if (matchType == MatchType.History)
            {
                _stadiumMatchDisplay.SetMatchHistory(match);
                openedMatches.Add(match.gameId);
            }
        }
    }
}