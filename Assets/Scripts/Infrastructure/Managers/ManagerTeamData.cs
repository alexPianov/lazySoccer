using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading
{
    public class ManagerTeamData : MonoBehaviour
    {
        public TeamStatistic teamStatisticData { get; private set; }
        public List<TeamPlayer> teamPlayersList { get; private set; }
        
        public List<TeamReward> teamRewardList { get; private set; }
        public List<TeamTrophy> teamTrophiesList { get; private set; }
        
        public List<FinancialStatistics> seasonBalance { get; private set; }
        public List<FinancialStatistics> pastDayBalance { get; private set; }

        public void SetTeamStatistic(TeamStatistic requestedTeam)
        {
            teamStatisticData = requestedTeam;
        }

        public void SetTeamRewards(List<TeamReward> rewards)
        {
            teamRewardList = rewards;
        }
        
        public void SetTeamTrophies(List<TeamTrophy> trophies)
        {
            teamTrophiesList = trophies;
        }

        public void SetTeamPlayers(List<TeamPlayer> players)
        {
            teamPlayersList = players;
        }
        
        public void SetSeasonBalance(List<FinancialStatistics> financialList)
        {
            seasonBalance = financialList;
        }
        
        public void SetPastDayBalance(List<FinancialStatistics> financialList)
        {
            pastDayBalance = financialList;
        }
        
        public List<TeamPlayer> GetTeamPlayers(TeamPlayerStatus requiredStatus = TeamPlayerStatus.None)
        {
            if (requiredStatus == TeamPlayerStatus.None)
            {
                return teamPlayersList;
            }
        
            return teamPlayersList.FindAll(player => player.status == requiredStatus);
        }
        
        
        public TeamPlayer GetTeamPlayer(int playerId)
        {
            return teamPlayersList.Find(player => player.playerId == playerId);
        }
    }
}