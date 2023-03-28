using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriends : CenterSlotList
    {
        [Title("List")] 
        [SerializeField] protected List<CommunityCenterFriendsSlot> PlayerSlots = new();
        
        [Title("Friend Window")]
        [SerializeField] private CommunityCenterFriend.CommunityCenterFriend CenterFriend;
        [HideInInspector] public CommunityCenterFriendsRequests FriendsRequests;
        
        protected ManagerCommunityData _managerCommunityData;
        protected BuildingWindowStatus _buildingWindowStatus;
        public void Awake()
        {
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            FriendsRequests = GetComponent<CommunityCenterFriendsRequests>();
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();

            base.Awake();
        }

        public virtual UniTask UpdateUserlist()
        {
            return new UniTask();
        }

        public void CreateDisplayListener(CommunityCenterFriendsSlot slot)
        {
            if(!CenterFriend) return;
            
            slot.gameObject.AddComponent<CommunityCenterFriendsSlotListener>()
                .SetFriendTable(CenterFriend);
        }
        
        protected Sprite GetUserSprite(GeneralClassGETRequest.User user)
        {
            return null;
        }

        protected void DeleteAllSlots()
        {
            DestroyAllSlots();
            PlayerSlots.Clear();
        }
    }
}