using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationRestorePassword : MonoBehaviour
    {
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttonBack;
        
        [SerializeField] private TMP_InputField inputEmail;
        
        private FindValidationContainer _container;
        private AuthenticationStatus _authentication;
        
        private AuthTypesOfRequests networkingManager;
        private ManagerPlayerData playerData;

        public void Input(TMP_InputField input) 
        { 
            playerData.PlayerData.Email = input.text;
            FirstCheck();
        }

        private void Start()
        {
            _authentication = ServiceLocator.GetService<AuthenticationStatus>();
            playerData = ServiceLocator.GetService<ManagerPlayerData>();
            networkingManager = ServiceLocator.GetService<AuthTypesOfRequests>();
            _container = GetComponent<FindValidationContainer>();
            
            buttonConfirm.onClick.AddListener(SendCodeMessage);
            buttonBack.onClick.AddListener(Back);
            
            inputEmail.text = playerData.PlayerData.Email;
        }

        private void SendCodeMessage() {
            if (_container.isRatification())
                CodeMessage().Forget();
        }

        public void ResendCode()
        {
            CodeMessage().Forget();
        }

        private async UniTask CodeMessage()
        {
            var success = await networkingManager.SendMailCode();
            
            if (success)
            {
                _authentication.StatusAction = StatusAuthentication.RestorePasswordCode;
            }
        }

        private void Back()
        {
            _authentication.StatusAction = StatusAuthentication.LogIn;
        }

        private void FirstCheck()
        {
            if (playerData.PlayerData.Email.Length != 0)
                buttonConfirm.interactable = true;
            else
                buttonConfirm.interactable = false;
        }
    }
}