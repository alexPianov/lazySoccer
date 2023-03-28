using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.Status;
using LazySoccer.Windows;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnion : MonoBehaviour
    {
        public CommunityCenterUnionDisplay unionDisplay;

        public UnionProfile CurrentUnion { get; private set; }
        private BuildingWindowStatus _buildingWindowStatus;
        private CommunityCenterUnionStatus _communityCenterUnionStatus;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private ManagerPlayerData _managerPlayerData;
        private ManagerCommunityData _managerCommunityData;

        private void Awake()
        {
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _communityCenterUnionStatus = ServiceLocator.GetService<CommunityCenterUnionStatus>();
        }

        public async UniTask RefreshUnion(StatusCommunityCenterUnion openCatalog = StatusCommunityCenterUnion.Members, bool updateUnionProfile = true, bool updateUserData = true)
        {
            await _communityCenterTypesOfRequest.GET_UserUnionProfile(false);
            if(updateUserData) await ServiceLocator.GetService<UserTypesOfRequests>().GetUserRequest(false);
            await ShowUnion(CurrentUnionInfo, openCatalog);
        }

        public void UpdateUnionEmblem(int emblemId)
        {
            CurrentUnionInfo.emblem.emblemId = emblemId;
        }

        private Union CurrentUnionInfo;
        public async UniTask ShowUnion(Union union, StatusCommunityCenterUnion openCatalog = StatusCommunityCenterUnion.Members)
        {
            Debug.Log("Show union: " + union.name);
            
            _buildingWindowStatus.OpenQuickLoading(true);

            CurrentUnionInfo = union;
            CurrentUnion = null;
            
            var ownUnion = _managerCommunityData.GetOwnUnionProfile();

            if (ownUnion != null)
            {
                if (CurrentUnionInfo.unionId == ownUnion.unionId)
                {
                    CurrentUnion = ownUnion;
                }
                else
                {
                    await GetCurrentUnionById(union);
                }
            }
            else
            {
                await GetCurrentUnionById(union);
            }
            
            _buildingWindowStatus.OpenQuickLoading(false);
            
            _buildingWindowStatus.SetAction(StatusBuilding.CommunityCenterUnion);

            if (openCatalog != StatusCommunityCenterUnion.Back)
            {
                _communityCenterUnionStatus.SetAction(openCatalog);
            }
            
            unionDisplay.SetInfo(union);
            unionDisplay.SetEmblem(GetUnionEmblem(union));
        }

        private async UniTask GetCurrentUnionById(Union union)
        {
            CurrentUnion = await _communityCenterTypesOfRequest
                .GET_UnionProfile(union.unionId);
        }

        private Sprite GetUnionEmblem(Union union)
        {
            //if(union.unionEmblem == null) { Debug.LogError("Union emblem is null: " + union.name);}
            return ServiceLocator.GetService<ManagerSprites>().GetUnionSprite(union);
        }
        
        public UnionTeam GetUserTeam()
        {
            if (CurrentUnion == null) return null;
            
            return CurrentUnion.unionTeams
                .Find(team => team.user.team.name == _managerPlayerData.PlayerHUDs.NameTeam.Value);
        }
        
        public string GetOfficerIdForReplacement()
        {
            if (CurrentUnion == null) return "";
            
            var officer = CurrentUnion.unionTeams
                .Find(team => team.type == MemberType.Officer);

            if (officer == null) return "";

            return officer.teamUnionId.ToString();
        }
    }
}