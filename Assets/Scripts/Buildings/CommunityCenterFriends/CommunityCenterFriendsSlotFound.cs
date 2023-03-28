using System;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsSlotFound : CommunityCenterFriendsSlot
    {
        [Title("Buttons")]
        public Button buttonAdd;
        
        [Title("Text")]
        public TMP_Text textStatus;

        private void Start()
        {
            buttonAdd.onClick.AddListener(AddFriend);
        }

        public async void AddFriend()
        {
            buttonAdd.interactable = false;
            
            var success = await Friends.FriendsRequests.FriendshipRequest(UserData);
            
            if (!success)
            {
                buttonAdd.interactable = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ButtonStatus(bool active, string text)
        {
            buttonAdd.interactable = active;
            textStatus.GetComponent<Localize>().SetTerm("3-CommCenter-User-Button-" + text);
        }
    }
}