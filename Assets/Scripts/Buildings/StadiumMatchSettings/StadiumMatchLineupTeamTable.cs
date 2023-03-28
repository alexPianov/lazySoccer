using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchSettings
{
    public class StadiumMatchLineupTeamTable : CenterTeamTable
    {
        public TeamRosterManager teamRosterManager;
        public PlayerPlaceholdersStorage PlaceholdersStorage;
        
        public async UniTask CreateLineupTable(List<TeamPlayer> teamPlayers)
        {
            cancel = false;
            SlotPlayerLocal = null;
            
            var players = CreatePlayersList(teamPlayers);

            RefreshTeamCount(players.Count);
            
            AddListener();
            
            await UniTask.WaitUntil(() => SlotPlayerLocal || cancel);

            // if (SlotPlayerLocal.GetTeamPlayer().status != TeamPlayerStatus.Healthy ||
            //     SlotPlayerLocal.GetTeamPlayer().status != TeamPlayerStatus.Charged)
            // {
            //     SlotPlayerLocal = null;
            //     return;
            // }
            
            cancel = false;

            if (SlotPlayerLocal)
            {
                teamRosterManager.SetSelectedPlayerId(SlotPlayerLocal.GetTeamPlayer().playerId);
                PlaceholdersStorage.SetPlayer();
            }
        }

        private bool cancel = false;
        public void Cancel()
        {
            cancel = true;
        }

    }
}