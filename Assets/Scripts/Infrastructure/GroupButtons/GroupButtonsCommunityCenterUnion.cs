using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsCommunityCenterUnion : MonoBehaviour
    {
        private CommunityCenterUnionStatus _status;
        
        public StatusCommunityCenterUnion _action;
        
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<CommunityCenterUnionStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _action;
        }
    }
}