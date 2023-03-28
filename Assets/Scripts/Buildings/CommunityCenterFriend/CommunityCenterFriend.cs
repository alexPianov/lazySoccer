using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenterFriendlyMatches;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriend
{
    public class CommunityCenterFriend : MonoBehaviour
    {
        public GeneralClassGETRequest.User CurrentUser { get; private set; }
        public TeamStatistic CurrentTeamStatistics { get; private set; }
        public List<TeamPlayer> CurrentTeamRoster { get; private set; }
        public OfficeTypesOfRequests OfficeTypesOfRequests { get; private set; }

        [Title("Refs")] 
        [SerializeField] private CommunityCenterFriendBang friendBang;
        [SerializeField] private CommunityCenterFriendButtons friendButtons;
        [SerializeField] private CommunityCenterFriendStatistics friendStatistics;
        [SerializeField] private GameObject popupContainerFriend;

        private BuildingWindowStatus _buildingWindowStatus;
        private TeamTypesOfRequests _teamTypesOfRequests;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private CommunityCenterFriendStatus _communityCenterFriendStatus;

        private void Awake()
        {
            _communityCenterFriendStatus = ServiceLocator.GetService<CommunityCenterFriendStatus>();
            OfficeTypesOfRequests = ServiceLocator.GetService<OfficeTypesOfRequests>();
            _teamTypesOfRequests = ServiceLocator.GetService<TeamTypesOfRequests>();
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
        }

        public async void ShowFriendData(string friendTeamName)
        {
            var users = await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_Users(friendTeamName);

            var user = users.Find(user => user.team.name == friendTeamName);
            
            await ShowFriendData(user);
        }

        public async UniTask ShowFriendData(GeneralClassGETRequest.User friend)
        {
            Debug.Log("Show Friend Data | Team: " + friend.team.teamId);

            if (CurrentUser != null && friend.userName != CurrentUser.userName)
            {
                CurrentTeamRoster = null;
                CurrentTeamStatistics = null;
            }
            
            CurrentUser = friend;

            friendButtons.SetButtons(friend);
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);

            if (CurrentTeamStatistics == null)
            {
                CurrentTeamStatistics = await ServiceLocator.GetService<OfficeTypesOfRequests>()
                    .GetTeamStatistic(friend.team.teamId);

                if (CurrentTeamStatistics.union != null)
                {
                    CurrentTeamStatistics.union.emblem = await  ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                        .GET_UnionEmblem(CurrentTeamStatistics.union.unionId, false);
                }
            }

            if (CurrentTeamRoster == null)
            {
                CurrentTeamRoster = await ServiceLocator.GetService<TeamTypesOfRequests>()
                    .GET_TeamPlayers(friend.team.teamId);
            }

            if(popupContainerFriend) popupContainerFriend.SetActive(true);
            
            ServiceLocator.GetService<CommunityCenterFriendStatus>().SetAction(StatusCommunityCenterFriend.Roster);
            //popupContainerFriend.SetAction(StatusCommunityCenterFriend.Roster);
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
            
            friendStatistics.ClearData();
            friendBang.UpdateBang();
        }
    }
}