using I2.Loc;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.SceneLoading.PlayerData.Enum;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MedicalCenter
{
    public class MedicalCenterMemberButtons : CenterMemberButtons
    {
        [Header("Refs")]
        [SerializeField] private MedicalCenterMessages messages;
        [SerializeField] private MedicalCenterMemberList memberList;
        
        public override void SetButtonsFunction(TeamPlayerStatus status)
        {
            if (status == TeamPlayerStatus.Healing)
            {
                //textInstant.text = "Instant Heal";
                textInstant.GetComponent<Localize>().SetTerm("3-General-Building-Instant-Heal");
                buttonInstant.onClick.AddListener(InstantHeal);
                buttonRemove.onClick.AddListener(CancelHealing);
            }
            
            if (status == TeamPlayerStatus.OnExamination)
            {
                //textInstant.text = "Instant Exam";
                textInstant.GetComponent<Localize>().SetTerm("3-General-Building-Instant-Exam");
                buttonInstant.onClick.AddListener(InstantExam);
                buttonRemove.onClick.AddListener(CancelExam);
            }
        }

        private async void InstantHeal()
        {
            var playerName = currentPlayer.GetTeamPlayer().name;

            string descrition = "Pay COST LAZY and PLAYER will get instant heal at once";
            
            var result = await _questionPopup.OpenQuestion(descrition, "Instant heal?",
                Param1: economy.instantHeal.ToString(), Param2: playerName);
            
            if(!result) return;

            SetButtonsInteracible(false);
            
            var success = await messages.InstantHeal(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }
            
            await messages.UpdateTeam();
            
            await UpdateBalanceLocally(economy.instantHeal);

            memberList.UpdateMemberList();
            
            _generalPopupMessage.ShowInfo("PLAYER was healed", Param1: playerName);
        }
        
        private async void CancelHealing()
        {
            var result = await _questionPopup.OpenQuestion
            ("All progress will be lost. Are you sure you want to remove this card?", 
                "Remove the player?");
            
            if(!result) return;

            SetButtonsInteracible(false);
            
            var playerName = currentPlayer.GetTeamPlayer().name;

            var success = await messages.CancelHealing(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }
            
            await messages.UpdateTeam();
            
            memberList.UpdateMemberList();
            
            _generalPopupMessage.ShowInfo("Healing was canceled for PLAYER", Param1: playerName);
        }
        
        private async void InstantExam()
        {
            var playerName = currentPlayer.GetTeamPlayer().name;

            string descrition = "Pay COST LAZY and PLAYER will get instant exam at once";
            
            var result = await _questionPopup.OpenQuestion(descrition, "Instant exam?",
                Param1: economy.instantHeal.ToString(), Param2: playerName);
            
            if(!result) return;
            
            SetButtonsInteracible(false);
            
            var success = await messages.InstantExaminate(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }
            
            await messages.UpdateTeam();
            
            await UpdateBalanceLocally(economy.instantExam);
            
            memberList.UpdateMemberList();
            
            _generalPopupMessage.ShowInfo
                ("PLAYER was examinated", Param1: playerName);
        }
        
        private async void CancelExam()
        {
            var playerName = currentPlayer.GetTeamPlayer().name;
            
            var result = await _questionPopup.OpenQuestion
            ("Examination progress will be lost", 
                "Are you sure?", "Cancel exam");
            
            if(!result) return;

            SetButtonsInteracible(false);
            
            var success = await messages.CancelExaminate(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }

            await messages.UpdateTeam();
            
            memberList.UpdateMemberList();

            _generalPopupMessage.ShowInfo("Examination was canceled for PLAYER", Param1: playerName);
        }
    }
}