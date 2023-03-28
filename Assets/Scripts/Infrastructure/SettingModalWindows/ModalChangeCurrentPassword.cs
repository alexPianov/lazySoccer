using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;
using LazySoccer.Popup;
using LazySoccer.Status;
using Scripts.Infrastructure.Utils;

public class ModalChangeCurrentPassword : BaseModalWindows
{
    [Title("Input")]
    [SerializeField] private TMP_InputField inputFieldPassword;
    [SerializeField] private TMP_InputField inputFieldNewPassword;
    [SerializeField] private TMP_InputField inputFieldNewPasswordConfirm;

    [SerializeField] private FindValidationContainer _valid;
    [SerializeField] private ManagerPlayerData _managerPlayerData;
    
    protected override void Start()
    {
        _valid = GetComponent<FindValidationContainer>();
        _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
        
        base.Start();
        
        inputFieldPassword.onValueChanged.AddListener(InputPassword);
        inputFieldNewPassword.onValueChanged.AddListener(InputNewPassword);
        inputFieldNewPasswordConfirm.onValueChanged.AddListener(InputNewPasswordConfirm);
    }
    
    public void ClearFields()
    {
        currentPassword = "";
        newPassword = "";
        newPasswordConfirm = "";
        
        base.ClearFields();
        
        CheckForm();
    }

    private string currentPassword;
    private void InputPassword(string value)
    {
        currentPassword = value;
        CheckForm();
    }

    private string newPassword;
    private void InputNewPassword(string value)
    {
        newPassword = value;
        CheckForm();
    }

    private string newPasswordConfirm;
    private void InputNewPasswordConfirm(string value)
    {
        newPasswordConfirm = value;
        CheckForm();
    }
    
    private void CheckForm()
    {
        if(ValidationUtils.StringPasswordValid(currentPassword) &&
           ValidationUtils.StringPasswordValid(newPassword) &&
           ValidationUtils.StringPasswordValid(newPasswordConfirm))
        {
            ActiveSaveButton(true);
        }
        else
        {
            ActiveSaveButton(false);
        }
    }
    protected async override void ClickSave()
    {
        if(!_valid.isRatification()) return;

        if (newPassword != newPasswordConfirm)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Passwords don't match", false);
            
            inputFieldNewPasswordConfirm.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }
        
        if (newPassword == currentPassword)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("The new password must not match the old one", false);
            
            inputFieldNewPassword.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }

        if (_managerPlayerData.PlayerData.Password != currentPassword)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("The password doesn't match", false);
            
            inputFieldPassword.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }
        
        var result = await ServiceLocator.GetService<ChangeUserTypesOfRequests>()
            .ChangePassword(currentPassword, newPassword);
        
        if (result)
        {
            ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Password = newPassword;
            ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Password changed");
            ServiceLocator.GetService<ModalWindowStatus>().SetAction(StatusModalWindows.None);
        }
        else
        {
            inputFieldNewPassword.GetComponent<ValidationField>().ActiveErrorHighlight();
        }
    }
}
