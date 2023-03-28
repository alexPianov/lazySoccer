using I2.Loc;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class FitnessCenterMemberButtons : CenterMemberButtons
    {
        [Header("Refs")]
        [SerializeField] private FitnessCenterMessages messages;
        [SerializeField] private FitnessCenterMemberList memberList;
        
        public override void SetButtonsFunction(TeamPlayerStatus status)
        {
            if (status == TeamPlayerStatus.RestoringForm)
            {
                textInstant.GetComponent<Localize>().SetTerm("3-General-Building-Instant-Charge");
                buttonInstant.onClick.AddListener(InstantCharge);
                buttonRemove.onClick.AddListener(CancelCharge);
            }
        }

        private async void InstantCharge()
        {
            var playerForm = currentPlayer.GetTeamPlayer().form;
            var playerName = currentPlayer.GetTeamPlayer().name;

            var price = economy.instantCharge;
            
            Debug.Log("playerForm: " + playerForm);

            if (playerForm < 100)
            {
                price = economy.instantForm;
            }
            
            string descrition = "Pay COST LAZY and PLAYER will get instant charge at once";
            
            var result = await _questionPopup.OpenQuestion
                (descrition, "Instant charge?", Param1: price.ToString(), Param2: playerName);
            
            if(!result) return;
            
            SetButtonsInteracible(false);
            
            var success = await messages.InstantRestoreForm(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }
            
            await messages.UpdateTeam();

            if (playerForm < 100)
            {
                await UpdateBalanceLocally(economy.instantForm);
                
                _generalPopupMessage.ShowInfo("PLAYER was charged", Param1: playerName);
            }
            else
            {
                await UpdateBalanceLocally(economy.instantCharge);
                
                _generalPopupMessage.ShowInfo("PLAYER was fully charged", Param1: playerName);
            }
            
            memberList.UpdateMemberList();
        }
        
        private async void CancelCharge()
        {
            var result = await _questionPopup.OpenQuestion
            ("Charging progress will be lost", 
                "Are you sure?", "Cancel charging");
            
            if(!result) return;
            
            var playerName = currentPlayer.GetTeamPlayer().name;
            
            SetButtonsInteracible(false);

            var success = await messages.CancelRestoringForm(currentPlayerId);

            if (!success)
            {
                SetButtonsInteracible(true);
                return;
            }

            await messages.UpdateTeam();
            
            memberList.UpdateMemberList();
            
            _generalPopupMessage.ShowInfo("Charging was canceled for PLAYER", Param1: playerName);
        }
    }
}