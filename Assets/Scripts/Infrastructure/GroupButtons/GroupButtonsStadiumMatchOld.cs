using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsStadiumMatchOld : MonoBehaviour
    {
        private StadiumStatusMatchOld _stadium;
        [SerializeField] private StatusStadiumMatchOld _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _stadium = ServiceLocator.GetService<StadiumStatusMatchOld>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _stadium.StatusAction = _game;
        }
    }
}