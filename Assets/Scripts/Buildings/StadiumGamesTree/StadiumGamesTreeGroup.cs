using System.Collections.Generic;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumGamesTreeGroup : MonoBehaviour
    {
        public MatchStep MatchStep;
        [SerializeField] private List<SlotPlayerMatch> matchSlots;

        public void SetMatchData(List<Match> matches)
        {
            ActiveGroupSlots(matches.Count > 0);
            
            foreach (var match in matches)
            {
                var emptySlot = matchSlots.Find(playerMatch => playerMatch.Match == null);
                
                if(emptySlot) emptySlot.SetInfo(match);
            }
        }

        private void ActiveGroupSlots(bool state)
        {
            foreach (var matchSlot in matchSlots)
            {
                matchSlot.gameObject.SetActive(state);
            }
        }
        
    }
}