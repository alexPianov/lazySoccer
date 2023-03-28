using System;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreateInviteSlot : CommunityCenterFriendsSlot
    {
        [SerializeField] private Button buttonInvite;
        [SerializeField] private TMP_Text textButton;
        [SerializeField] private Sprite spriteIsInvited;
        private Sprite spriteNotInvited;
        private Image imageButtonInvite;
        private bool IsInvited;

        private void Awake()
        {
            imageButtonInvite = buttonInvite.GetComponent<Image>();
            spriteNotInvited = imageButtonInvite.sprite;
            buttonInvite.onClick.AddListener(OnClickInvite);
        }
        
        private CommunityCenterUnionCreateInvites _inviteList;
        public void SetMaster(CommunityCenterUnionCreateInvites invites)
        {
            _inviteList = invites;
        }
        
        public void IsAlreadyInvited()
        {
            if(UserData == null) { Debug.Log("User Data is null"); return; }
            
            if(_inviteList.UnionRequests == null) { Debug.Log("UnionRequests is null"); return; }
            
            var state = _inviteList.UnionRequests
                .Find(user => user.user.userId == UserData.userId);

            if (state == null)
            {
                return;
            }
            
            buttonInvite.interactable = false;
            //textButton.text = "In union";
            textButton.GetComponent<Localize>().SetTerm("3-CommCenterPopup-InviteUsers-Slot-Button-InUnion");
            
            if (state.isConfirmUser)
            {
                //textButton.text = "Requested";
                textButton.GetComponent<Localize>().SetTerm("3-CommCenterPopup-InviteUsers-Slot-Button-Requested");
            }
            
            if (state.isConfirmUnion)
            {
                //extButton.text = "Invited";
                textButton.GetComponent<Localize>().SetTerm("3-CommCenterPopup-InviteUsers-Slot-Button-Invite");
            }
        }

        public void AlreadyInUnion()
        {
            var unionTeams = _inviteList.CenterUnion.CurrentUnion.unionTeams;

            foreach (var teamProfile in unionTeams)
            {
                Debug.Log(" --- Union team: " + teamProfile.user.team.name + " | current slot team: " + UserData.team.name);
                if(teamProfile.user.team.name == UserData.team.name)
                {
                    buttonInvite.gameObject.SetActive(false);
                    break;
                }
            }
        }

        private void OnClickInvite()
        {
            Debug.Log("Invite: " + UserData.userName);
            
            if (IsInvited)
            {
                _inviteList.DeleteInvite(UserData);
                Invite();
            }
            else
            {
                _inviteList.AddInvite(UserData);
                Invited();
            }
        }

        private void Invite()
        {
            //textButton.text = "Pick";
            textButton.GetComponent<Localize>().SetTerm("3-CommCenterPopup-InviteUsers-Slot-Button-Pick");
            imageButtonInvite.sprite = spriteNotInvited;
            textButton.color = DataUtils.GetColorFromHex("FFFFFF");
            IsInvited = false;
        }

        private void Invited()
        {
            //textButton.text = "Picked";
            textButton.GetComponent<Localize>().SetTerm("3-CommCenterPopup-InviteUsers-Slot-Button-Picked");
            imageButtonInvite.sprite = spriteIsInvited;
            textButton.color = DataUtils.GetColorFromHex("A455FF");
            IsInvited = true;
        }

        private void OnDestroy()
        {
            _inviteList.DeleteInvite(UserData);
        }
    }
}