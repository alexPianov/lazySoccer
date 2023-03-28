using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsCreateTeamOptions : MonoBehaviour
    {
        private CreateTeamOptionsStatus _status;
        [SerializeField] private StatusCreateTeamOptions _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<CreateTeamOptionsStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _game;
        }
    }
}