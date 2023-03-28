using System;
using Playstel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionMemberSlot : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] private TMP_Text textRate;
        [SerializeField] private TMP_Text textTeamName;
        [SerializeField] private TMP_Text textContribution;
        
        [Title("Image")]
        [SerializeField] private Image imageStatus;
        [SerializeField] private Image imageEmblem;

        [Title("Buttons")] 
        [SerializeField] private Button buttonMore;
        [SerializeField] private Button buttonPromote;
        [SerializeField] private Button buttonDemote;
        [SerializeField] private Button buttonKick;

        [Title("Panel")] 
        [SerializeField] private UiTransparency panel;

        private Button buttonSlot;

        private void Awake()
        {
            buttonSlot = GetComponent<Button>();
            buttonSlot.onClick.AddListener(OpenFriendWindow);
            
            buttonMore.onClick.AddListener(OpenPanel);
            buttonPromote.onClick.AddListener(Promote);
            buttonDemote.onClick.AddListener(Demote);
            buttonKick.onClick.AddListener(Kick);
        }

        private CommunityCenterUnionMembers _unionMembers;
        public void SetMemberList(CommunityCenterUnionMembers  memberList)
        {
            _unionMembers = memberList;
        }

        private void OpenFriendWindow()
        {
            _unionMembers.CommunityCenterFriend.ShowFriendData(_unionTeamName);
        }

        private string _unionTeamName;
        private UnionTeam _unionTeam;
        public void SetInfo(UnionTeam unionTeam, int rate)
        {
            _unionTeam = unionTeam;
            _unionTeamName = unionTeam.user.team.name;
            
            if (textRate) textRate.text = rate.ToString();
            if (textTeamName) textTeamName.text = unionTeam.user.team.name;
            if (textContribution) textContribution.text = unionTeam.contribution.ToString();
        }

        public void MenuAccess(MemberType memberType)
        {
            if (memberType == MemberType.Simple)
            {
                buttonMore.gameObject.SetActive(false);
                buttonDemote.gameObject.SetActive(false);
                buttonPromote.gameObject.SetActive(false);
                buttonKick.gameObject.SetActive(false);
            }

            if (memberType == MemberType.Officer)
            {
                buttonMore.gameObject.SetActive(true);
                buttonDemote.gameObject.SetActive(false);
                buttonPromote.gameObject.SetActive(false);
                buttonKick.gameObject.SetActive(true);
            }
            
            if (memberType == MemberType.Master)
            {
                buttonMore.gameObject.SetActive(true);
                
                buttonDemote.gameObject.SetActive(_unionTeam.type != MemberType.Simple);
                buttonPromote.gameObject.SetActive(_unionTeam.type != MemberType.Officer);
                
                buttonKick.gameObject.SetActive(true);
            }
        }

        public void SetEmblem(Sprite spriteEmblem)
        {
            if(imageEmblem && spriteEmblem) imageEmblem.sprite = spriteEmblem;
        }

        public void SetStatusEmbem(Sprite spriteStatus)
        {
            imageStatus.enabled = spriteStatus;
            
            if(imageStatus && spriteStatus) imageStatus.sprite = spriteStatus;
        }

        private void OpenPanel()
        {
            panel.BlocksRaycasts(true);
            panel.Transparency(false);
        }

        public void ClosePanel()
        {
            panel.BlocksRaycasts(false);
            panel.Transparency(true);
        }

        private async void Promote()
        {
            ClosePanel();
            buttonSlot.interactable = false;
            var result = await _unionMembers.Promote(_unionTeam.teamUnionId);
            buttonSlot.interactable = true;
            
            if (result)
            {
                _unionMembers.UpdateUnionInfo();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"PLAYER was promoted", Param1: _unionTeam.user.team.name);
            } 
        }

        private async void Demote()
        {
            ClosePanel();
            buttonSlot.interactable = false;
            var result = await _unionMembers.Demote(_unionTeam.teamUnionId);
            buttonSlot.interactable = true;
            if (result)
            {
                _unionMembers.UpdateUnionInfo();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"PLAYER was demoted", Param1: _unionTeam.user.team.name);
            } 
        }

        private async void Kick()
        {
            ClosePanel();
            buttonSlot.interactable = false;
            var result = await _unionMembers.Kick(_unionTeam.teamUnionId);
            buttonSlot.interactable = true;
            if (result)
            {
                _unionMembers.UpdateUnionInfo();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"PLAYER was kicked", Param1: _unionTeam.user.team.name);
            } 
        }
    }
}