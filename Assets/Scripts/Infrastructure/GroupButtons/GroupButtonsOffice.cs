using System;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtonsOffice : MonoBehaviour
{
    private OfficeStatus _office;
    [SerializeField] private StatusOffice _game;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _office = ServiceLocator.GetService<OfficeStatus>();

        _button.onClick.AddListener(LoadUIStatus);
    }

    private void LoadUIStatus()
    {
        _office.StatusAction = _game;
    }
}
