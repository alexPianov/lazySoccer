using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileLayoutSetting : MonoBehaviour
{
    private ModalWindowStatus _statusModalWin;
    private PopupStatus _statusPopup;

    [SerializeField] private Button back;
    [SerializeField] private Button close;

    [SerializeField] private Button ChangeName;
    [SerializeField] private Button ChangePayment;
    [SerializeField] private Button ChangeLanguage;
    [SerializeField] private Button ChangeEmail;
    [SerializeField] private Button ChangePassword;


    private void Start()
    {
        _statusModalWin = ServiceLocator.GetService<ModalWindowStatus>();
        _statusPopup = ServiceLocator.GetService<PopupStatus>();

        back.onClick.AddListener(ClickBack);
        close.onClick.AddListener(ClickClose);

        ChangeName.onClick.AddListener(ClickChangeName);
        ChangePayment.onClick.AddListener(ClickChangePayment);
        ChangeLanguage.onClick.AddListener(ClickChangeLanguage);
        ChangeEmail.onClick.AddListener(ClickChangeEmail);
        ChangePassword.onClick.AddListener(ClickChangePassword);
    }

    private void ClickClose()
    {
        _statusPopup.SetAction(StatusPopup.None);
    }
    
    private void ClickBack()
    {
        _statusPopup.SetAction(StatusPopup.Back);
    }
    private void ClickChangeName()
    {
    }

    private void ClickChangePayment()
    {
    }
    private void ClickChangeLanguage()
    {
    }

    private void ClickChangeEmail()
    {
    }
    private void ClickChangePassword()
    {
    }
}
