using Cysharp.Threading.Tasks;
using LazySoccer.Network.DataBaseURL;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.Network
{
    public class FitnessCenterTypesOfRequests: BaseTypesOfRequests<FitnessCenterRequests>
    {
        [SerializeField] private FitnessCenterDbURL dbURL;

        public override string FullURL(string URL, FitnessCenterRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        public async UniTask<bool> POST_FitnessCenter(FitnessCenterRequests type, int id)
        {
            Debug.Log("POST_FitnessCenter: " + type);
            
            var obj = new PlayerIdentificator()
            {
                playerId = id
            };

            var jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, type, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
    }
}