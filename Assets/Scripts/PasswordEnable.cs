using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Windows;
using static TMPro.TMP_InputField;
using UnityEngine.UI;

public class PasswordEnable : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private TMP_InputField _inputField;
    private void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        if (_toggle == null)
            _toggle = GetComponentInChildren<Toggle>(true);
        _toggle.onValueChanged.AddListener(EnablePassword);
    }

    public void EnablePassword(bool result)
    {
        if (result)
        {
            _inputField.contentType = TMP_InputField.ContentType.Password;
        }
        else
            _inputField.contentType = TMP_InputField.ContentType.Standard;

        _inputField.ReleaseSelection();
        _inputField.ForceLabelUpdate();
    }
}
