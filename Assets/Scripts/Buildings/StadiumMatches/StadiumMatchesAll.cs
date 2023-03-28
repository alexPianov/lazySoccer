using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.SceneLoading.Buildings.Stadium.StadiumMatches;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumMatchesAll : CenterSlotList
    {
        [SerializeField] private StadiumMatch.StadiumMatch _stadiumMatchDisplay;
        [SerializeField] private GameObject prefabMatchFuture;
        [SerializeField] private GameObject prefabMatchHistory;

        public List<string> openedMatches = new();
        [ContextMenu("Update List")]
        public async void UpdateList()
        {
            DestroyAllSlots();
            
            await CreateMatchList(MatchType.Upcoming, prefabMatchFuture);
            await CreateMatchList(MatchType.History, prefabMatchHistory);
        }

        private async UniTask CreateMatchList(MatchType matchType, GameObject slotPrefab)
        {
            var matchList = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetMatches(matchType);

            foreach (var match in matchList)
            {
                var slotInstance = CreateSlot(slotPrefab);

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
                    matchSlot.SetMatchType(matchType);
                    matchSlot.SetMaster(this);
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