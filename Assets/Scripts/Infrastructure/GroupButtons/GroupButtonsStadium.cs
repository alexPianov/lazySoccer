using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsStadium : MonoBehaviour
    {
        private StadiumStatus _stadium;
        [SerializeField] private StatusStadium _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _stadium = ServiceLocator.GetService<StadiumStatus>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _stadium.StatusAction = _game;
        }
    }
}