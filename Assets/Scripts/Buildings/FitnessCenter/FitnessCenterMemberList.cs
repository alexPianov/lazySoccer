using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class FitnessCenterMemberList : CenterMemberList
    {
        [SerializeField] private FitnessCenterMessages messages;
        [SerializeField] private BuildingInformation buildingInformation;
        [SerializeField] private SlotPlayerForm playerFormSlider;

        private void Start()
        {
            buildingInformation.updateLevel.AddListener(UpdateMemberList);
        }

        protected override void CreatePlayerList()
        {
            var restoringForm = CreatePlayersList(TeamPlayerStatus.RestoringForm, true);
            if(restoringForm != null && restoringForm.Count > 0) playerSlots.AddRange(restoringForm);
        }

        protected override void CreateSlots()
        {
            var buildingLevel = buildingInformation.GetHouseInfo().Level.Value;

            playerFormSlider.SetCenterLevel(buildingLevel);

            var freeSlotsCount = GetMaxMember(buildingLevel) - playerSlots.Count;
            CreateFreeSlots(freeSlotsCount, StatusBuilding.FitnessCenterTeam);
            
            var pastLevelMaxMembers = GetMaxMember(buildingLevel - 1) - playerSlots.Count;
            buildingInformation.DisableDowngrade(pastLevelMaxMembers < 0);
            
            //CreateLockedSlots(maxMembers);
        }

        private int maxMembers = 12;
        private int GetMaxMember(int level)
        {
            if (level < 1) level = 1;
            
            switch (level)
            {
                case <= 4: return 1;
                case <= 9: return 2;
                case <= 14: return 4;
                case <= 19: return 6;
                case <= 20: return maxMembers;
                default: return 1;
            }
        }

        protected async override void UpdateMemberStatus(SlotPlayer slot)
        {
            var player = slot.GetTeamPlayer();
            var status = player.status;
            
            if (status == TeamPlayerStatus.Charged || status == TeamPlayerStatus.RestoringForm)
            {
                _generalPopupMessage.ShowInfo("Player already " + status);
                return;
            }

            if (status == TeamPlayerStatus.Healthy || status == TeamPlayerStatus.Exhausted)
            {
                _buildingWindowStatus.SetAction(StatusBuilding.QuickLoading);
                
                var success = await messages.StartRestoringForm(player.playerId);

                if (!success)
                {
                    _buildingWindowStatus.StatusAction = StatusBuilding.FitnessCenterTeam;
                    return;
                }
                
                player.status = TeamPlayerStatus.RestoringForm;
                await UpdateTeamPlayers();
            
                _buildingWindowStatus.StatusAction = StatusBuilding.FitnessCenterMain;

                _generalPopupMessage.ShowInfo("PLAYER started restoring form", true, player.name);
                
                return;
            }
            
            _generalPopupMessage.ShowInfo("Player must be exhausted or healthy for being a member");
        }
    }
}