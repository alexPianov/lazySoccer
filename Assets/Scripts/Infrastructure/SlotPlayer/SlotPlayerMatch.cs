using System;
using I2.Loc;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerMatch: MonoBehaviour
    {
        [Title("Text")]
        public TMP_Text matchFirstTeam;
        public TMP_Text matchFirstTeamDivision;
        public TMP_Text matchSecondTeam;
        public TMP_Text matchSecondTeamDivision;
        public TMP_Text matchDate;
        public TMP_Text matchScore;
        public TMP_Text matchBet;
        
        [Title("Image")]
        public Image matchFirstTeamImage;
        public Image matchSecondTeamImage;

        public string GameId { get; private set; }
        public string RequestId { get; private set; }
        
        public Match Match { get; private set; }
        public FriendshipRequest Request { get; private set; }
        
        public void SetInfo(Match match, bool hideCount = false)
        {
            Match = match;
            GameId = match.gameId;
            
            SetFirstTeam(match.hostTeam);
            SetSecondTeam(match.guestTeam);
            SetMatchDate(match.gameDate);
            
            if (hideCount)
            {
                SetMatchScore($"VS");
            }
            else
            {
                SetMatchScore($"{match.hostGoalFor}:{match.guestGoalFor}");
            }
        }

        public void ShowMatchScore()
        {
            SetMatchScore($"{Match.hostGoalFor}:{Match.guestGoalFor}");
        }

        public void SetInfo(FriendshipRequest request)
        {
            Debug.Log("Set Request: " + request.type + " id: " + request.requestId);
            RequestId = request.requestId;
            Request = request;

            SetFirstTeam(request.recipient.team);
            SetSecondTeam(request.sender.team);
            SetMatchDate(request.date);
            SetMatchBet(request.bet);
        }

        public void SetFirstTeam(Team team)
        {
            matchFirstTeam.text = team.name;
            if(matchFirstTeamDivision) matchFirstTeamDivision.text = DataUtils.GetDivisionDetailsInfo(team.division);
            var sprite = GetEmblemSprite(team.teamEmblem);
            if (sprite) matchFirstTeamImage.sprite = sprite;
        }

        public void SetSecondTeam(Team team)
        {
            matchSecondTeam.text = team.name;
            if(matchSecondTeamDivision) matchSecondTeamDivision.text = DataUtils.GetDivisionDetailsInfo(team.division);
            var sprite = GetEmblemSprite(team.teamEmblem);
            if (sprite) matchSecondTeamImage.sprite = sprite;
        }
        
        private Sprite GetEmblemSprite(Emblem emblem)
        {
            if (emblem == null) return null;

            return ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(emblem.emblemId);
        }

        public void SetMatchScore(string score)
        {
            if (matchScore) matchScore.text = score.ToString();
        }

        public void SetMatchDate(DateTime gameDate)
        {
            if (matchDate) matchDate.text = DataUtils.GetDate(gameDate);
        }

        public void SetMatchBet(int? bet)
        {
            if (matchBet == null) return;
            
            if (bet == null || bet == 0)
            {
                matchBet.GetComponent<Localize>().SetTerm("3-CommCenter-Matches-Slot-NoBet");
            }
            else
            {
                matchBet.GetComponent<Localize>().SetTerm("3-CommCenter-Matches-Slot-Bet");
                matchBet.GetComponent<LocalizationParamsManager>().SetParameterValue("param", bet.ToString());
            }
        }
    }
}