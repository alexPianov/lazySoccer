using System.Collections;
using System.Collections.Generic;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtonsOfficeCustomize : MonoBehaviour
{
    private OfficeCustomizeStatus _office;
    [SerializeField] private StatusOfficeCustomize _game;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _office = ServiceLocator.GetService<OfficeCustomizeStatus>();

        _button.onClick.AddListener(LoadUIStatus);
    }

    private void LoadUIStatus()
    {
        _office.StatusAction = _game;
    }
}
