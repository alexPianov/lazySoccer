using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using LazySoccer.Utils;
using UnityEngine;
using UnityEngine.Networking;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.Network.Post.TeamClassPostRequest;

namespace LazySoccer.Network
{
    public class CreateTeamTypesOfRequest : BaseTypesOfRequests<CreateTeamRequests>
    {
        [SerializeField] private CreateTeamDbURL dbURL;
        public override string FullURL(string URL, CreateTeamRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        public override async UniTask SendMessage(CreateTeamRequests type)
        {
            switch (type)
            {
                case CreateTeamRequests.CreateTeam:
                    await CreateTeam();
                    break;
                case CreateTeamRequests.GeneratePlayers:
                    await GeneratePlayersReceiptList();
                    break;
                default: break;
            }
        }
        
        protected bool RequestOK(UnityWebRequest result)
        {
            switch (result.responseCode)
            {
                case 200:
                    return true;
                case 400:
                case 403:
                case 404:
                case 500:
                default:
                    Debug.LogError($"Error code: {result.responseCode}");
                    return false;
            }
        }
        
        public async UniTask<bool> CreateTeam()
        {
            var token = _managerPlayer.PlayerData.Token;
            
            var obj = new CreateTeamPost()
            {
                name = _managerPlayer.PlayerHUDs.NameTeam.Value,
                shortName = _managerPlayer.PlayerHUDs.NameShortTeam.Value
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CreateTeamRequests.CreateTeam, 
                JSON: jsonData, 
                Token: token, 
                ActiveGlobalLoading: false);
            
            Debug.Log("Create Team: " + result.downloadHandler.text);

            var success = RequestOK(result);
            
            if (!success)
            {
                return _generalPopupMessage.ValidationCheck(result);
            }

            return success;
            //return _generalPopupMessage.ValidationCheck(result);
        }
        
        public class ReceiptList
        {
            public string receiptList;
        }

        public async UniTask GeneratePlayersReceiptList()
        {
            Debug.Log("GeneratePlayersReceiptList");

            await GeneratePlayersOption(TeamPlayerStatus.GenerationOptionOne);
            await GeneratePlayersOption(TeamPlayerStatus.GenerationOptionTwo);
            await GeneratePlayersOption(TeamPlayerStatus.GenerationOptionThree);
        }

        public async UniTask GeneratePlayersOption(TeamPlayerStatus option)
        {
            var token = _managerPlayer.PlayerData.Token;
            var jsonData = GetJsonPlayersOption((int)option);
            
            await PostRequest(_networkingManager.BaseURL, 
                CreateTeamRequests.GeneratePlayers, 
                Token: token, 
                JSON: jsonData,
                ActiveGlobalLoading: false);
        }
        
        private string GetJsonPlayersOption(int number)
        {
            var obj = new ReceiptList()
            {
                receiptList = number.ToString()
            };
            
            return JsonUtility.ToJson(obj);
        }
        
        public async UniTask<bool> SavePlayersGenerationOption(TeamPlayerStatus option, bool globalLoading = true)
        {
            var token = _managerPlayer.PlayerData.Token;
            var jsonData = GetJsonPlayersOption((int)option);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CreateTeamRequests.SavePlayersGeneration, 
                Token: token, 
                JSON: jsonData,
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
    }
}
