using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Error;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class MedicalCenterTypesOfRequests : BaseTypesOfRequests<MedicalCenterRequests>
    {
        [SerializeField] private MedicalCenterDbURL dbURL;

        public override string FullURL(string URL, MedicalCenterRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        [Button]
        public async UniTask<bool> POST_MedicalCenter(MedicalCenterRequests type, int id)
        {
            Debug.Log("POST_MedicalCenter: " + type);
            
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