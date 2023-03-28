using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.UI
{
    public class GroupButtonsOfficeCustomizeForm : MonoBehaviour
    {
        private OfficeCustomizeStatusForm _office;
        [SerializeField] private StatusOfficeCustomizeForm _game;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _office = ServiceLocator.GetService<OfficeCustomizeStatusForm>();

            _button.onClick.AddListener(LoadUIStatus);
        }

        private void LoadUIStatus()
        {
            _office.StatusAction = _game;
        }
    }
}