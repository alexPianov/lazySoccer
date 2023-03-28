using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseModalWindows : MonoBehaviour
{
    private ModalWindowStatus _modalWindowStatus;

    [SerializeField] protected Button buttonClose;
    [SerializeField] protected Button buttonSave;

    protected virtual void Start()
    {
        _modalWindowStatus = ServiceLocator.GetService<ModalWindowStatus>();
        
        if(buttonClose) buttonClose.onClick.AddListener(ClickClose);
        if(buttonSave) buttonSave.onClick.AddListener(ClickSave);
    }
    
    protected virtual void ClickClose()
    {
        _modalWindowStatus.SetAction(StatusModalWindows.None);
    }
    
    protected virtual void ClickSave()
    {

    }
    
    protected void ActiveSaveButton(bool active)
    {
        buttonSave.interactable = active;
    }

    protected void ClearFields()
    {
        var inputFields = GetComponentsInChildren<TMP_InputField>();

        foreach (var inputField in inputFields)
        {
            inputField.text = "";

            if (inputField.TryGetComponent(out PasswordEnable passwordEnable))
            {
                passwordEnable.EnablePassword(true);
            }
        }
    }
    
}
