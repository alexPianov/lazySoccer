using System;
using LazySoccer.Popup;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionSlotInvite : MonoBehaviour
    {
        [Title("Buttons")] 
        [SerializeField] private Button buttonAccept;
        [SerializeField] private Button buttonDelete;
        
        [Title("Text")] 
        [SerializeField] private TMP_Text textUnionName;
        
        [Title("Image")]
        [SerializeField] private Image imageEmblem;

        private GeneralPopupMessage _generalPopupMessage;

        private void Start()
        {
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            buttonAccept.onClick.AddListener(Accept);
            buttonDelete.onClick.AddListener(Delete);
        }

        public UserUnionRequest UserUnionRequest;
        
        public void SetInfo(UserUnionRequest unionRequest)
        {
            UserUnionRequest = unionRequest;

            if (textUnionName) textUnionName.text = unionRequest.union.name;
        }

        public void SetEmblem(Sprite sprite)
        {
            if (imageEmblem && sprite) imageEmblem.sprite = sprite;
        }

        private CommunityCenterUnionsJoinRequest _joinRequest;
        public void SetMaster(CommunityCenterUnionsJoinRequest request)
        {
            _joinRequest = request;
        }

        public void CheckAcceptButton(CommunityCenterUnionsJoinRequest.JoinType _joinType)
        {
            if (_joinType == CommunityCenterUnionsJoinRequest.JoinType.Request)
            {
                buttonAccept.gameObject.SetActive(false);
            }
        }
        
        private async void Accept()
        {
            if (_joinRequest.GetBuildingLevel() < 5)
            {
                _generalPopupMessage.ShowInfo("Upgrade community center to level 5 to gain access");
                return;
            }
            
            buttonAccept.interactable = false;

            Debug.Log("Accept");
            var success = await _joinRequest.AcceptUnionMember(UserUnionRequest.requestId);

            if (success)
            {
                
            }
            
            if(buttonAccept) buttonAccept.interactable = true;
        }

        private async void Delete()
        {
            buttonAccept.interactable = false;
            
            var success = await _joinRequest.RejectUnionMember(UserUnionRequest.requestId);

            if (success)
            {
                
            }
            
            if(buttonAccept) buttonAccept.interactable = true;
        }
    }
}