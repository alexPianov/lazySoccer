using System;
using System.Collections.Generic;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using LazySoccer.Table;
using LazySoccer.Utils;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchStatistics
{
    public class StadiumMatchStatisticsReplaces : CenterSlotList
    {
        [SerializeField] private Button buttonClose;

        private void Start()
        {
            buttonClose.onClick.AddListener(Back);
        }

        private void Back()
        {
            ServiceLocator.GetService<StadiumStatusMatchOld>().SetAction(StatusStadiumMatchOld.Statistics);
        }

        public void SetTeamReplaces(List<PlayerGameStats> playerGameStatsList)
        {
            ServiceLocator.GetService<StadiumStatusMatchOld>().SetAction(StatusStadiumMatchOld.TeamReplaces);

            var Substiture = playerGameStatsList.FindAll(stats => stats.positionIndex == PositionIndex.Substitute);

            var Relegated = playerGameStatsList.FindAll(stats => stats.positionIndex == PositionIndex.Relegated);

            Debug.Log("Relegated players: " + Relegated.Count);
            
            if(Relegated.Count == 0) return;
            
            DestroyAllSlots();
            
            foreach (var relegatedPlayer in Relegated)
            {
                var relegatedPlayerPosition = DataUtils.GetSpecialityAbbreviation(relegatedPlayer.playerPosition);

                foreach (var substiturePlayer in Substiture)
                {
                    var substiturePlayerPosition = DataUtils.GetSpecialityAbbreviation(substiturePlayer.playerPosition);

                    if (substiturePlayerPosition == relegatedPlayerPosition)
                    {
                        var slot = CreateSlot();

                        if (slot.TryGetComponent(out StadiumMatchStatisticsReplacesSlot replacesSlot))
                        {
                            replacesSlot.SetInfo(substiturePlayer, relegatedPlayer);
                        }
                        
                        break;
                    }
                }
            }
        }
        
        public bool HasRelegatedPlayers(List<PlayerGameStats> playerGameStatsList)
        {
            var Relegated = playerGameStatsList.FindAll(stats => stats.positionIndex == PositionIndex.Relegated);

            Debug.Log("Relegated players: " + Relegated.Count);
            
            return Relegated.Count != 0;
        }
        

        private List<string> replaceList = new ();
        private ReplacePlayers GetReplace(List<PlayerGameStats> playerGameStatsList)
        {
            ReplacePlayers replace = new();
            
            replace.textNamePlayerFirst = "";

            return replace;
        }
        
        public struct ReplacePlayers
        {
            public string textNamePlayerFirst;
            public string textNamePlayerSecond;
        
            public string textGoalsPlayerFirst;
            public string textGoalsPlayerSecond;
        
            public string textPositionPlayerFirst;
            public string textPositionPlayerSecond;
        }
        
    }
}