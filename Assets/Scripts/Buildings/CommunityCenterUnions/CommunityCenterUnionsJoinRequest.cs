using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionsJoinRequest : CenterSlotList
    {
        private List<UserUnionRequest> CurrentUnionRequests = new();

        [SerializeField] private JoinType _joinType;
        [SerializeField] private CommunityCenterUnion.CommunityCenterUnion _unionDisplay;
        [SerializeField] private CommunityCenterUnionsUpdate _unionsUpdate; 
        [SerializeField] private BuildingInformation _buildingInformation;
        
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private BuildingWindowStatus _buildingWindowStatus;
        
        [Title("Text")]
        [SerializeField] private TMP_Text textRequestCount;
        [SerializeField] private TMP_Text textRequestCountButton;
        
        public enum JoinType
        {
            Invite, Request
        }

        private void Start()
        {
            _communityCenterTypesOfRequest = ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>();

            _buildingWindowStatus = ServiceLocator
                .GetService<BuildingWindowStatus>();
        }

        public int GetBuildingLevel()
        {
            return _buildingInformation.GetHouseInfo().Level.Value;
        }

        public bool CheckUnionForRequest(int unionId)
        {
            foreach (var request in CurrentUnionRequests)
            {
                if (request.union.unionId == unionId) return true;
            }

            return false;
        }

        public async UniTask UnionJoinRequests()
        {
            CurrentUnionRequests = await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .GET_UnionRequests();

            DestroyAllSlots();

            int count = 0;
            foreach (var request in CurrentUnionRequests)
            {
                if (_joinType == JoinType.Invite)
                {
                    if (request.isConfirmUnion && !request.isConfirmUser)
                    {
                        CreateRequest(request);
                        count++; 
                        continue;
                    }
                }
                
                if (_joinType == JoinType.Request)
                {
                    if (!request.isConfirmUnion && request.isConfirmUser)
                    {
                        CreateRequest(request);
                        count++; 
                        continue;
                    }
                }
            }

            if (count == 0)
            {
                // textRequestCount.text = $"{titleName}";
                // textRequestCountButton.text = $"{titleName}";
                textRequestCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", "");
                textRequestCountButton.GetComponent<LocalizationParamsManager>().SetParameterValue("param", "");
                return;
            }
            
            // textRequestCount.text = $"{titleName} ({count})";
            // textRequestCountButton.text = $"{titleName} ({count})";
            textRequestCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({count})");
            textRequestCountButton.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({count})");
        }

        private void CreateRequest(UserUnionRequest request)
        {
            var slot = CreateSlot();

            if (slot.TryGetComponent(out CommunityCenterUnionSlotInvite invite))
            {
                //CreateUnionListener(slot);
                
                Debug.Log("request emblem: " + request.union.emblem);
                Debug.Log("request emblem id: " + request.union.emblem.emblemId);
                
                invite.SetInfo(request);
                invite.SetMaster(this);
                invite.SetEmblem(GetEmblemSprite(request.union.emblem));
                invite.CheckAcceptButton(_joinType);
            }
        }
        
        private void CreateUnionListener(GameObject slot)
        {
            slot.AddComponent<CommunityCenterUnionSlotListener>()
                .SetUnionDisplay(_unionDisplay);
        }

        public async UniTask<bool> AcceptUnionMember(int _request)
        {
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionRequestAcceptUser(_request);
            
            if (success)
            {
                await _unionsUpdate.UpdateUnionsTask();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"You are added to union members");
            }

            _buildingWindowStatus.OpenQuickLoading(false);
            
            return success;
        }
        
        public async UniTask<bool> RejectUnionMember(int _request)
        {
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionRequestDeleteUser(_request);
            
            if (success)
            {
                await _unionsUpdate.UpdateUnionsTask();
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"You are rejected from membership");
            }
            
            _buildingWindowStatus.OpenQuickLoading(false);

            return success;
        }

    }
}