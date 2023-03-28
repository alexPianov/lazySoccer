using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LazySoccer.Network;
using Scripts.Infrastructure.Utils;

public class LogInManager : MonoBehaviour
{
    [SerializeField] private ManagerPlayerData _playerData;
    [SerializeField] private AuthTypesOfRequests _networkingManager;
    [SerializeField] private FindValidationContainer _validationContainer;
    
    public void Email(string value) => _playerData.PlayerData.Email = value;
    public void Password(string value) => _playerData.PlayerData.Password = value;

    [SerializeField] private Button button;

    private void Start()
    {
       _playerData = ServiceLocator.GetService<ManagerPlayerData>();
        _networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();
        button.onClick.AddListener(OnLogIn);
        FirstPlayApp();
    }
    
    private void OnEnable()
    {
        button.interactable = false;
    }

    public void Verification()
    {
        if(ValidationUtils.StringEmailValid(_playerData.PlayerData.Email) &&
           ValidationUtils.StringPasswordValid(_playerData.PlayerData.Password)) 
        {
            button.interactable = true;
        }
    }
    public void OnLogIn()
    {
        if (_validationContainer.isRatification())
            StartLogin().Forget();
    }
    
    private async UniTask StartLogin()
    {
        await _networkingManager.SendMessage(AuthOfRequests.Login);
    }
    
    public void FirstPlayApp()
    {
        if (_playerData.PlayerData.Email == null)
        {
            Debug.LogError("Email is null!");
        }

        if(ValidationUtils.StringEmailValid(_playerData.PlayerData.Email) &&
           ValidationUtils.StringPasswordValid(_playerData.PlayerData.Password)) 
        {
            _validationContainer.SendText(ErrorsStatus.Email, _playerData.PlayerData.Email);
            _validationContainer.SendText(ErrorsStatus.PasswordLogin, _playerData.PlayerData.Password);
            Verification();
        }
    }
}
