using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.SceneLoading.Validator;
using LazySoccer.Status;
using Playstel;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.Popup
{
    public class ErrorPopup : MonoBehaviour
    {
        [SerializeField] private GameObject prefabMessage;
        [SerializeField] private Transform container;

        private GeneralValidation _generalValidation;
        private GeneralPopupMessage _generalPopupMessage;
        private ManagerPlayerData _playerData;
        private AuthenticationStatus _authenticationStatus;
        private ManagerAudio _managerAudio;
        private CancellationTokenSource _cancellationTokenSource;
        
        private void Start()
        {
            _playerData = ServiceLocator.GetService<ManagerPlayerData>();
            _authenticationStatus = ServiceLocator.GetService<AuthenticationStatus>();
            _managerAudio = ServiceLocator.GetService<ManagerAudio>();
            
            _generalValidation = ServiceLocator.GetService<GeneralValidation>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            _generalValidation.OnActionErrorsStatus += Error;
            _generalPopupMessage.OnActionMessageStatus += Message;
        }

        private void OnDestroy()
        {
            _generalValidation.OnActionErrorsStatus -= Error;
            _generalPopupMessage.OnActionMessageStatus -= Message;
        }
        
        private void Message(string messageText, bool positive, string param)
        {
            Debug.Log("Message: " + messageText);

            var slot = Instantiate(prefabMessage, container);

            if (slot.TryGetComponent(out PopupMessageSlot slotMessage))
            {
                slotMessage.SetInfo(messageText, positive, param);
            }
            
            Activities(messageText);
            Sound(positive);
        }

        private void Error(ErrorsStatus errors, string errorText, string param)
        {
            Debug.Log("Error: " + errorText + " | Status: " + errors);

            Message(errorText, false, param);
        }

        private void Sound(bool positive)
        {
            if (positive)
            {
                _managerAudio.PlaySound(ManagerAudio.AudioSound.Success);
            }
            else
            {
                _managerAudio.PlaySound(ManagerAudio.AudioSound.Error);
            }
        }

        private void Activities(string errorText)
        {   
            if (errorText == $"The account: { _playerData.PlayerData.Email } is not confirmed")
            {
                Debug.Log("Enter code");
                _authenticationStatus.SetAction(StatusAuthentication.EnterCode);
            }
            
            if (errorText == $"The_account_is_not_confirmed")
            {
                Debug.Log("Enter code");
                _authenticationStatus.SetAction(StatusAuthentication.EnterCode);
            }
        }
    }
}
