using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendsSlot : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] protected TMP_Text textRate;
        [SerializeField] protected TMP_Text textNickName;
        [SerializeField] protected TMP_Text textTeamName;
        
        [Title("Image")]
        [SerializeField] private Image imageAvatar;
        [SerializeField] private Image imageEmblem;

        public CommunityCenterFriends Friends { get; private set; }
        
        public GeneralClassGETRequest.User UserData { get; private set; }

        public void SetInfo(GeneralClassGETRequest.User user, int rate)
        {
            UserData = user;
            if(textRate) textRate.text = rate.ToString();
            if(textTeamName) textTeamName.text = user.team.name;
            if(textNickName) textNickName.text = user.userName;
        }

        public void SetEmblem(Sprite spriteAvatar, Sprite spriteEmblem)
        {
            if(imageEmblem && spriteEmblem) imageEmblem.sprite = spriteEmblem;
            if(imageAvatar && spriteAvatar) imageAvatar.sprite = spriteAvatar;
        }

        public void SetFriendList(CommunityCenterFriends friends)
        {
            Friends = friends;
        }
    }
}