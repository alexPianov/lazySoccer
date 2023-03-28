using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsMarket : MonoBehaviour
    {
        private MarketStatus _status;
        [SerializeField] private StatusMarket _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _status = ServiceLocator.GetService<MarketStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _status.StatusAction = _game;
        }
    }
}