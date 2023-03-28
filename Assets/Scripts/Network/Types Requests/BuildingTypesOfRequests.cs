using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using LazySoccer.Utils;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class BuildingTypesOfRequests : BaseTypesOfRequests<BuildingRequests>
    {
        [SerializeField] private BuildingDbURL dbURL;
        public override string FullURL(string URL, BuildingRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        public override async UniTask SendMessage(BuildingRequests type) { }
        
        [Button]
        public async UniTask<bool> Get_AllBuildingRequest(bool globalLoading = true)
        {
            var result = await GetRequest
                (_networkingManager.BaseURL, BuildingRequests.AllBuilding, Token: _managerPlayer.PlayerData.Token,
                    ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get_AllBuildingRequest: " + result.downloadHandler.text);
            
            var a = DataUtils.Deserialize<List<BuildingAll>>(result.downloadHandler.text);
            ServiceLocator.GetService<ManagerBuilding>().RequestServer(a);

            return a != null && a.Count > 0;
        }
        
        public async UniTask<List<BuildingAll>> Get_AllBuildingsRequest(int teamId, bool globalLoading = true)
        {
            var param = $"?=teamId{teamId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                BuildingRequests.AllBuilding, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get_AllBuildingRequest: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<BuildingAll>>(result.downloadHandler.text); 
        }
        
        private string HouseInfoJSON(int buildingId = 0)
        {
            UpgradeBuilding upgrade = new UpgradeBuilding()
            {
                teamBuildingId = buildingId
            };
            return  JsonUtility.ToJson(upgrade);
        }

        [Button]
        public async UniTask<bool> Post_BuildingUpgrade(int buildingId = 0, bool globalLoading = false)
        {      
            var result = await PostRequest(_networkingManager.BaseURL, 
                BuildingRequests.UpgradeBulding, JSON: HouseInfoJSON(buildingId), Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);

            return _generalPopupMessage.ValidationCheck(result);
        }
        
        [Button]
        public async UniTask<bool> Post_CancelUpgrade(int buildingId = 0, bool globalLoading = false)
        {      
            var result = await PostRequest(_networkingManager.BaseURL, 
                BuildingRequests.Downgrade, JSON: HouseInfoJSON(buildingId), Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);

            return _generalPopupMessage.ValidationCheck(result);
        }
        public async UniTask<bool> Post_Downgrade(int buildingId, bool globalLoading = false)
        {
            var result = await PostRequest(_networkingManager.BaseURL, 
                BuildingRequests.Downgrade, JSON: HouseInfoJSON(buildingId), Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }

        public async UniTask<bool> Post_ImmediateUpgrade(int buildingId, bool globalLoading = false)
        {
            var result = await PostRequest(_networkingManager.BaseURL, 
                BuildingRequests.ImmediateUpgrade, 
                JSON: HouseInfoJSON(buildingId), 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
    }
}
