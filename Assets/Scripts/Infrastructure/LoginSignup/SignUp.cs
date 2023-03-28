using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;
using LazySoccer.Utils;
using Scripts.Infrastructure.Utils;

public class SignUp : MonoBehaviour
{
    [SerializeField] private Button signUp;
    [SerializeField] private Toggle togglePolitics;

    [SerializeField] private FindValidationContainer _valid;

    private ManagerPlayerData playerData;
    private AuthTypesOfRequests _networking;
    private GeneralValidation _validation;

    public void Start()
    {
        playerData = ServiceLocator.GetService<ManagerPlayerData>();
        _networking = ServiceLocator.GetService<AuthTypesOfRequests>();
        _validation = ServiceLocator.GetService<GeneralValidation>();

        signUp.onClick.AddListener(CheckButton);
    }
    
    public void CheckForm()
    {
        if (ValidationUtils.StringNicknameValid(nickName) && 
            ValidationUtils.StringEmailValid(email) &&
            ValidationUtils.StringPasswordValid(pass) && 
            ValidationUtils.StringPasswordValid(passRepeat) &&
            togglePolitics.isOn)
        {
            signUp.interactable = true;
        }
        else
        {
            signUp.interactable = false;
        }
    }
    [Button]
    public void CheckButton()
    {
        if (_valid.isRatification())
        {
            if (_validation.TextError
                (ErrorsStatus.PasswordAndConfirmPassword, pass, passRepeat).Length == 0)
            {
                SaveCredentials();
                SendFileToServer();
            }
        }          
    }
    
    private async UniTask SendFileToServer()
    {
        var success = await _networking.SignUp();

        if (!success)
        {
            ClearCredentials();
        }
        else
        {
            playerData.PlayerHUDs.NameTeam.Value = "";
            playerData.PlayerHUDs.NameShortTeam.Value = "";
        }
    }

    public void NickNamePlayer(string value)
    {
        nickName = value;
        CheckForm();
    }
    public void EmailPlayer(string value)
    {
        email = value;
        CheckForm();
    }
    public void PasswordPlayer(string value)
    {
        pass = value;
        CheckForm();
    }
    public void PasswordConfirm(string value)
    {
        passRepeat = value;
        CheckForm();
    }

    private string nickName = "";
    private string email = "";
    private string pass = "";
    private string passRepeat = "";
    private void SaveCredentials()
    {
        playerData.PlayerData.UserName = nickName;
        playerData.PlayerData.Email = email;
        playerData.PlayerData.Password = pass;
        playerData.PlayerData.PasswordConfirm = passRepeat;
    }
    
    private void ClearCredentials()
    {
        playerData.PlayerData.UserName = "";
        playerData.PlayerData.Email = "";
        playerData.PlayerData.Password = "";
        playerData.PlayerData.PasswordConfirm = "";
    }
}
