using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtonsOfficePlayer : MonoBehaviour
{
    private OfficePlayerStatus _status;
    [SerializeField] private StatusOfficePlayer _game;
    private Button _button;
    void Start()
    {
        _button = GetComponent<Button>();
        _status = ServiceLocator.GetService<OfficePlayerStatus>();

        _button.onClick.AddListener(LoadUIStatus);
    }

    private void LoadUIStatus() => _status.StatusAction = _game;
}
