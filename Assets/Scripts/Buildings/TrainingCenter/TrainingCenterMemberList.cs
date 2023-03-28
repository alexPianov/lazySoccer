using LazySoccer.Network.Get;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class TrainingCenterMemberList: CenterMemberList
    {
        [SerializeField] private TrainingCenterMessages messages;
        [SerializeField] private BuildingInformation buildingInformation;
        [SerializeField] private TrainingCenterMemberSkill skillList;

        private void Start()
        {
            buildingInformation.updateLevel.AddListener(UpdateMemberList);
        }

        protected override void CreatePlayerList()
        {
            var restoringForm = CreatePlayersList
                (GeneralClassGETRequest.TeamPlayerStatus.Training, true);
            
            if(restoringForm != null && restoringForm.Count > 0) 
                playerSlots.AddRange(restoringForm);
        }

        protected override void CreateSlots()
        {
            var buildingLevel = buildingInformation.GetHouseInfo().Level.Value;

            var freeSlotsCount = GetMaxMember(buildingLevel) - playerSlots.Count;
            CreateFreeSlots(freeSlotsCount, StatusBuilding.TrainingCenterTeam);
            
            var pastLevelMaxMembers = GetMaxMember(buildingLevel - 1) - playerSlots.Count;
            buildingInformation.DisableDowngrade(pastLevelMaxMembers < 0);

            //CreateLockedSlots(maxMembers);
        }

        private int maxMembers = 6;
        private int GetMaxMember(int level)
        {
            if (level < 1) level = 1;
            
            switch (level)
            {
                case <= 4: return 1;
                case <= 9: return 2;
                case <= 14: return 3;
                case <= 19: return 4;
                case <= 20: return maxMembers;
                default: return 1;
            }
        }

        protected async override void UpdateMemberStatus(SlotPlayer slot)
        {
            var player = slot.GetTeamPlayer();
            var status = player.status;

            if (status == GeneralClassGETRequest.TeamPlayerStatus.Training)
            {
                _generalPopupMessage.ShowInfo("Player already STATUS", Param1: status.ToString());
                return;
            }

            if (status == GeneralClassGETRequest.TeamPlayerStatus.Healthy || 
                status == GeneralClassGETRequest.TeamPlayerStatus.Charged)
            {
                //var playerSkillId = await skillList.GetPlayerSkillId(player.playerId);
            
                //if (playerSkillId == null) return;

                _buildingWindowStatus.SetAction(StatusBuilding.QuickLoading);
                
                var success = await messages.SwitchTrainingMode(player.playerId);

                if (!success)
                {
                    _buildingWindowStatus.StatusAction = StatusBuilding.TrainingCenterTeam;
                    return;
                }
                
                player.status = GeneralClassGETRequest.TeamPlayerStatus.Training;
                await UpdateTeamPlayers();
            
                _buildingWindowStatus.StatusAction = StatusBuilding.TrainingCenterMain;

                _generalPopupMessage.ShowInfo("PLAYER started training", Param1: player.name);
                
                return;
            }
            
            _generalPopupMessage.ShowInfo("Player must be healthy for being a member");
        }
    }
}