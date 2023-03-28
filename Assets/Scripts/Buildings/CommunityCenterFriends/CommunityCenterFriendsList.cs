using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static Scripts.Infrastructure.Managers.ManagerCommunityData;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsList : CommunityCenterFriends
    {
        [Title("Type")]
        [SerializeField] private FriendType friendshipType;
        
        [Title("Text")]
        [SerializeField] private TMP_Text textFriendsCount;
        [SerializeField] private TMP_Text textFriendsCountButton;

        public override async UniTask UpdateUserlist()
        {
            var users = GetUsers();
            
            await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .GET_FriendsRequests(false);

            users = GetUsers();

            DeleteAllSlots();

            if(users.Count == 0) return;

            for (var i = 0; i < users.Count; i++)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterFriendsSlot slot))
                {
                    slot.SetInfo(users[i], i + 1);
                    slot.SetEmblem(GetUserSprite(users[i]), GetEmblemSprite(users[i]));
                    slot.SetFriendList(this);
                    
                    CreateDisplayListener(slot);
                    
                    PlayerSlots.Add(slot);
                }
            }
        }

        private List<GeneralClassGETRequest.User> GetUsers()
        {
            var users = _managerCommunityData.GetFriends(friendshipType);

            textFriendsCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({users.Count})");
            textFriendsCountButton.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({users.Count})");
            // textFriendsCount.text = $"{titleName} ({users.Count})";
            // textFriendsCountButton.text = $"{titleName} ({users.Count})";
            return users;
        }
    }
}