using LazySoccer.Network.Get;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumDivisionGroupStandingsSlot : MonoBehaviour
    {
        [SerializeField] private Image imageTeamEmblem;
        [SerializeField] private TMP_Text textTeamName;
        [SerializeField] private TMP_Text textP;
        [SerializeField] private TMP_Text textW;
        [SerializeField] private TMP_Text textD;
        [SerializeField] private TMP_Text textL;
        [SerializeField] private TMP_Text textGF;
        [SerializeField] private TMP_Text textGA;
        [SerializeField] private TMP_Text textPTS;

        public void SetInfo(DivisionTournament tournament)
        {
            imageTeamEmblem.sprite = GetEmblemSprite(tournament.team.teamEmblem);
            textTeamName.text = tournament.team.name;
            textP.text = tournament.matchPlayed.ToString();
            textW.text = tournament.wins.ToString();
            textD.text = tournament.draws.ToString();
            textL.text = tournament.loses.ToString();
            textGF.text = tournament.goalFor.ToString();
            textGA.text = tournament.goalAgainst.ToString();
            textPTS.text = tournament.pts.ToString();
        }
        
        private Sprite GetEmblemSprite(Emblem emblem)
        {
            if (emblem == null) return null;

            return ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(emblem.emblemId);
        }
        
    }
}