using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using LazySoccer.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreateInvites : CenterUserSearch
    {
        public Button buttonInvite;
        public List<GeneralClassGETRequest.User> InvitedUsers = new();
        public List<GeneralClassGETRequest.UserUnionRequest> UnionRequests { get; private set; }
        
        public CommunityCenterUnion.CommunityCenterUnion CenterUnion;

        private void Start()
        {
            buttonInvite.onClick.AddListener(InviteUsers);
        }

        public async void UpdateUnionRequest()
        {
            await GetUnionRequests();
            UpdateUserlist();
        }
        
        private async UniTask GetUnionRequests()
        {
            UnionRequests = await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .GET_UnionRequests(CenterUnion.CurrentUnion.unionId);
        }

        protected override void AddButtonListener(GameObject slotInstance)
        {
            if (slotInstance.TryGetComponent(out CommunityCenterUnionCreateInviteSlot slot))
            {
                slot.SetMaster(this);
                slot.IsAlreadyInvited();
                slot.AlreadyInUnion();
            }
        }

        public void AddInvite(GeneralClassGETRequest.User user)
        {
            InvitedUsers.Add(user);
            CheckButtonInteraction();
        }

        public void DeleteInvite(GeneralClassGETRequest.User user)
        {
            InvitedUsers.Remove(user);
            CheckButtonInteraction();
        }

        private void CheckButtonInteraction()
        {
            buttonInvite.interactable = InvitedUsers.Count > 0;
        }

        private async void InviteUsers()
        {
            List<string> invitedUsersId = new();

            foreach (var user in InvitedUsers)
            {
                invitedUsersId.Add(user.userId);
            }
            
            if(invitedUsersId.Count == 0) return;
            
            buttonInvite.interactable = false;

            var result = await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .POST_UnionRequestInviteUsers(invitedUsersId.ToArray());

            if (result)
            {
                await GetUnionRequests();
                UpdateUserlist();
                
                GeneralPopupMessage.ShowInfo("Picked users were invited to the union");
                
                if (CenterUnion.CurrentUnion.policy == 
                    GeneralClassGETRequest.RecruitingPolicy.Close)
                {
                    ServiceLocator.GetService<CommunityCenterUnionStatus>()
                        .SetAction(StatusCommunityCenterUnion.Requests);
                }
                
            }

            InvitedUsers.Clear();
            buttonInvite.interactable = true;
        }
    }
}