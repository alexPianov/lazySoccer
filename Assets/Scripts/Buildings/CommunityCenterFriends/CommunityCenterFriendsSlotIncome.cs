using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsSlotIncome : CommunityCenterFriendsSlot
    {
        [Title("Buttons")]
        public Button buttonAccept;
        public Button buttonReject;
        
        private void Start()
        {
            buttonAccept.onClick.AddListener(AcceptFriend);
            buttonReject.onClick.AddListener(RejectFriend);
        }

        private async void AcceptFriend()
        {
            buttonAccept.interactable = false;
            
            var success = await Friends.FriendsRequests.FriendshipConfirm(UserData);

            if (!success)
            {
                buttonAccept.interactable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private async void RejectFriend()
        {
            var result = await Friends.QuestionPopup.OpenQuestion
            ($"USER will be rejected", "Are you sure?", "Reject", Param1: UserData.userName);
            
            if(!result) return;
            
            buttonAccept.interactable = false;
            
            var success = await Friends.FriendsRequests.FriendshipReject(UserData);

            if (!success)
            {
                buttonAccept.interactable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}