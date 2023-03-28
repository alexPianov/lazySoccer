using System;
using System.Collections.Generic;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnions;
using LazySoccer.SceneLoading.UI;
using LazySoccer.Status;
using LazySoccer.Windows;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionButtons : MonoBehaviour
    {
        [Title("Refs")]
        [SerializeField] private CommunityCenterUnion CenterUnion;
        [SerializeField] private CommunityCenterUnionsMode UnionsMode;
        [SerializeField] private CommunityCenterUnionGroupButtons GroupButtons;
        [SerializeField] private CommunityCenterUnions.CommunityCenterUnions CenterUnions;
        [SerializeField] private CommunityCenterUnionsJoinRequest JoinRequest;
        
        [Title("Buttons")]
        [SerializeField] private Button buttonLeave;
        [SerializeField] private Button buttonRequest;
        [SerializeField] private Button buttonInvite;
        [SerializeField] private Button buttonClose;

        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private ManagerPlayerData _managerPlayerData;

        private void Awake()
        {
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
        }

        private void Start()
        {
            buttonLeave.onClick.AddListener(LeaveCurrentUnion);
            buttonRequest.onClick.AddListener(JoinCurrentUnion);
            buttonClose.onClick.AddListener(CloseWindow);
        }
        
        public void UpdateButtons()
        {
            JoinButton();
            
            var playerTeam = CenterUnion.GetUserTeam();

            if (playerTeam == null)
            {
                buttonLeave.gameObject.SetActive(false);
                buttonInvite.gameObject.SetActive(false);
                buttonRequest.gameObject.SetActive(true);

                if (CenterUnion.CurrentUnion == null)
                {
                    ActiveRequestButton(true);
                    GroupButtons.RefreshGroupButtons(MemberType.Simple);
                    return;
                }
                
                var haveRequest = JoinRequest
                    .CheckUnionForRequest(CenterUnion.CurrentUnion.unionId);
                
                ActiveRequestButton(!haveRequest);

                GroupButtons.RefreshGroupButtons(MemberType.Simple);
                
                return;
            }

            buttonRequest.gameObject.SetActive(false);

            if (playerTeam.type == MemberType.Master || playerTeam.type == MemberType.Officer)
            {
                buttonInvite.gameObject.SetActive(true);
            }
            else
            {
                buttonInvite.gameObject.SetActive(false);
            }
            
            buttonLeave.gameObject.SetActive(true);
            
            GroupButtons.RefreshGroupButtons(playerTeam.type);
        }

        private void JoinButton()
        {
            if (!UnionsMode.userCanJoinUnions)
            {
                buttonRequest.interactable = false;
                return;
            }

            var ownUnion = ServiceLocator.GetService<ManagerCommunityData>()
                .GetOwnUnionProfile();

            if (ownUnion == null)
            {
                buttonRequest.interactable = true;
            }
            else
            {
                buttonRequest.interactable = CenterUnion.CurrentUnion.unionId != ownUnion.unionId;
            }
        }

        private async void LeaveCurrentUnion()
        {
            Debug.Log("Leave union | user id: " + _managerPlayerData.PlayerData.UserId);
            buttonRequest.interactable = false;
            
            var playerTeam = CenterUnion.GetUserTeam();
            if (playerTeam == null)
            {
                Debug.LogError("Failed to find player team");
                return;
            }
            
            var replacementMasterId = "";
            if (playerTeam.type == MemberType.Master)
            {
                replacementMasterId = CenterUnion.GetOfficerIdForReplacement();
            }
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionMemberKick(playerTeam.teamUnionId, replacementMasterId);

            if (success)
            {
                await CenterUnion.RefreshUnion();
                CenterUnions.UpdateUnionsList();
                
                Debug.Log("CloseWindow");
                if (playerTeam.type == MemberType.Master)
                {
                    CloseWindow();
                }
                
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("You have left this union");
            }
            
            buttonRequest.interactable = true;
        }
        
        private async void JoinCurrentUnion()
        {
            Debug.Log("Join: " + CenterUnion.CurrentUnion.name);
            buttonRequest.interactable = false;
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionRequestJoin(CenterUnion.CurrentUnion.unionId);

            if (success)
            {
                await CenterUnion.RefreshUnion();
                CenterUnions.UpdateUnionsList();
                ActiveRequestButton(false);

                return;
            }
            
            buttonRequest.interactable = true;
        }

        private void ActiveRequestButton(bool state)
        {
            buttonRequest.interactable = state;
            
            if (state)
            {
                buttonRequest.GetComponentInChildren<Localize>().SetTerm("3-CommCenterUnion-Button-JoinRequest");
            }
            else
            {
                buttonRequest.GetComponentInChildren<Localize>().SetTerm("3-CommCenterUnion-Button-RequestWasSent");
            }
        }

        public void CloseWindow()
        {
            ServiceLocator.GetService<BuildingWindowStatus>()
                .SetAction(StatusBuilding.CommunityCenter);
            
            ServiceLocator.GetService<CommunityCenterStatus>()
                .SetAction(StatusCommunityCenter.Unions);
        }
    }
}