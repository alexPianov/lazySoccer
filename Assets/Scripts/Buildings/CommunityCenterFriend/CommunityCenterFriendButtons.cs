using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using LazySoccer.SceneLoading.Buildings.CommunityCenterFriendlyMatches;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static Scripts.Infrastructure.Managers.ManagerCommunityData;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriend
{
    public class CommunityCenterFriendButtons : MonoBehaviour
    {
        [Title("Refs")] 
        [SerializeField] private CommunityCenterFriendStatusCheck statusCheck;
        [SerializeField] private CommunityCenterFriendsRequests friendsRequests;

        [Title("Button")] 
        [SerializeField] private GameObject prefabButton;
        [SerializeField] private Transform prefabContainer;

        private GeneralClassGETRequest.User CurrentUser;

        public enum ButtonType
        {
            MatchRequest, FriendDelete, FriendshipConfirm, 
            FriendshipReject, FriendshipRequest, FriendshipRequested,
            MatchRequested
        }

        private Dictionary<ButtonType, Button> buttons = new();

        public void SetButtons(GeneralClassGETRequest.User user)
        {
            CurrentUser = user;
            
            DeleteAllButtons();
            
            if (statusCheck.StatusMatchWithCurrentUser(user.userName))
            {
                return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Confirmed, user.userName))
            {
                var _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
                
                if (_managerCommunityData.HaveMatchWith(user))
                {
                    CreateButton(ButtonType.MatchRequested);
                }
                else
                {
                    CreateButton(ButtonType.MatchRequest);
                }
                CreateButton(ButtonType.FriendDelete);
                return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Income, user.userName))
            {
                CreateButton(ButtonType.FriendshipConfirm);
                CreateButton(ButtonType.FriendshipReject);
                return;
            }
            
            if (statusCheck.StatusMatch(FriendType.Outcome, user.userName))
            {
                CreateButton(ButtonType.FriendshipRequested);
                return;
            }

            CreateButton(ButtonType.FriendshipRequest);
        }
        
        private void DeleteAllButtons()
        {
            for (var i = 0; i < prefabContainer.childCount; i++)
            {
                Destroy(prefabContainer.GetChild(i).gameObject);
            }
            
            buttons.Clear();
        }

        private void ActiveButton(ButtonType buttonType, bool state)
        {
            buttons.TryGetValue(buttonType, out var button);
            button.interactable = state;
        }

        private void CreateButton(ButtonType buttonType)
        {
            var button = Instantiate(prefabButton, prefabContainer).GetComponent<Button>();
            buttons.Add(buttonType, button);

            var buttonText = button.GetComponentInChildren<TMP_Text>();

            if (buttonType == ButtonType.FriendshipRequest)
            {
                button.onClick.AddListener(FriendshipRequest);
                //buttonText.text = "Request friendship";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-FriendshipRequest");
            }
            
            if (buttonType == ButtonType.FriendshipConfirm)
            {
                button.onClick.AddListener(FriendshipConfirm);
                //buttonText.text = "Confirm friendship";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-FriendshipConfirm");
            }
            
            if (buttonType == ButtonType.FriendshipReject)
            {
                button.onClick.AddListener(FriendshipReject);
                //buttonText.text = "Reject request";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-Reject");
            }
            
            if (buttonType == ButtonType.FriendDelete)
            {
                button.onClick.AddListener(FriendDelete);
                //buttonText.text = "Delete friend";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-Delete");
            }
            
            if (buttonType == ButtonType.FriendshipRequested)
            {
                button.interactable = false;
                //buttonText.text = "Friendship requested";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-FriendshipRequest");
            }
            
            if (buttonType == ButtonType.MatchRequest)
            {
                button.onClick.AddListener(MatchRequest);
                //buttonText.text = "Request match";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-MatchRequest");
            }
            
            if (buttonType == ButtonType.MatchRequested)
            {
                button.interactable = false;
                //buttonText.text = "Match requested";
                buttonText.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-MatchRequested");
            }
        }

        private async void FriendshipRequest()
        {
            ActiveButton(ButtonType.FriendshipRequest, false);
            
            var result = await friendsRequests.FriendshipRequest(CurrentUser);
            
            if (result)
            {
                DeleteAllButtons();
                CreateButton(ButtonType.FriendshipRequested);
            }
            else
            {
                ActiveButton(ButtonType.FriendshipRequest, true);
            }
        }
        
        private async void FriendshipConfirm()
        {
            ActiveButton(ButtonType.FriendshipConfirm, false);
            
            var result = await friendsRequests.FriendshipConfirm(CurrentUser);
            
            if (result)
            {
                DeleteAllButtons();
                CreateButton(ButtonType.FriendDelete);
                CreateButton(ButtonType.MatchRequest);
            }
            else
            {
                ActiveButton(ButtonType.FriendshipConfirm, true);
            }
        }
        
        private async void FriendshipReject()
        {
            ActiveButton(ButtonType.FriendshipReject, false);
            
            var result = await friendsRequests.FriendshipReject(CurrentUser);
            
            if (result)
            {
                DeleteAllButtons();
                CreateButton(ButtonType.FriendshipRequest);
            }
            else
            {
                ActiveButton(ButtonType.FriendshipReject, true);
            }
        }
        
        private void MatchRequest()
        {
            Debug.Log("MatchRequest");
            ServiceLocator.GetService<CommunityCenterPopupStatus>()
                .SetAction(StatusCommunityCenterPopup.MatchPlanner);
        }
        
        private async void FriendDelete()
        {
            ActiveButton(ButtonType.FriendDelete, false);
            
            var result = await friendsRequests.DeleteFriend(CurrentUser);
            
            if (result)
            {
                DeleteAllButtons();
                CreateButton(ButtonType.FriendshipRequest);
            }
            else
            {
                ActiveButton(ButtonType.FriendDelete, true);
            }
        }
    }
}