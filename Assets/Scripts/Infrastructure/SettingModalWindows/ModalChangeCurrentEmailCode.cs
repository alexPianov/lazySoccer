using LazySoccer.Network;
using LazySoccer.Status;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalChangeCurrentEmailCode : BaseModalWindows
    {
        public Button buttonResend;
        
        [Title("TMP")]
        public TMP_InputField inputEmailCode;
        public TMP_Text textEmail;
        
        [Title("Validation")]
        [SerializeField] private FindValidationContainer _valid;

        private void Start()
        {
            _valid = GetComponent<FindValidationContainer>();

            base.Start();

            inputEmailCode.onValueChanged.AddListener(EditCodeField);
            buttonResend.onClick.AddListener(ResendCode);
        }

        public void ClearFields()
        {
            code = "";
            
            base.ClearFields();
            
            CheckForm();
        }

        private string code;
        private void EditCodeField(string text)
        {
            code = text;
            CheckForm();
        }

        private string newEmail;
        public void EditNewEmailField(string text)
        {
            newEmail = text;
            textEmail.text = $"We have sent a verification code to {newEmail}";
        }
        
        private string password;
        public void EditPasswordField(string text)
        {
            password = text;
        }
        
        public void CheckForm()
        {
            if (ValidationUtils.StringCodeValid(code))
            {
                ActiveSaveButton(true);
            }
            else
            {
                ActiveSaveButton(false);
            }
        }

        protected async override void ClickSave()
        {
            if (!_valid.isRatification()) return;

            var result = await ServiceLocator.GetService<ChangeUserTypesOfRequests>()
                .ChangeEmail(newEmail, password, code);

            if (result)
            {
                ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Email = newEmail;
                
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("The email is changed to EMAIL", Param1: newEmail);
                
                ServiceLocator.GetService<ModalWindowStatus>().SetAction(StatusModalWindows.None);
            }
            else
            {
                inputEmailCode.GetComponent<ValidationField>().ActiveErrorHighlight();
            }
        }

        private async void ResendCode()
        {
            var result = await ServiceLocator.GetService<ChangeUserTypesOfRequests>()
                .SendCodeToNewMail(newEmail);
            
            if (result)
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("The code is resend to EMAIL", Param1: newEmail);
                
                ClearFields();
            }
        }

    }
}