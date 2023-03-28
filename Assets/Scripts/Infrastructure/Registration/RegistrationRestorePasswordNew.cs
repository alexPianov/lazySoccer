using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationRestorePasswordNew : MonoBehaviour
    {
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttomBack;
        
        private AuthenticationStatus _authentication;
        private FindValidationContainer _container;
        private AuthTypesOfRequests networkingManager;
        private ManagerPlayerData playerData;

        public void NewPassword(TMP_InputField input) 
        { 
            playerData.PlayerData.Password = input.text;
            FirstCheck();
        }

        private void Start()
        {
            _authentication = ServiceLocator.GetService<AuthenticationStatus>();
            playerData = ServiceLocator.GetService<ManagerPlayerData>();
            networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();
            _container = GetComponent<FindValidationContainer>();
            
            buttonConfirm.onClick.AddListener(SendCodeMesseng);
            buttomBack.onClick.AddListener(Back);
        }

        private void SendCodeMesseng() {
            if (_container.isRatification())
                CodeMessage().Forget();
        }

        private async UniTask CodeMessage()
        {
            var success = await networkingManager.RestorePassword();
            
            if (success)
            {
                //_authentication.StatusAction = StatusAuthentication.LogIn;
            }
        }

        private void Back()
        {
            _authentication.StatusAction = StatusAuthentication.RestorePasswordCode;
        }

        private void FirstCheck()
        {
            if (playerData.PlayerData.Password.Length != 0)
                buttonConfirm.interactable = true;
            else
                buttonConfirm.interactable = false;
        }
    }
}