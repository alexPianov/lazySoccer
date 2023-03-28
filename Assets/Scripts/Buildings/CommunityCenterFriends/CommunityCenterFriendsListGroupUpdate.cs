using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsListGroupUpdate : MonoBehaviour
    {
        public List<CommunityCenterFriendsList> lists = new ();

        public async void UpdateUserlists()
        {
            foreach (var list in lists)
            {
                await list.UpdateUserlist();
            }
        }
    }
}