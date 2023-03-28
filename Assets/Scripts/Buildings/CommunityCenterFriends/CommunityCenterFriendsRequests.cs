using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsRequests : MonoBehaviour
    {
        [SerializeField]
        private CommunityCenterFriends Friendlist;
        
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private CommunityCenterStatus _communityCenterStatus;
        private CommunityCenterPopupStatus _communityCenterPopupStatus;
        
        private void Awake()
        {
            _communityCenterTypesOfRequest = ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>();
                
            _communityCenterStatus = ServiceLocator
                .GetService<CommunityCenterStatus>();

            _communityCenterPopupStatus = ServiceLocator
                .GetService<CommunityCenterPopupStatus>();
        }

        public async UniTask<bool> FriendshipRequest(GeneralClassGETRequest.User user)
        {
            var success = await _communityCenterTypesOfRequest
                .POST_FriendshipRequest(user.userId);

            if (success)
            {
                Friendlist.GeneralPopupMessage.ShowInfo
                    ($"Friendship with FRIEND was requested", Param1:user.userName);
                
                //await Friendlist.UpdateUserlist();
                
                UpdateFriendlists();
            }

            return success;
        }

        public async UniTask<bool> FriendshipConfirm(GeneralClassGETRequest.User user)
        {
            var success =  await _communityCenterTypesOfRequest
                .POST_FriendshipConfirm(user.userId);
            
            if (success)
            {
                Friendlist.GeneralPopupMessage.ShowInfo
                    ($"FRIEND was added to friendlist", Param1: user.userName);
                
                UpdateFriendlists();
            }

            return success;
        }
        
        public async UniTask<bool> FriendshipReject(GeneralClassGETRequest.User user)
        {
            Debug.Log("FriendshipReject: " + user.userId);

            var request = ServiceLocator.GetService<ManagerCommunityData>()
                .GetFriendshipRequest(user.userId);
            
            var success = await _communityCenterTypesOfRequest
                .POST_FriendshipReject(request.requestId);
            
            if (success)
            {
                Friendlist.GeneralPopupMessage
                    .ShowInfo($"FRIEND was rejected", Param1: user.userName);
                
                UpdateFriendlists();
            }

            return success;
        }
        
        public async UniTask<bool> DeleteFriend(GeneralClassGETRequest.User user)
        {
            var success = await _communityCenterTypesOfRequest
                .POST_DeleteFriend(user.userId);
            
            if (success)
            {
                Friendlist.GeneralPopupMessage.ShowInfo
                    ($"FRIEND was deleted from friendlist", Param1:user.userName);

                UpdateFriendlists();
            }

            return success;
        }

        private void UpdateFriendlists()
        {
            Debug.Log("UpdateFriendlists");
            
            _communityCenterStatus.SetAction(StatusCommunityCenter.Friends);
            
            if (_communityCenterPopupStatus.currentStatus 
                == StatusCommunityCenterPopup.FreindsFind)
            { 
                _communityCenterPopupStatus.StatusAction = StatusCommunityCenterPopup.None;
                _communityCenterPopupStatus.StatusAction = StatusCommunityCenterPopup.FreindsFind; 
            }
        }
    }
}