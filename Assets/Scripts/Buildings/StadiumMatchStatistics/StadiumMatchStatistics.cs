using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchStatistics
{
    public class StadiumMatchStatistics : MonoBehaviour
    {
        [SerializeField] public StadiumMatch.StadiumMatch StadiumMatch;
        [SerializeField] public StadiumMatchStatisticsRow statisticsHost;
        [SerializeField] public StadiumMatchStatisticsRow statisticsGuest;

        public void UpdateStatistics()
        {
            var matchInfo = StadiumMatch.MatchStatistics;
            
            if(matchInfo.teamGameStats == null) {Debug.LogError("Failed to find match teamGameStats");}
            
            var hostTeamStat = matchInfo.teamGameStats.Find(stats => stats.teamId == StadiumMatch.Match.hostTeam.teamId);
            var hostPlayersStat = matchInfo.playerGameStats.FindAll(stats => stats.teamId == StadiumMatch.Match.hostTeam.teamId);
            if(hostTeamStat == null) {Debug.LogError("Failed to find host team stat");}
            statisticsHost.SetStatistics(hostTeamStat, hostPlayersStat);
            
            var guestTeamStat = matchInfo.teamGameStats.Find(stats => stats.teamId == StadiumMatch.Match.guestTeam.teamId);
            var guestPlayersStat = matchInfo.playerGameStats.FindAll(stats => stats.teamId == StadiumMatch.Match.guestTeam.teamId);
            if(guestTeamStat == null) {Debug.LogError("Failed to find guest team stat");}
            statisticsGuest.SetStatistics(guestTeamStat, guestPlayersStat);
        }
    }
}