using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtonsAuth : MonoBehaviour
{
    private AuthenticationStatus _status;
    [SerializeField] private StatusAuthentication _game;
    private Button _button;
    void OnEnable()
    {
        _button = GetComponent<Button>();
        _status = ServiceLocator.GetService<AuthenticationStatus>();

        _button.onClick.AddListener(LoadUIStatus);
    }

    private void LoadUIStatus() => _status.StatusAction = _game;
}
