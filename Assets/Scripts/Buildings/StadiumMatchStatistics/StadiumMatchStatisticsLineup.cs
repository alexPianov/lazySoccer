using System.Collections.Generic;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchStatistics
{
    public class StadiumMatchStatisticsLineup : CenterTeamTable
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
        
        public  void OpenTeamList(List<PlayerGameStats> teamList)
        { 
            ServiceLocator.GetService<StadiumStatusMatchOld>().SetAction(StatusStadiumMatchOld.TeamLineup);
            
            var roster = GetTeamPlayers();
            
            var currentPlayers = new List<TeamPlayer>();

            foreach (var player in teamList)
            {
                var rosterPlayerName = player.playerName.Split(" ")[0];
                var rosterPlayer = roster.Find(teamPlayer => teamPlayer.name == rosterPlayerName);
                if (rosterPlayer == null) continue;
                currentPlayers.Add(rosterPlayer);
            }
            
            CreatePlayersList(currentPlayers);
            RefreshTeamCount(currentPlayers.Count, true);
        }
    }
}