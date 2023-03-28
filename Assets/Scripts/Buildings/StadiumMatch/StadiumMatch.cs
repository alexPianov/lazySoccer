using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.Stadium;
using LazySoccer.Status;
using LazySoccer.Windows;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatch
{
    public class StadiumMatch : MonoBehaviour
    {
        [SerializeField] private StadiumSlotMatch _stadiumSlotMatch;
        [SerializeField] private TMP_Text textTeamHost;
        [SerializeField] private TMP_Text textTeamGuest;
        [SerializeField] private StadiumMatchReplay stadiumMatchReplay;

        [SerializeField] private MatchSettingsManager matchSettings;

        public MatchStatistics MatchStatistics { get; private set; }
        public Match Match { get; private set; }
        public async void SetMatchHistory(Match match)
        {
            ServiceLocator.GetService<BuildingWindowStatus>().SetAction(StatusBuilding.QuickLoading);
            
            Match = match;
            
            MatchStatistics = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetMatch(match.gameId);

            await stadiumMatchReplay.ShowMatch(match.gameId);

            _stadiumSlotMatch.SetInfo(match, true);

            //textTeamHost.text = match.hostGoalFor.ToString();
            //textTeamGuest.text = match.guestGoalFor.ToString();
            
            ServiceLocator.GetService<BuildingWindowStatus>().SetAction(StatusBuilding.StadiumMatchHistory);
            ServiceLocator.GetService<StadiumStatusMatchOld>().SetAction(StatusStadiumMatchOld.Replay);
        }

        public void SetMatchFuture(Match match)
        {
            ServiceLocator.GetService<StadiumStatus>().SetAction(StatusStadium.MatchSettings);

            matchSettings.GetMatchData(match);
        }

        public void UpdateGoalCount(int hostGoals, int guestGoals)
        {
            textTeamHost.text = hostGoals.ToString();
            textTeamGuest.text = guestGoals.ToString();
        }
    }
}