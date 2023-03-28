using Cysharp.Threading.Tasks;
using LazySoccer.Status;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace LazySoccer.Network
{
    public class ChangeUserTypesOfRequests : BaseTypesOfRequests<ChangeOfRequests>
    {
        [SerializeField] private ChangeUserDbURL dbURL;
        public override string FullURL(string URL, ChangeOfRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        protected void RequestOK(UnityWebRequest result, Action<UnityWebRequest> action)
        {
            switch (result.responseCode)
            {
                case 200:
                    action(result);
                    break;
                default:
                    Debug.LogError($"Error code: {result.responseCode}");
                    break;
            }
        }

        public async UniTask<bool> ChangePassword(string currentPassword, string newPassword)
        {
            var obj = new ChangePassword()
            {
                password = currentPassword,
                newPassword = newPassword
            };

            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, ChangeOfRequests.ChangePassword, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> ChangeName(string newNickname)
        {
            var obj = new ChangeName()
            {
                userName = newNickname,
            };

            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                ChangeOfRequests.ChangeNickName, 
                    JSON: jsonData, Token: _managerPlayer.PlayerData.Token);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> SendCodeToNewMail(string newEmail)
        {
            var param = $"?mail={newEmail}";
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                ChangeOfRequests.SendCodeToNewMail, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> ChangeEmail(string newEmail, string password, string code)
        {
            var obj = new ChangeEmail()
            {
                email = newEmail,
                password = password,
                code = code
            };

            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, ChangeOfRequests.ChangeMail, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
    }
}
