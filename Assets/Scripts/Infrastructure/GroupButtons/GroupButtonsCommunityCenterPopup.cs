using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsCommunityCenterPopup : MonoBehaviour
    {
        private CommunityCenterPopupStatus _status;
        [SerializeField] private StatusCommunityCenterPopup _action;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<CommunityCenterPopupStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _action;
        }
    }
}