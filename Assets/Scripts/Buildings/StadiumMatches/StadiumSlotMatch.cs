using System;
using I2.Loc;
using LazySoccer.Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.SceneLoading.Buildings.Stadium.StadiumMatches;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumSlotMatch : SlotPlayerMatch
    {
        [SerializeField] private TMP_Text textAttendance;
        [SerializeField] private TMP_Text textTournament;
        [SerializeField] private TMP_Text textLineupStatus;
        [SerializeField] private Image imageLineupStatus;
        [SerializeField] private Sprite[] spriteLineupStatuses;

        public bool showScore;
 
        private void Start()
        {
            if (!GetComponent<Button>()) return;
            
            if (showScore)
            {
                GetComponent<Button>().onClick.AddListener(ShowScore);
            }
            else
            {
                GetComponent<Button>().onClick.AddListener(ShowInfo);
            }
        }

        public void SetAttendance(double attendance)
        {
            var attenanceDisplay = Mathf.Round((float)attendance * 100);
            Debug.Log("attenanceDisplay: " + attenanceDisplay);
            if (textAttendance && textAttendance.GetComponent<LocalizationParamsManager>())
            {
                textAttendance.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"{attenanceDisplay}%");
                return;
            }
            
            if (textAttendance) textAttendance.text = $"{attenanceDisplay}%";
        }

        public void SetTournament(Tournament tournament)
        {
            if(textTournament) textTournament
                .GetComponent<Localize>().SetTerm($"Stadium-{tournament.ToString()}");
        }

        [SerializeField]
        private StadiumMatches _stadiumMatchesOld;
        public void SetMaster(StadiumMatches matchesOld)
        {
            _stadiumMatchesOld = matchesOld;
        }
        
        [SerializeField]
        private StadiumMatchesAll _stadiumMatchesAll;
        public void SetMaster(StadiumMatchesAll matches)
        {
            _stadiumMatchesAll = matches;
        }

        private MatchType _matchType;
        
        public void SetMatchType(MatchType matchType)
        {
            _matchType = matchType;
        }

        private void ShowInfo()
        {
            if (_matchType == MatchType.Upcoming && !waitingForLineup)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Players already placed on the field");
                return;
            }

            if (_matchType == MatchType.History)
            {
                ShowMatchScore();
            }

            if (_stadiumMatchesOld)
            {
                _stadiumMatchesOld.ShowMatchInfo(Match, _matchType);
            }

            if (_stadiumMatchesAll)
            {
                _stadiumMatchesAll.ShowMatchInfo(Match, _matchType);
            }
        }

        [SerializeField]
        private bool waitingForLineup;
        public void CheckMatchLineup(Match match)
        {
            var teamId = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.TeamId;
            
            if(!textLineupStatus || !textLineupStatus.GetComponent<Localize>() || imageLineupStatus == null) return;
            
            if (match.hostTeam.teamId == teamId)
            {
                waitingForLineup = match.hostGameStatus == GameStatus.WaitingForLineup;
            }
            
            if (match.guestTeam.teamId == teamId)
            {
                waitingForLineup = match.guestGameStatus == GameStatus.WaitingForLineup;
            }

            if (waitingForLineup)
            {
                textLineupStatus.GetComponent<Localize>().SetTerm("Stadium-General-Lineup");
                imageLineupStatus.sprite = spriteLineupStatuses[0];
            }
            else
            {
                textLineupStatus.GetComponent<Localize>().SetTerm("Stadium-General-LineupReady");
                imageLineupStatus.sprite = spriteLineupStatuses[1];
            }
        }

        private void ShowScore()
        {
            ShowMatchScore();
        }
    }
}