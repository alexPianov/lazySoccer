using Cysharp.Threading.Tasks;
using LazySoccer.Network.Error;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class CommandTypesOfRequests : BaseTypesOfRequests<CommandRequests>
    {
        [SerializeField] private CommandDbURL dbURL;
        public override string FullURL(string URL, CommandRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        public override async UniTask SendMessage(CommandRequests type)
        {
            switch (type)
            {
                case CommandRequests.AddTransactionIdTest:
                    await POST_TransactionId(type);
                    break;
                case CommandRequests.TopUpAccountTest:
                    await POST_TopUpAccount();
                    break;
                case CommandRequests.AddNFTTests:
                    await POST_Command(type);
                    break;
                case CommandRequests.AddPlayersTraumasTest:
                    await POST_Command(type);
                    break;
                case CommandRequests.AddPlayersTraitsTest:
                    await POST_Command(type);
                    break;
                case CommandRequests.AddPlayersTrophiesRewardsTest:
                    await POST_Command(type);
                    break;
                case CommandRequests.AddPlayersLoseFormTest:
                    await POST_Command(type);
                    break;
                case CommandRequests.FameTest:
                    await GET_FameTest(type);
                    break;
                case CommandRequests.DeleteUserTest:
                    await POST_DeleteUserTest(type);
                    break;
                case CommandRequests.AddTeamTrophiesRewardsTest:
                    await POST_Command(type);
                    break;
                case CommandRequests.GenerateGamesTest:
                    await POST_Command(type);
                    break;
                
                default: break;
            }
        }
        
        [Button]
        public async UniTask POST_Command(CommandRequests type, bool globalLoading = true)
        {
            Debug.Log("--- Command: " + type);
            
            var result = await PostRequest
                (_networkingManager.BaseURL, type, Token: _managerPlayer.PlayerData.Token, 
                    ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Command result: " + result.downloadHandler.text);
        }
        
        [Button]
        public async UniTask POST_TopUpAccount(bool globalLoading = true)
        {
            Debug.Log("--- TopUpAccount");
            
            var amount = "?amount=5000000";

            await PostRequest
                (_networkingManager.BaseURL, CommandRequests.TopUpAccountTest,
                    Token: _managerPlayer.PlayerData.Token, 
                    URLParam: amount,
                    ActiveGlobalLoading: globalLoading);
        }
        
        private async UniTask POST_TransactionId(CommandRequests type)
        {
            Debug.Log("--- TransactionId: " + type);
            
            var obj = new TransactionId()
            {
                signature = ""
            };

            var jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest
            (_networkingManager.BaseURL, type, JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token);
            
            Debug.Log("TransactionId result: " + result.downloadHandler.text);
        }
        
        private async UniTask GET_FameTest(CommandRequests type)
        {
            Debug.Log("--- TransactionId: " + type);
            
            var result = await GetRequest
            (_networkingManager.BaseURL, type, Token: _managerPlayer.PlayerData.Token);
            
            Debug.Log("TransactionId result: " + result.downloadHandler.text);
        }
        
        private async UniTask POST_DeleteUserTest(CommandRequests type)
        {
            var param = $"?refreshToken={_managerPlayer.PlayerData.RefreshToken}";
            
            await PostRequest(_networkingManager.BaseURL, type,
                Token: _managerPlayer.PlayerData.Token, URLParam: param);
        }
        
        private async UniTask GET_Job(CommandRequests type)
        {
            var param = $"?refreshToken={_managerPlayer.PlayerData.RefreshToken}";
            
            await PostRequest(_networkingManager.BaseURL, type,
                Token: _managerPlayer.PlayerData.Token, URLParam: param);
        }
        
        [Button]
        public async UniTask GET_GameGenerator(Tournament tournament = Tournament.Amateur_conference, 
            bool globalLoading = true)
        {
            var param = $"/{(int)tournament}?isRestoreFormPlayers={true}";
            
            await PostRequest(_networkingManager.BaseURL, CommandRequests.GenerateGamesTest,
                Token: _managerPlayer.PlayerData.Token, URLParam: param, ActiveGlobalLoading: globalLoading);
        }
    }
}