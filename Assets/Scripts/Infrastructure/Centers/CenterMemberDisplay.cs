using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberDisplay : SlotPlayer
    {
        [Title("Display")]
        [SerializeField] protected GameObject playerPanel;

        [Title("Buttons")] 
        [SerializeField] private CenterMemberButtons memberButtons;

        public void SetMember(SlotPlayer slotPlayer)
        {
            memberButtons.currentPlayer = slotPlayer;
            
            playerPanel.SetActive(slotPlayer);

            memberButtons.SetButtonsInteracible(slotPlayer);
            
            if (slotPlayer)
            {
                var teamPlayer = slotPlayer.GetTeamPlayer();
                memberButtons.currentPlayerId = teamPlayer.playerId;
                
                SetInfo(teamPlayer);
                
                memberButtons.RemoveButtonListeners();
                memberButtons.SetButtonsFunction(teamPlayer.status);
            }
        }
    }
}