using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsSlotOutcome : CommunityCenterFriendsSlot
    {
        [Title("Buttons")]
        public Button buttonCancelRequest;
        
        private void Start()
        {
            if(buttonCancelRequest) buttonCancelRequest.onClick.AddListener(CancelRequest);
        }

        private void CancelRequest()
        {
            Debug.Log("Cancel request");
        }
    }
}