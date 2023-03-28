using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static Scripts.Infrastructure.Managers.ManagerCommunityData;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsFind : CenterUserSearch
    {
        [Title("Check")] 
        [SerializeField] private CommunityCenterFriendStatusCheck statusCheck;

        protected override void AddButtonListener(GameObject slotInstance)
        {
            if (slotInstance.TryGetComponent(out CommunityCenterFriendsSlotFound slot))
            {
                SetButtonStatus(slot);
            }
        }

        private void SetButtonStatus(CommunityCenterFriendsSlotFound slotFound)
        {
            List<string> names = new(); 
            
            names.Add(slotFound.UserData.team.name);
            names.Add(slotFound.UserData.userName);

            if (statusCheck.StatusMatchWithCurrentUser(names))
            {
                slotFound.ButtonStatus(false, "You"); return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Confirmed, names))
            {
                slotFound.ButtonStatus(false, "Confirmed"); return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Income, names))
            {
                slotFound.ButtonStatus(false, "FriendshipRequest"); return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Outcome, names))
            {
                slotFound.ButtonStatus(false, "Requested"); return;
            }
            
            slotFound.ButtonStatus(true, "AddFriend");
        }
    }
}