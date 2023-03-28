using Cysharp.Threading.Tasks;
using LazySoccer.Status;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static LazySoccer.Network.Error.ErrorRequest;
using static LazySoccer.Network.Post.AuthClassPostAnswer;
using static LazySoccer.Network.Post.AuthClassPostRequest;

namespace LazySoccer.Network
{
    public class AuthTypesOfRequests : BaseTypesOfRequests<AuthOfRequests>
    {
        [SerializeField] private AuthDbURL dbURL;
        public override string FullURL(string URL, AuthOfRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        public override async UniTask SendMessage(AuthOfRequests type)
        {
            switch (type)
            {
                case AuthOfRequests.Login:
                    await Login(type);
                    break;
                case AuthOfRequests.MailConfirm:
                    await MailConfirm(type);
                    break;
                case AuthOfRequests.SendMailCode:
                    await SendMailCode(type);
                    break;
                case AuthOfRequests.RestorePassword:
                    await RestorePassword(type);
                    break;
                case AuthOfRequests.TwoFactorAuth:
                    await TwoFactorAuth(type);
                    break;
                case AuthOfRequests.SignUp:
                    await SignUp(type);
                    break;
                default:break;
            }
        }
        protected async UniTask<bool> RequestOK(UnityWebRequest result, Action<UnityWebRequest> action)
        {
            switch (result.responseCode)
            {
                case 200:
                    action(result);
                    return true;
                    break;
                case 204:
                    action(result);
                    return true;
                    break;
                case 400:
                    _generalPopupMessage.ValidationCheck(result);
                    return false;
                    break;
                case 403:
                    _generalPopupMessage.ValidationCheck(result);
                    return false;
                    break;
                case 404:
                    return false;
                    break;
                default:
                    Debug.LogError($"Error code: {result.responseCode}");
                    ServiceLocator.GetService<AuthenticationStatus>()
                        .SetAction(StatusAuthentication.Back);
                    
                    ServiceLocator.GetService<LoadingStatus>().SetAction(StatusLoading.None);
                    return false;
                    break;
            }
        }

        #region Login
        private async UniTask Login(AuthOfRequests type)
        {
            _managerLoading.ControlLoading(true);
            
            var obj = new Login()
            {
                login = _managerPlayer.PlayerData.Email,
                password = _managerPlayer.PlayerData.Password
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                type, JSON: jsonData, ActiveGlobalLoading: false);

            await RequestOK(result, LoginResult);
        }
        private void LoginResult(UnityWebRequest result)
        {
            var answer = JsonUtility.FromJson<LoginAnswer>(result.downloadHandler.text);
            Debug.Log("LoginResult | token: " + answer.token);
            _managerPlayer.PlayerData.Token = answer.token;
            _managerPlayer.PlayerData.RefreshToken = answer.refreshToken;
            _managerPlayer.PlayerData.TwoFactorAuth = answer.twoFactorEnabled;
            
            if (answer.twoFactorEnabled)
            {
                _authentication.StatusAction = StatusAuthentication.TwoFA;
            }
            else
            {
                _managerPayloadBootstrap.Boot(false);
            }
        }

        #endregion

        #region MailConfirm
        private async UniTask MailConfirm(AuthOfRequests type)
        {
            var obj = new MailConfirm()
            {
                email = _managerPlayer.PlayerData.Email,
                code = _managerPlayer.PlayerData.Code
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest
                (_networkingManager.BaseURL, type, JSON: jsonData);
            
            await RequestOK(result, MailConfirmResult);
            
            //var success = _generalPopupMessage.ValidationCheck(result, true);

            // if (success)
            // {
            //     await SendMessage(AuthOfRequests.Login);
            // }
        }

        private async void MailConfirmResult(UnityWebRequest result)
        {
            await SendMessage(AuthOfRequests.Login);
        }
        
        #endregion

        #region SendMailCode
        public async UniTask<bool> SendMailCode(AuthOfRequests type = AuthOfRequests.SendMailCode)
        {
            var result = await PostRequest(_networkingManager.BaseURL, type, URLParam: _managerPlayer.PlayerData.Email);
            return await RequestOK(result, SendMailCodeResult);
        }
        
        private void SendMailCodeResult(UnityWebRequest result)
        {
            //_authentication.StatusAction = StatusAuthentication.RestorePasswordCode;
        }
        
        #endregion

        #region RestorePassword
        public async UniTask<bool> RestorePassword(AuthOfRequests type = AuthOfRequests.RestorePassword)
        {
            var obj = new RestorePassword()
            {
                email = _managerPlayer.PlayerData.Email,
                password = _managerPlayer.PlayerData.Password,
                code = _managerPlayer.PlayerData.Code
            };
            string jsonData = JsonUtility.ToJson(obj);
            var result = await PostRequest(_networkingManager.BaseURL, type, JSON: jsonData);
            return await RequestOK(result, RestorePasswordResult);
        }
        
        private void RestorePasswordResult(UnityWebRequest result)
        {
            Login(AuthOfRequests.Login).Forget();
        }
        #endregion

        #region GenerateTwoFactorAuth
        
        public async UniTask<bool> GenerateTwoFactorAuth()
        {
            var token = _managerPlayer.PlayerData.Token;

            var codeSize = "?qrCodeSize=" + 3;
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                AuthOfRequests.GenerateTwoFactorAuth, 
                Token: token, URLParam: codeSize, ActiveGlobalLoading: true);
            
            GenerateTwoFactorAuthResult(result);

            return true;

            // var success = _generalPopupMessage.ValidationCheck(result);
            //
            // if (success) 
            //
            // return success;
        }
        
        private void GenerateTwoFactorAuthResult(UnityWebRequest result)
        {
            var answer = JsonUtility.FromJson<TwoFactorAuthentication>(result.downloadHandler.text);
            
            Debug.Log("EnableTwoFactorAuthResult: " + result.downloadHandler.text);
            
            if(answer == null) {Debug.Log("Failed to find server answer");return;}
            if(_managerPlayer == null) {Debug.Log("Failed to find manager player");return;}
            if(_managerPlayer.PlayerData == null) {Debug.Log("Failed to find player data");return;}
            
            _managerPlayer.PlayerData.AuthenticationBarCodeImage = answer.authenticationBarCodeImage;
            _managerPlayer.PlayerData.AuthenticationManualCode = answer.authenticationManualCode;
        }
        
        #endregion
        
        #region EnableTwoFactorAuth
        
        public async UniTask<bool> EnableTwoFactorAuth(string pincode)
        {
            var token = _managerPlayer.PlayerData.Token;

            var code = "?code=" + pincode;
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                AuthOfRequests.EnableTwoFactorAuth, 
                Token: token, URLParam: code, ActiveGlobalLoading: true);

            return _generalPopupMessage.ValidationCheck(result);
        }
        
        #endregion

        #region TwoFactorAuth

        private class Pin
        {
            public string pin;
        }
        
        private async UniTask TwoFactorAuth(AuthOfRequests type)
        {
            var token = _managerPlayer.PlayerData.Token;
            
            var obj = new Pin()
            {
                pin = _managerPlayer.PlayerData.Pin
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            Debug.Log("Pin Json: " + jsonData);
            
            var result = await PostRequest
                (_networkingManager.BaseURL, type, JSON: jsonData, Token: token);
            
            var success = _generalPopupMessage.ValidationCheck(result);
            
            if(success) await RequestOK(result, TwoFactorAuthResult);
        }
        
        private void TwoFactorAuthResult(UnityWebRequest result)
        {
            var answer = JsonUtility.FromJson<LoginAnswer>(result.downloadHandler.text);
            
            _managerPlayer.PlayerData.Token = answer.token;
            _managerPlayer.PlayerData.RefreshToken = answer.refreshToken;
            
            _managerPayloadBootstrap.Boot(true);
        }
        
        #endregion
        
        #region RefreshToken
        
        private async UniTask RefreshToken()
        {
            var token = _managerPlayer.PlayerData.Token;
            
            var result = await PostRequest
                (_networkingManager.BaseURL, AuthOfRequests.RefreshToken, Token: token);

            await RequestOK(result, RefreshTokenResult);
        }
        private void RefreshTokenResult(UnityWebRequest result)
        {
            var answer = JsonUtility.FromJson<LoginAnswer>(result.downloadHandler.text);
            
            _managerPlayer.PlayerData.Token = answer.token;
            _managerPlayer.PlayerData.RefreshToken = answer.refreshToken;
            
        }
        #endregion

        #region SignUp
        public async UniTask<bool> SignUp(AuthOfRequests type = AuthOfRequests.SignUp)
        {
            Debug.Log("SignUp | UserName: " + _managerPlayer.PlayerData.UserName + " Email: " + _managerPlayer.PlayerData.Email + 
                      " Pass: " + _managerPlayer.PlayerData.Password);
            
            var obj = new Register()
            {
                userName = _managerPlayer.PlayerData.UserName,
                email = _managerPlayer.PlayerData.Email,
                password = _managerPlayer.PlayerData.Password
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            var result = await PostRequest
                (_networkingManager.BaseURL, type, JSON: jsonData);

            return await RequestOK(result, SignUpResult);
        }
        
        private void SignUpResult(UnityWebRequest result)
        {
            _authentication.StatusAction = StatusAuthentication.EnterCode;
        }
        
        #endregion
    }
}
