using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsRegistrationCustomizeForm : MonoBehaviour
    {
        private RegistrationCustomizeStatusForm _office;
        [SerializeField] private StatusOfficeCustomizeForm _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _office = ServiceLocator.GetService<RegistrationCustomizeStatusForm>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _office.StatusAction = _game;
        }
    }
}