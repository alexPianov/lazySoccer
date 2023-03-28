using System;
using LazySoccer.Network.Get;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterLeaderSlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text textRate;
        [SerializeField] private TMP_Text textTeamName;
        [SerializeField] private TMP_Text textPower;
        [SerializeField] private TMP_Text textPerformance;
        [SerializeField] private TMP_Text textBalance;
        [SerializeField] private Image imageEmblem;
        
        public WorldRatingUser userData { get; private set; }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenFriendWindow);
        }

        public void SetInfo(WorldRatingUser user, int rate)
        {
            userData = user;

            textRate.text = $"{rate}.";
            textTeamName.text = user.teamName;
            textPower.text = user.power.ToString();
            textPerformance.text = user.performance.ToString();
            textBalance.text = user.balance.ToString();
        }

        private CommunityCenterFriend.CommunityCenterFriend _communityCenterFriend;
        public void SetFriendWindow(CommunityCenterFriend.CommunityCenterFriend friendDisplay)
        {
            _communityCenterFriend = friendDisplay;
        }

        public void OpenFriendWindow()
        {
            _communityCenterFriend.ShowFriendData(userData.teamName);
        }

        public void SetEmblem(Sprite sprite)
        {
            if(imageEmblem && sprite) imageEmblem.sprite = sprite;
        }
    }
}