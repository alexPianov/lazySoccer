using System;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnion;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate;
using LazySoccer.Status;
using LazySoccer.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionSettings
{
    public class CommunityCenterUnionSettings : MonoBehaviour
    {
        [Title("Refs")]
        [SerializeField] private CommunityCenterUnionEmblem unionEmblem;
        [SerializeField] private CommunityCenterUnion.CommunityCenterUnion communityCenterUnion;

        
        [Title("Toggle")]
        [SerializeField] private Toggle toggleOpenStatus;
        
        [Title("Buttons")]
        [SerializeField] private Button buttonSave;

        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private ManagerPlayerData _managerPlayerData;

        private void Awake()
        {
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            toggleOpenStatus.onValueChanged.AddListener(arg0 => OpenCloseStatus());
            buttonSave.onClick.AddListener(Save);
        }

        public void UpdateSettings()
        {
            Debug.Log("UpdateSettings Union Emblem Id: " + _managerPlayerData.PlayerHUDs.UnionEmblemId.Value);
            toggleOpenStatus.isOn =
                communityCenterUnion.CurrentUnion.policy == GeneralClassGETRequest.RecruitingPolicy.Open;

            unionEmblem.UpdateQuickTable();
            unionEmblem.quickTable.PickSlot(_managerPlayerData.PlayerHUDs.UnionEmblemId.Value);
            unionEmblem.UpdateScrollbars();
        }

        private void OpenCloseStatus()
        {
            Debug.Log("Is Open: " + toggleOpenStatus.isOn);
        }

        private async void Save()
        {
            Debug.Log("Save | Current emblem: " + unionEmblem.CurrentEmblemId 
                                                + " | Is Open: " + toggleOpenStatus.isOn);
            
            buttonSave.interactable = false;
            
            var request = new GeneralClassGETRequest.UnionUpdateRequest();

            request.emblemId = unionEmblem.CurrentEmblemId;
            
            if (toggleOpenStatus.isOn)
            {
                request.recruitingPolicy = GeneralClassGETRequest.RecruitingPolicy.Open;
            }
            else
            {
                request.recruitingPolicy = GeneralClassGETRequest.RecruitingPolicy.Close;
            }

            await _communityCenterTypesOfRequest.POST_UnionUpdate(request);
            communityCenterUnion.UpdateUnionEmblem(unionEmblem.CurrentEmblemId);
            await communityCenterUnion.RefreshUnion(StatusCommunityCenterUnion.Back);
            //await _communityCenterTypesOfRequest.GET_UserUnionProfile(false);

            ServiceLocator.GetService<BuildingWindowStatus>()
                .SetAction(StatusBuilding.CommunityCenterUnion);

            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Union changes are saved");
            
            buttonSave.interactable = true;
        }
    }
}