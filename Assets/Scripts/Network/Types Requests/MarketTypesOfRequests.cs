using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LazySoccer.Network.DataBaseURL;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class MarketTypesOfRequests: BaseTypesOfRequests<MarketRequests>
    {
        [SerializeField] private MarketDbURL dbURL;

        public override string FullURL(string URL, MarketRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        #region Market GET Requests
        
        public async UniTask<List<MarketInventory>> GET_MarketInventory(bool globalLoading = false)
        {
            var result = await GetRequest(_networkingManager.BaseURL, 
                MarketRequests.Inventory,
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            var a =  DataUtils.Deserialize<List<MarketInventory>>
                (result.downloadHandler.text);

            _managerMarket.SetMarketInventory(a);

            return a;
        }
        
        public async UniTask<List<MarketInGameShop>> GET_InGameShop()
        {
            Debug.Log("GET_InGameShop");
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                MarketRequests.InGameShop,
                Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var a =  DataUtils.Deserialize<List<MarketInGameShop>>
                (result.downloadHandler.text);

            _managerMarket.SetMarketInGameShop(a);

            return a;
        }
        
        public async UniTask<List<MarketPlayerTransfer>> GET_MyTransfers()
        {
            Debug.Log("GET_MyTransfers");
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                MarketRequests.Transfers,
                Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            Debug.Log("GET_MyTransfers: " + result.downloadHandler.text);
            
            var a =  DataUtils.Deserialize<List<MarketPlayerTransfer>>
                (result.downloadHandler.text);

            _managerMarket.SetMarketPlayerTransfers(a);

            return a;
        }
        
        public async UniTask<List<MarketOffer>> GET_Offers(string transferId)
        {
            Debug.Log("GET_Offers");

            var param = $"/{transferId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                MarketRequests.Offers,
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);
            
            return DataUtils.Deserialize<List<MarketOffer>>
                (result.downloadHandler.text); 
        }
        
        #endregion

        #region Market POST Requests
        
        public class RequestTransfersAll
        {
            public int? takeCount;
            public int? skipCount;
            public RequestPrice price;
            public RequestPower power;
            public int? positionId;
            public TransferOrder order;
            public List<int?> traits;
        }
        
        public enum TransferOrder
        {
            Latest,
            Price, 
            Deadline,
            Power,
            LatestDesc,
            DeadlineDesc,
            PriceDesc,
            PowerDesc
        }
        
        public class RequestPrice
        {
            public int? start;
            public int? end;
        }
        public class RequestPower
        {
            public int? start;
            public int? end;
        }

        public async UniTask<List<MarketPlayerTransfer>> POST_TransfersAll
            (RequestTransfersAll transfersAll, bool globalLoading = false)
        {
            var jsonData = DataUtils.Serialize(transfersAll);
            //var jsonData = JsonUtility.ToJson(transfersAll);
            
            Debug.Log("POST_TransfersAll: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.TransfersAll, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("POST_TransfersAll: " + result.downloadHandler.text);
            
            var a =  DataUtils.Deserialize<List<MarketPlayerTransfer>>
                (result.downloadHandler.text);
            
            _managerMarket.SetMarketGlobalTransfers(a);
            
            return a;
        }

        public class RequestPlayerForTransfer
        {
            public int playerId;
            public int price;
            public int deadLineDaysCount;
            public int? blitzPrice;
        }
        
        public async UniTask<bool> POST_PlayerForTransfer(RequestPlayerForTransfer playerForTransfer, bool globalLoading = false)
        {
            var jsonData = DataUtils.Serialize(playerForTransfer);
            
            Debug.Log("jsonData: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.PlayerForTransfer, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            await _teamTypesOfRequests.GET_TeamPlayers(false);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public class RequestPlayerForTransferCancel
        {
            public string playerTransferId;
        }
        
        public async UniTask<bool> POST_PlayerForTransferCancel(RequestPlayerForTransferCancel playerForTransfer, bool globalLoading = false)
        {
            var jsonData = JsonUtility.ToJson(playerForTransfer);
            Debug.Log("POST_PlayerForTransferCancel JSON: " + jsonData);
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.PlayerForTransferCancel, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
                
            await _teamTypesOfRequests.GET_TeamPlayers(false);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        
        public class RequestPlayerForTransferSendPriceOffer
        {
            public string playerTransferId;
            public int price;
        }
        
        public async UniTask<bool> POST_PlayerForTransferSendPriceOffer(RequestPlayerForTransferSendPriceOffer playerForTransfer, bool globalLoading = false)
        {
            var jsonData = JsonUtility.ToJson(playerForTransfer);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.PlayerForTransferSendPriceOffer, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public class InGameShopBuyBox
        {
            public int tierId;
        }
        
        public async UniTask<bool> POST_InGameShopBuyBox(InGameShopBuyBox box, bool globalLoading = false)
        {
            var jsonData = JsonUtility.ToJson(box);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.InGameShopBuyBox, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            await ServiceLocator.GetService<ManagerBalanceUpdate>().UpdateBalance(globalLoading);
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        
        public class InGameShopOpenBox
        {
            public int inventoryId;
        }
        
        public async UniTask<string> POST_InGameShopOpenBox(InGameShopOpenBox box, bool globalLoading = false)
        {
            var jsonData = JsonUtility.ToJson(box);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.InGameShopOpenBox, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);

            await ServiceLocator.GetService<ManagerBalanceUpdate>().UpdateBalance(globalLoading);
            
            return result.downloadHandler.text;
            //return _generalPopupMessage.ValidationCheck(result);
        }
        
        public class InGameShopUseLoot
        {
            public int inventoryId;
            public string chosenId;
        }
        
        public async UniTask<string> POST_InGameShopUseLoot(InGameShopUseLoot useLoot, bool globalLoading = false)
        {
            var jsonData = JsonUtility.ToJson(useLoot);
            
            Debug.Log("jsonData: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                MarketRequests.InGameShopUseLoot, 
                JSON: jsonData, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);

            await ServiceLocator.GetService<ManagerBalanceUpdate>().UpdateBalance(globalLoading);
            
            Debug.Log("result: " + result.downloadHandler.text);
            
            return result.downloadHandler.text;
        }
        
        #endregion
    }
}