using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.Network
{
    public class TrainingCenterTypesOfRequests : BaseTypesOfRequests<TrainingCenterRequests>
    {
        [SerializeField] private TrainingCenterDbURL dbURL;

        public override string FullURL(string URL, TrainingCenterRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        public class RequestTrainingCenter
        {
            public int playerId;
        }

        public async UniTask<bool> POST_TrainingCenter(TrainingCenterRequests type, 
            int _playersId, bool globalLoading = false)
        {
            Debug.Log("POST_TrainingCenter: " + type);
            
            var obj = new RequestTrainingCenter()
            {
                playerId = _playersId
            };

            var jsonData = JsonUtility.ToJson(obj);
            Debug.Log("POST_TrainingCenter JSON: " + jsonData);
            var result = await PostRequest(_networkingManager.BaseURL, type, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_TrainingCenterInstant(TrainingCenterRequests type, 
            int _playerId, bool globalLoading = false)
        {
            Debug.Log("POST_TrainingCenterInstant: " + type);
            
            var obj = new RequestTrainingCenter()
            {
                playerId = _playerId
            };

            var jsonData = JsonUtility.ToJson(obj);
            Debug.Log("POST_TrainingCenterInstant JSON: " + jsonData);
            var result = await PostRequest(_networkingManager.BaseURL, 
                TrainingCenterRequests.InstantTrainingPlayers, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
    }
}