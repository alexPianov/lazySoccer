using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using Scripts.Infrastructure.Managers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriendlyMatches
{
    public class CommunityCenterFriendlyMatches : CenterSlotList
    {
        private ManagerCommunityData _managerCommunityData;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;

        private void Awake()
        {
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
        }

        public void UpdateMatchesList()
        {
            var friends = _managerCommunityData.GetMatchRequests();

            DestroyAllSlots();
            
            if(friends.Count == 0) return;
            if(friends[0] == null) return;

            foreach (var friend in friends)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterFriendlyMatchesSlot slot))
                {
                    slot.SetInfo(friend);
                    slot.SetMaster(this);
                }
            }
        }

        public async UniTask<bool> CancelMatch(string matchId)
        {
            return await _communityCenterTypesOfRequest.POST_MatchCancel(matchId);
        }
        
        public async UniTask<bool> CoinfirmMatch(string matchId)
        {
            return await _communityCenterTypesOfRequest.POST_MatchConfirm(matchId);
        }
    }
}