using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnion;
using LazySoccer.SceneLoading.Buildings.OfficeStatistics;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriend
{
    public class CommunityCenterFriendStatistics : MonoBehaviour
    {
        [Title("Refs")] [SerializeField] private CommunityCenterFriend centerFriend;
        
        private CommunityCenterFriendBuildings centerFriendBuildings;
        private OfficeStatisticsPosition officeStatisticsPosition;
        private OfficeStatisticsRewardList statisticsRewardList;
        private OfficeStatisticsTrophiesList statisticsTrophiesList;
        
        private TeamTypesOfRequests _teamTypesOfRequests;
        private BuildingWindowStatus _buildingWindowStatus;
        private BuildingTypesOfRequests _buildingTypesOfRequests;
        private CommunityCenterFriendStatus _communityCenterFriendStatus;


        private void Awake()
        {
            _communityCenterFriendStatus = ServiceLocator.GetService<CommunityCenterFriendStatus>();
            officeStatisticsPosition = GetComponent<OfficeStatisticsPosition>();
            centerFriendBuildings = GetComponent<CommunityCenterFriendBuildings>();
            statisticsRewardList = GetComponent<OfficeStatisticsRewardList>();
            statisticsTrophiesList = GetComponent<OfficeStatisticsTrophiesList>();

            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _teamTypesOfRequests = ServiceLocator.GetService<TeamTypesOfRequests>();
            _buildingTypesOfRequests = ServiceLocator.GetService<BuildingTypesOfRequests>();
        }

        private List<TeamReward> CurrentTeamRewards = new();
        private List<TeamTrophy> CurrentTeamTropies = new();
        private List<BuildingAll>  CurrentTeamBuildings = new();

        public async void UpdateStatistics() 
        {
            _buildingWindowStatus.OpenQuickLoading(true);

            officeStatisticsPosition.UpdateInfo(centerFriend.CurrentTeamStatistics);

            var teamId = centerFriend.CurrentUser.team.teamId;
            Debug.Log("UpdateStatistics | teamId: " + teamId);
            
            if (CurrentTeamRewards == null)
            {
                CurrentTeamRewards = await _teamTypesOfRequests.GET_TeamRewards(teamId, false);
                Debug.Log("CurrentTeamRewards: " + CurrentTeamRewards.Count);
            }
            
            if (CurrentTeamTropies == null)
            {
                CurrentTeamTropies = await _teamTypesOfRequests.GET_TeamTrophies(teamId, false);
                Debug.Log("CurrentTeamTropies: " + CurrentTeamTropies.Count);
            }

            if (CurrentTeamBuildings == null)
            {
                CurrentTeamBuildings = await _buildingTypesOfRequests
                    .Get_AllBuildingsRequest(teamId, false);
            }
            
            centerFriendBuildings.CreateBuildings(CurrentTeamBuildings);
            statisticsRewardList.CreateRewards(CurrentTeamRewards);
            statisticsTrophiesList.CreateTrophies(CurrentTeamTropies);
            
            _buildingWindowStatus.OpenQuickLoading(false);
            //_buildingWindowStatus.SetAction(StatusBuilding.CommunityCenterFriend);
        }

        public void ClearData()
        {
            CurrentTeamTropies = null;
            CurrentTeamRewards = null;
            CurrentTeamBuildings = null;
        }
    }
}