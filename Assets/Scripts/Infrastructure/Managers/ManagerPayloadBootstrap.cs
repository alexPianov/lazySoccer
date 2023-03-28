using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using static LazySoccer.SceneLoading.Buildings.CommunityCenter.CommunityCenterLeaders;

namespace LazySoccer.Network
{
    public class ManagerPayloadBootstrap : MonoBehaviour
    {
        private ManagerPlayerData _managerPlayer;
        private LoadingScene _loadingScene;
        private ManagerLoading _managerLoading;
        private CreateTeamStatus _createTeamStatus;

        private void Start()
        {
            _managerPlayer = ServiceLocator.GetService<ManagerPlayerData>();
            _loadingScene = ServiceLocator.GetService<LoadingScene>();
            _managerLoading = ServiceLocator.GetService<ManagerLoading>();
            _createTeamStatus = ServiceLocator.GetService<CreateTeamStatus>();
        }

        public async UniTask Boot(bool globalLoading = true)
        {
            Debug.Log("Boot");
            
            if(globalLoading) _managerLoading.ControlLoading(true);
            
            await GetUserData();
            
            if (TeamNameIsNull())
            {
                await CreateTeam();
                return;
            }

            await GetBuildingsData();
            
            var result = await GetTeamPlayers();

            if (!result)
            {
                await GetTeamInfo();
                OpenCreateTeamScene(); return;
            }

            await GetTeamInfo();
            await CacheLeaderboardData();

            await GetSeasonBalance();
            await GetPastDayBalance();
            
            await GetFriends();
            await GetFriendsRequests();

            await GetUnions();
            await GetUserUnionProfile();
            
            await GetCurrentSeason();
            
            //_managerLoading.ControlLoading(true);
            
            await LoadMenuScene();

            Debug.Log("Finish boot");
            _managerLoading.ControlLoading(false);
        }

        private async UniTask CacheLeaderboardData()
        {
            await GetLeaderboard(LeaderFilter.ByPower);
            await GetLeaderboard(LeaderFilter.ByPerformance);
            await GetLeaderboard(LeaderFilter.ByBalance);
        }

        private async void OpenCreateTeamScene()
        {
            await _loadingScene.AssetLoaderScene
                ("CreateTeam", StatusBackground.Active, new UniTask());
                
            ServiceLocator.GetService<CreateTeamStatus>()
                .SetAction(StatusCreateTeam.CreateTeam);
        }

        private async UniTask LoadMenuScene()
        {
            await _loadingScene.AssetLoaderScene("GameLobby", StatusBackground.Active, new UniTask());
        }

        private bool TeamNameIsNull()
        {
            return _managerPlayer.PlayerHUDs.NameTeam.Value.Length == 0;
        }
        
        private async UniTask GetUserData()
        {
            await ServiceLocator.GetService<UserTypesOfRequests>()
                .GetUserRequest(false);
        }
        
        private async UniTask CreateTeam()
        {
            Debug.Log("Player has no available team | Create new team");
            
            await _loadingScene.AssetLoaderScene
                ("CreateTeam", StatusBackground.Active, new UniTask());
            
            _createTeamStatus.StatusAction = StatusCreateTeam.CreateTeam;
            
            _managerLoading.ControlLoading(false);
        }
        
        private async UniTask GetBuildingsData()
        {
            await ServiceLocator.GetService<BuildingTypesOfRequests>()
                .Get_AllBuildingRequest(false);
        }

        private async UniTask<bool> GetTeamPlayers()
        {
            return await ServiceLocator.GetService<TeamTypesOfRequests>()
                .GET_TeamPlayers(false);
        }

        private async UniTask<bool> GetLeaderboard(LeaderFilter filter)
        {
            return await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_WorldRating(filter, globalLoading: false);
        }

        private async UniTask GetTeamInfo()
        {
            await ServiceLocator.GetService<OfficeTypesOfRequests>().
                GetTeamInfo(false);
        }
        
        private async UniTask GetPastDayBalance()
        {
            await ServiceLocator.GetService<OfficeTypesOfRequests>()
                .GetFinancialStatistics(OfficeRequests.PastDay, false);
        }

        private async UniTask GetSeasonBalance()
        {
            await ServiceLocator.GetService<OfficeTypesOfRequests>()
                .GetFinancialStatistics(OfficeRequests.Season, false);
        }
        
        private async UniTask GetFriends()
        {
            await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_Friends(false);
        }

        private async UniTask GetUnions()
        {
            await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_Unions(false, false);
        }
        
        private async UniTask GetUserUnionProfile()
        {
            await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_UserUnionProfile(false);
        }
        
        private async UniTask GetFriendsRequests()
        {
            await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .GET_FriendsRequests(false);
        }
        
        private async UniTask GetCurrentSeason()
        {
            await ServiceLocator.GetService<TeamTypesOfRequests>().GET_Season(false);
        }
    }
}