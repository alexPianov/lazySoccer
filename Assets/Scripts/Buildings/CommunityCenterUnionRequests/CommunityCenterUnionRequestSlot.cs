using System;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionRequests
{
    public class CommunityCenterUnionRequestSlot : CommunityCenterFriendsSlot
    {
        [Title("Button")]
        [SerializeField] private Button buttonAccept;
        [SerializeField] private Button buttonDelete;

        private void Awake()
        {
            buttonAccept.onClick.AddListener(AcceptMember);
            buttonDelete.onClick.AddListener(RejectMember);
        }

        [SerializeField] private CommunityCenterUnionRequests _communityCenterUnionRequests;
        public void SetMaster(CommunityCenterUnionRequests unionRequests)
        {
            Debug.Log("SetUnionRequests: " + unionRequests.name);
            _communityCenterUnionRequests = unionRequests;
        }

        private GeneralClassGETRequest.UserUnionRequest _request;
        public void SetRequest(GeneralClassGETRequest.UserUnionRequest request)
        {
            _request = request;

            if (!_request.isConfirmUser)
            {
                buttonAccept.interactable = false;
                buttonAccept.GetComponentInChildren<Localize>().SetTerm("3-General-Button-Invited");
            }
        }

        private async void AcceptMember()
        {
            buttonDelete.interactable = false;
            
            var success = await _communityCenterUnionRequests
                .AcceptUnionMember(_request.requestId, UserData.userName);

            if (!success)
            {
                buttonDelete.interactable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private async void RejectMember()
        {
            buttonDelete.interactable = false;
            
            var success = await _communityCenterUnionRequests
                .RejectUnionMember(_request.requestId, UserData.userName);

            if (!success)
            {
                buttonDelete.interactable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}