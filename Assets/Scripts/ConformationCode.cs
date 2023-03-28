using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;
using I2.Loc;
using Scripts.Infrastructure.Utils;

public class ConformationCode : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private FindValidationContainer _validationField;

    [SerializeField] private TMP_InputField inputCode;
    [SerializeField] private Button button;
    [SerializeField] private Button buttonResend;
    private string code;

    private AuthTypesOfRequests _networkingManager;
    private ManagerPlayerData _managerPlayerData;

    private void Start()
    {
        _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
        _networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();

        button.onClick.AddListener(SendMailConfirm);
        buttonResend.onClick.AddListener(ResendCode);
        inputCode.onValueChanged.AddListener(UpdateCode);
        
        _validationField = GetComponent<FindValidationContainer>();
    }
    
    public void UpdateCode(string text)
    {
        inputCode.text = _managerPlayerData.PlayerData.Code = code = text;
        Validation();
    }

    public void ActiveScene()
    {
        _validationField.ClearValidationFields();
        
        var Email = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Email;
        
        if (Email != null)
            m_Text.GetComponent<LocalizationParamsManager>().SetParameterValue("param", Email);
    }

    private void Validation()
    {
        if(ValidationUtils.StringCodeValid(code))
        {
            button.interactable = true;
        }
        else
            button.interactable = false;
    }

    private async void SendMailConfirm()
    {
        if(_validationField.isRatification())
           await SendMessage(AuthOfRequests.MailConfirm);
    }

    private async void SendResetCode()
    {
        if (_validationField.isRatification())
        {
            var success = await _networkingManager.SendMailCode();

            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Code is send to EMAIL", Param1: _managerPlayerData.PlayerData.Email);
                
                UpdateCode(null);
            }
        }
    }

    private async void ResendCode()
    {
        var success = await _networkingManager.SendMailCode();

        if (success)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Code is resend to EMAIL", Param1: _managerPlayerData.PlayerData.Email);
            UpdateCode(null);
        }
    }
    
    private async UniTask SendMessage(AuthOfRequests type)
    {
        await _networkingManager.SendMessage(type);
    }
}
