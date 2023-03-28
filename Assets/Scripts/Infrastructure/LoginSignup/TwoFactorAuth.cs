using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;
using LazySoccer.Status;
using TMPro;

public class TwoFactorAuth : MonoBehaviour
{
    [SerializeField] private FindValidationContainer _container;
    [SerializeField] private Button _confirm;
    [SerializeField] private Button _back;
    [SerializeField] private TMP_InputField _inputFieldPinCode;

    private AuthTypesOfRequests networkingManager;
    private ManagerPlayerData playerData;
    private AuthenticationStatus _authentication;

    // Start is called before the first frame update
    void Start()
    {
        _authentication = ServiceLocator.GetService<AuthenticationStatus>();
        playerData = ServiceLocator.GetService<ManagerPlayerData>();
        networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();
        _container = GetComponent<FindValidationContainer>();
        
        _inputFieldPinCode.onValueChanged.AddListener(Pin);
        _confirm.onClick.AddListener(SendCodeMesseng);
        _back.onClick.AddListener(Back);
    }

    private void SendCodeMesseng()
    {
        if (_container.isRatification())
            CodeMessage().Forget();
    }

    private void Back()
    {
        _authentication.StatusAction = StatusAuthentication.LogIn;
    }
    
    private async UniTask CodeMessage()
    {
        await networkingManager.SendMessage(AuthOfRequests.TwoFactorAuth);
    }
    
    public void Pin(string inputText)
    {
        playerData.PlayerData.Pin = inputText;
        FirstCheck(inputText);
    }
    
    private void FirstCheck(string value)
    {
        if (value.Length != 0)
            _confirm.interactable = true;
        else
            _confirm.interactable = false;
    }
}
