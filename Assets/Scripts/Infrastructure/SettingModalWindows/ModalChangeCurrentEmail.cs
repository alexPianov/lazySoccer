using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.ModalWindows;
using LazySoccer.Status;
using Scripts.Infrastructure.Utils;
using TMPro;
using UnityEngine;

public class ModalChangeCurrentEmail : BaseModalWindows
{
    [Header("Fields")]
    public TMP_InputField inputPassword;
    public TMP_InputField inputNewEmail;
    public TMP_Text textEmail;
    
    [Header("Refs")]
    public ModalChangeCurrentEmailCode emailCode;
    
    [SerializeField] private FindValidationContainer _valid;
    private ManagerPlayerData _playerData;

    private void Start()
    {
        _valid = GetComponent<FindValidationContainer>();
        _playerData = ServiceLocator.GetService<ManagerPlayerData>();
        
        base.Start();
        
        inputPassword.onValueChanged.AddListener(EditPasswordField);
        inputNewEmail.onValueChanged.AddListener(EditNewEmailField);
    }

    public void UpdateTextEmail()
    {
        textEmail.text = $"({_playerData.PlayerData.Email})";
    }

    public void ClearFields()
    {
        password = "";
        newEmail = "";

        base.ClearFields();
        
        CheckForm();
    }

    private string password;
    private void EditPasswordField(string text)
    {
        password = text;
        CheckForm();
    }
    
    private string newEmail;
    private void EditNewEmailField(string text)
    {
        newEmail = text;
        CheckForm();
    }

    public void CheckForm()
    {
        if (ValidationUtils.StringEmailValid(newEmail) &&
            ValidationUtils.StringPasswordValid(password))
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
        if (!_valid.isRatification()) return;

        if (newEmail == _playerData.PlayerData.Email)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("The new email must not match the old one", false);
            
            inputNewEmail.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }
        
        if (password != _playerData.PlayerData.Password)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("The current password field must match the account password", false);

            inputPassword.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }
        
        var result = await ServiceLocator.GetService<ChangeUserTypesOfRequests>()
            .SendCodeToNewMail(newEmail);
        
        if (result)
        {
            ServiceLocator.GetService<ModalWindowStatus>()
                .SetAction(StatusModalWindows.ChangeEmailCode);

            if (emailCode)
            {
                emailCode.EditNewEmailField(newEmail);
                emailCode.EditPasswordField(password);
            }
            else
            {
                Debug.LogError("Failed to find ModalChangeCurrentEmailCode");
            }
        }
        else
        {
            inputNewEmail.GetComponent<ValidationField>().ActiveErrorHighlight();
        }
    }
}
