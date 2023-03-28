using System;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.Status;
using Scripts.Infrastructure.Utils;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalTwoFaPin : BaseModalWindows
    {
        public GameObject panelPin;
        public TMP_InputField inputPin;
        public TMP_Text buttonActiveStatus;
        
        private ManagerPlayerData _playerData;
        
        private void Start()
        {
            _playerData = ServiceLocator.GetService<ManagerPlayerData>();
            inputPin.onValueChanged.AddListener(InputPin);
            
            base.Start();
        }

        public void ActiveWindow(bool state)
        {
            ClearFields();
            panelPin.SetActive(state);

            if (_playerData.PlayerData.TwoFactorAuth)
            {
                buttonActiveStatus.GetComponent<Localize>().SetTerm("3-2FA-Button-Disable");
                //buttonActiveStatus.text = "Disable 2FA";
            }
            else
            {
                buttonActiveStatus.GetComponent<Localize>().SetTerm("3-2FA-Button-Enable");
                //buttonActiveStatus.text = "Enable 2FA";
            }
        }

        private string pin;
        private void InputPin(string value)
        {
            pin = value;
            CheckForm();
        }
        
        private void CheckForm()
        {
            if(ValidationUtils.StringPinValid(pin))
            {
                ActiveSaveButton(true);
            }
            else
            {
                ActiveSaveButton(false);
            }
        }

        protected override void ClickClose()
        {
            ActiveWindow(false);
        }

        protected async override void ClickSave()
        {
            var success = await ServiceLocator
                .GetService<AuthTypesOfRequests>().EnableTwoFactorAuth(pin);

            if (success)
            {
                _playerData.PlayerData.TwoFactorAuth = !_playerData.PlayerData.TwoFactorAuth;
                
                ActiveWindow(false);
                
                ServiceLocator.GetService<ModalWindowStatus>()
                    .SetAction(StatusModalWindows.Apply2FA);
            }
        }
    }
}