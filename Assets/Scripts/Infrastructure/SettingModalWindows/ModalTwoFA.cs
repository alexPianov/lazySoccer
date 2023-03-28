using System;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.Status;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalTwoFA : BaseModalWindows
    {
        [Title("Panel")]
        [SerializeField] private ModalTwoFaPin twoFaPin;
        [SerializeField] private Button buttonPopup;
        
        [Title("Key")]
        [SerializeField] private TMP_Text textEnableInfo;
        [SerializeField] private TMP_Text textEnableStatus;
        [SerializeField] private TMP_Text textKey;
        [SerializeField] private RawImage imageQr;
        //[SerializeField] private int textureSize = 10;
        
        private ManagerPlayerData _playerData;
        private QuestionPopup _questionPopup;

        private void Start()
        {
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();
            _playerData = ServiceLocator.GetService<ManagerPlayerData>();

            base.Start();
            
            buttonPopup.onClick.AddListener(OpenPopupPin);
        }

        #region Generate Key and QR Code

        public async void UpdateQrCode()
        {
            var result = await ServiceLocator
                .GetService<AuthTypesOfRequests>().GenerateTwoFactorAuth();

            if (result)
            {
                textKey.text = _playerData.PlayerData.AuthenticationManualCode;
                var base64 = SplitImageCode(_playerData.PlayerData.AuthenticationBarCodeImage);
                //imageQr.texture = Base64ToTexture2D(base64);
                imageQr.texture = DataUtils.Base64ToTexture2D(base64);
            }
        }
        
        private string SplitImageCode(string serverCodeImage)
        {
            return serverCodeImage.Split(',')[1];
        }

        private Texture Base64ToTexture2D(string b64_string)
        {
            // Debug.Log(b64_string);
            // var b64_bytes = Convert.FromBase64String(b64_string);
            //
            // var tex = new Texture2D(textureSize, textureSize);
            // tex.LoadImage(b64_bytes);
            //
            // return tex;

            return null;
        }

        #endregion

        public void UpdateActiveButton()
        {
            if (_playerData.PlayerData.TwoFactorAuth)
            {
                textEnableInfo.GetComponent<Localize>().SetTerm("3-2FA-Text-Enabled");
                textEnableStatus.GetComponent<Localize>().SetTerm("3-2FA-Button-Disable");
                
                textEnableStatus.color = DataUtils.GetColorFromHex("D90000");
            }
            else
            {
                textEnableInfo.GetComponent<Localize>().SetTerm("3-2FA-Text-Disabled");
                textEnableStatus.GetComponent<Localize>().SetTerm("3-2FA-Button-Enable");
                
                textEnableStatus.color = DataUtils.GetColorFromHex("52B400");
            }
        }
        
        protected override void ClickSave()
        {
            GUIUtility.systemCopyBuffer = textKey.text;
            
            _questionPopup.OpenQuestion
            ("Paste this key in Google Authorization app", 
                    "Key is copied", "I understand");
        }

        private async void OpenPopupPin()
        {
            if (_playerData.PlayerData.TwoFactorAuth)
            {
                var result = await _questionPopup.OpenQuestion
                ("Your account protection will be reduced",
                    "Are you sure?", "Disable 2FA");
            
                if(!result) return;
            }

            twoFaPin.ActiveWindow(true);
        }
    }
}