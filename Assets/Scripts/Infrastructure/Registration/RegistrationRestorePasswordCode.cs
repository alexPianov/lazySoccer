using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationRestorePasswordCode : MonoBehaviour
    {
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttonBack;
        
        private AuthenticationStatus _authentication;
        private FindValidationContainer _container;
        private ManagerPlayerData playerData;

        public void Code(TMP_InputField input) 
        { 
            playerData.PlayerData.Code = input.text;
            FirstCheck();
        }

        private void Start()
        {
            _authentication = ServiceLocator.GetService<AuthenticationStatus>();
            playerData = ServiceLocator.GetService<ManagerPlayerData>();
            _container = GetComponent<FindValidationContainer>();
            
            buttonConfirm.onClick.AddListener(Next);
            buttonBack.onClick.AddListener(Back);
        }

        private void Next() 
        {
            if (_container.isRatification())
            {
                _authentication.StatusAction = StatusAuthentication.CreateNewPassword;
            }
        }

        private void Back()
        {
            _authentication.StatusAction = StatusAuthentication.RestorePassword;
        }

        private void FirstCheck()
        {
            if (playerData.PlayerData.Code.Length != 0)
                buttonConfirm.interactable = true;
            else
                buttonConfirm.interactable = false;
        }
    }
}