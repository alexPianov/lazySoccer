using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsMarketPopup : MonoBehaviour
    {
        private MarketPopupStatus _status;
        [SerializeField] private StatusMarketPopup _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<MarketPopupStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _game;
        }
    }
}