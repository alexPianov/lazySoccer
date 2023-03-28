using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsStadiumTournament : MonoBehaviour
    {
        private StadiumStatusTournament _stadium;
        [SerializeField] private StatusStadiumTournament _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _stadium = ServiceLocator.GetService<StadiumStatusTournament>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _stadium.StatusAction = _game;
        }
    }
}