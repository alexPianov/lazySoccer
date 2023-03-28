using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    [RequireComponent(typeof(Button))]
    public class CommunityCenterFriendsSlotListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddAsPickedPlayer);
        }

        private CommunityCenterFriend.CommunityCenterFriend _centerFriend;
        public void SetFriendTable(CommunityCenterFriend.CommunityCenterFriend table)
        {
            _centerFriend = table;
        }

        private void AddAsPickedPlayer()
        {
            _centerFriend.ShowFriendData(GetComponent<CommunityCenterFriendsSlot>().UserData);
        }
    }
}