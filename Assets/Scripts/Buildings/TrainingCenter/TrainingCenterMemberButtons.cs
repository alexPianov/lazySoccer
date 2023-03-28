using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class TrainingCenterMemberButtons : CenterMemberButtons
    {
        [Header("Refs")]
        [SerializeField] private TrainingCenterMessages messages;
        [SerializeField] private TrainingCenterMemberList memberList;
        [SerializeField] private TrainingCenterMemberSkill memberSkill;
        
        public override void SetButtonsFunction(GeneralClassGETRequest.TeamPlayerStatus status)
        {
            if (status == GeneralClassGETRequest.TeamPlayerStatus.Training)
            {
                //textInstant.text = "Instant Training";
                textInstant.GetComponent<Localize>().SetTerm("3-General-Building-Instant-Training");
                buttonInstant.onClick.AddListener(InstantTraining);
                buttonRemove.onClick.AddListener(CancelTraining);
            }
        }

        private async void InstantTraining()
        {
            var playerForm = currentPlayer.GetTeamPlayer().form;
            var playerName = currentPlayer.GetTeamPlayer().name;

            var price = economy.instantTraining;
            
            Debug.Log("Player Training: " + playerForm);

            if (playerForm < 100)
            {
                price = economy.instantForm;
            }

            string description = "Pay COST LAZY and PLAYER will get instant training at once";
            
            var result = await _questionPopup.OpenQuestion(description, "Instant training?",
                Param1: price.ToString(), Param2: playerName);
            
            if(!result) return;
            
            SetButtonsInteracible(false);

            //var currentSkill = await memberSkill.GetUpdatedSkill(currentPlayerId, AdditionClassGetRequest.SkillStatus.TrainingImproving);

            var success = await messages.InstantTraining(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }
            
            await messages.UpdateTeam();

            await UpdateBalanceLocally(economy.instantForm);
                
            _generalPopupMessage.ShowInfo("PLAYER was trained", Param1: playerName);
            
            memberList.UpdateMemberList();
        }
        
        private async void CancelTraining()
        {
            var result = await _questionPopup.OpenQuestion
            ("Training progress will be lost", 
                "Are you sure?", "Cancel training");
            
            if(!result) return;
            
            var playerName = currentPlayer.GetTeamPlayer().name;
            
            SetButtonsInteracible(false);

            //var currentSkill = await memberSkill.GetUpdatedSkill(currentPlayerId, AdditionClassGetRequest.SkillStatus.TrainingImproving);

            var success = await messages.CancelTrainingMode(currentPlayerId);
            
            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }

            await messages.UpdateTeam();
            
            memberList.UpdateMemberList();
            
            _generalPopupMessage.ShowInfo("Training was canceled for PLAYER", Param1: playerName);
        }
    }
}