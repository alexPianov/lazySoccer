using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionRequests
{
    public class CommunityCenterUnionRequests : CenterSlotList
    {
        public CommunityCenterUnion.CommunityCenterUnion CenterUnion;

        public List<GeneralClassGETRequest.UserUnionRequest> CurrentUnionRequests { get; private set; }
        private BuildingWindowStatus _buildingWindowStatus;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;

        private void Awake()
        {
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
        }

        public void UpdateRequests()
        {
            GetUnionRequests();
        }

        private async UniTask GetUnionRequests()
        {
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var profile = ServiceLocator.GetService<ManagerCommunityData>()
                .GetOwnUnionProfile();

            CurrentUnionRequests = await 
                ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                    .GET_UnionRequests(profile.unionId);

            _buildingWindowStatus.SetAction(StatusBuilding.CommunityCenterUnion);

            CreateRequestList();
        }

        private void CreateRequestList()
        {
            if (CurrentUnionRequests == null || CurrentUnionRequests.Count == 0) return;
            if (CurrentUnionRequests[0].user.team == null) return;
            
            DestroyAllSlots();
            
            for (var i = 0; i < CurrentUnionRequests.Count; i++)
            {
                var request = CurrentUnionRequests[i];
                
                if (request.user.team == null) continue;

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterUnionRequestSlot slot))
                {
                    slot.SetInfo(request.user, i + 1);
                    slot.SetEmblem(null, GetEmblemSprite(request.user));
                    
                    slot.SetMaster(this);
                    slot.SetRequest(request);
                }
            }
        }
        
        public async UniTask<bool> AcceptUnionMember(int _request, string userName = "")
        {
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionRequestAcceptUser(_request);
            
            if (success)
            {
                await CenterUnion.RefreshUnion(StatusCommunityCenterUnion.Back, false, false);
                await GetUnionRequests();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"USER was added to union members", Param1: userName);
            }

            _buildingWindowStatus.OpenQuickLoading(false);
            
            return success;
        }
        
        public async UniTask<bool> RejectUnionMember(int _request, string userName = "")
        {
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionRequestDeleteUser(_request);
            
            if (success)
            {
                await GetUnionRequests();
                //await CenterUnion.RefreshUnion(StatusCommunityCenterUnion.Back, false, false);
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"USER was rejected from membership", Param1: userName);
            }
            
            _buildingWindowStatus.OpenQuickLoading(false);

            return success;
        }
    }
}