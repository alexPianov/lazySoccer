using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsCommunityCenterFriend : MonoBehaviour
    {
        private CommunityCenterFriendStatus _status;
        [SerializeField] private StatusCommunityCenterFriend _action;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<CommunityCenterFriendStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _action;
        }
    }
}