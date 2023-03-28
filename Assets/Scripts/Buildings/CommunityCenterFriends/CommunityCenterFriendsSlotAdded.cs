using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsSlotAdded : CommunityCenterFriendsSlot
    {
        [Title("Buttons")]
        public Button buttonMessage;
        public Button buttonDelete;
        
        private void Start()
        {
            if(buttonMessage) buttonMessage.onClick.AddListener(FriendMessage);
            buttonDelete.onClick.AddListener(DeleteFriend);
        }
        
        private void FriendMessage()
        {
            Debug.Log("Start messaging");
        }
        
        private async void DeleteFriend()
        {
            var result = await Friends.QuestionPopup.OpenQuestion
                ($"USER will be deleted from friendist", 
                    "Are you sure?", "Delete", Param1: UserData.userName);
            
            if(!result) return;

            buttonDelete.interactable = false;
            
            var success = await Friends.FriendsRequests.DeleteFriend(UserData);

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