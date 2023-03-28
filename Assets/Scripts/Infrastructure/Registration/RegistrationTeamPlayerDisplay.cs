using LazySoccer.Table;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamPlayerDisplay : SlotPlayer
    {
        private SlotPlayer currentPlayer;
        
        public void SetPlayer(SlotPlayer slotPlayer)
        {
            currentPlayer = slotPlayer;
            
            if (slotPlayer)
            {
                var teamPlayer = slotPlayer.GetTeamPlayer();
                SetInfo(teamPlayer);
            }
        }
    }
}