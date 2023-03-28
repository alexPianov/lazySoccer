using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LazySoccer.Network;
using Cysharp.Threading.Tasks;

public class LayoutRestorePassword : MonoBehaviour
{
    [SerializeField] private FindValidationContainer _container;
    [SerializeField] private Button _confirm;


    private AuthTypesOfRequests networkingManager;
    private ManagerPlayerData playerData;

    public void Email(string value) 
    { 
        playerData.PlayerData.Email = value;
        FirstCheck();
    }
    public void Password(string value) 
    { 
        playerData.PlayerData.Password = value;
        FirstCheck();
    }

    private void Start()
    {
        playerData = ServiceLocator.GetService<ManagerPlayerData>();
        networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();
        _container = GetComponent<FindValidationContainer>();
        _confirm.onClick.AddListener(SendCodeMesseng);
    }

    private void SendCodeMesseng() {
        if (_container.isRatification())
            CodeMessage().Forget();
    }

    private async UniTask CodeMessage()
    {
        await networkingManager.SendMessage(AuthOfRequests.SendMailCode);
    }

    private void FirstCheck()
    {
        if (playerData.PlayerData.Email.Length != 0 && playerData.PlayerData.Password.Length != 0)
            _confirm.interactable = true;
        else
            _confirm.interactable = false;
    }
}
