using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MedicalCenter
{
    public class MedicalCenterMemberList : CenterMemberList
    {
        [SerializeField] private MedicalCenterMessages messages;
        [SerializeField] private BuildingInformation buildingInformation;
        
        private void Start()
        {
            buildingInformation.updateLevel.AddListener(UpdateMemberList);
        }
        
        protected override void CreatePlayerList()
        {
            var healing = CreatePlayersList(TeamPlayerStatus.Healing, true);
            var onExamination = CreatePlayersList(TeamPlayerStatus.OnExamination, true);
            
            if(healing != null && healing.Count > 0) playerSlots.AddRange(healing);
            if(onExamination != null && onExamination.Count > 0) playerSlots.AddRange(onExamination);
        }

        protected override void CreateSlots()
        {
            var buildingLevel = buildingInformation.GetHouseInfo().Level.Value;
            var freeSlotsCount = GetMaxMember(buildingLevel) - playerSlots.Count;
            CreateFreeSlots(freeSlotsCount, StatusBuilding.MedicalCenterTeam);
            
            var pastLevelMaxMembers = GetMaxMember(buildingLevel - 1) - playerSlots.Count;
            buildingInformation.DisableDowngrade(pastLevelMaxMembers < 0);
            
            //CreateLockedSlots(maxMembers);
        }

        private int maxMembers = 24;
        private int GetMaxMember(int level)
        {
            if (level < 1) level = 1;
            
            switch (level)
            {
                case <= 4: return 2;
                case <= 9: return 3;
                case <= 14: return 6;
                case <= 19: return 12;
                case <= 20: return maxMembers;
                default: return 2;
            }
        }

        protected async override void UpdateMemberStatus(SlotPlayer slot)
        {
            var player = slot.GetTeamPlayer();
            var status = player.status;
            
            if (status == TeamPlayerStatus.Healing || status == TeamPlayerStatus.OnExamination)
            {
                _generalPopupMessage.ShowInfo("Player already STATUS", Param1: status.ToString());
                return;
            }

            if (status == TeamPlayerStatus.NotHealing)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.QuickLoading);
                
                var success = await messages.Heal(player.playerId);

                if (!success)
                {
                    _buildingWindowStatus.SetAction(StatusBuilding.MedicalCenterTeam);
                    return;
                }

                player.status = TeamPlayerStatus.Healing;
                await UpdateTeamPlayers();
                _generalPopupMessage.ShowInfo("PLAYER started healing", Param1: player.name);
            
                _buildingWindowStatus.StatusAction = StatusBuilding.MedicalCenterMain;
                return;
            }
            
            if (status == TeamPlayerStatus.Healthy)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.QuickLoading);
                
                var success = await messages.Examinate(player.playerId);

                if (!success)
                {
                    _buildingWindowStatus.SetAction(StatusBuilding.MedicalCenterTeam);
                    return;
                }
                
                player.status = TeamPlayerStatus.OnExamination;
                await UpdateTeamPlayers();
                _generalPopupMessage.ShowInfo("PLAYER started exam", Param1: player.name);
            
                _buildingWindowStatus.StatusAction = StatusBuilding.MedicalCenterMain;
                return;
            }
            
            _generalPopupMessage.ShowInfo("Player must be not healing or healthy for being a member");
        }
    }
}