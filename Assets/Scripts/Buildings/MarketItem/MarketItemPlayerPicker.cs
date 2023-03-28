using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketItemPlayerPicker : MonoBehaviour
    {
        [SerializeField] private CenterTeamTable marketTeam;
        [SerializeField] private GameObject panelRosterList;
        [SerializeField] private MarketItemPlayerSkillPicker skillPicker;
        
        public async UniTask<string> GetChosenId(MarketInventory inventoryItem, StatusMarketPopup popup)
        {
            Debug.Log("Loot type: " + inventoryItem.lootBox.loot);
            
            if (inventoryItem.lootBox.loot == LootName.MiracleMedicine)
            {
                var player = await GetTeamPlayer(popup);
                if (player == null) return null;
                Debug.Log("Player id: " + player.playerId.ToString());
                return player.playerId.ToString();
            }

            if (inventoryItem.lootBox.loot == LootName.IntensiveTrainingProgramme)
            {
                var player = await GetTeamPlayer(popup);
                if (player == null) return null;
                    
                Debug.Log("Player id: " + player.playerId.ToString());
                var skillId = await skillPicker.GetPlayerSkillId(player.playerId);
                if (skillId == null) return null;
                
                Debug.Log("Skill id: " + skillId.ToString());
                return skillId.ToString();
            }

            if (inventoryItem.lootBox.loot == LootName.StaffIncentivesProgramme ||
                inventoryItem.lootBox.loot == LootName.StaffTrainingProgramme)
            {
                Debug.LogError("The feature is not ready yet");
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Not ready yet");
                
                return "";
            }

            if (inventoryItem.lootBox.loot == LootName.ScoutInsight)
            {
                Debug.LogError("The feature is not ready yet");
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Not ready yet");
                
                return "";
            }
            
            return "";
        }
        
        public async UniTask<TeamPlayer> GetTeamPlayer(StatusMarketPopup statusOnFinish = StatusMarketPopup.None)
        {
            cancel = false;
            
            marketTeam.SlotPlayerLocal = null;
            
            panelRosterList.SetActive(true);
            
            marketTeam.CreateTable();

            await UniTask.WaitUntil(() => marketTeam.SlotPlayerLocal || cancel);

            if (cancel)
            {
                cancel = false;
                ServiceLocator.GetService<MarketPopupStatus>().SetAction(statusOnFinish);
                return null;
            }

            cancel = false;
            
            panelRosterList.SetActive(false);
            TeamPlayer teamPlayer = null;
            
            if (marketTeam.SlotPlayerLocal)
            {
                teamPlayer = marketTeam.SlotPlayerLocal.GetTeamPlayer();
                marketTeam.SlotPlayerLocal = null;
            }
            
            ServiceLocator.GetService<MarketPopupStatus>().SetAction(statusOnFinish);

            return teamPlayer;
        }
        
        public async UniTask<TeamPlayer> GetTeamPlayer()
        {
            return marketTeam.SlotPlayerLocal.GetTeamPlayer();
        }

        private bool cancel;
        public void Close()
        {
            cancel = true;
            panelRosterList.SetActive(false);
        }
    }
}