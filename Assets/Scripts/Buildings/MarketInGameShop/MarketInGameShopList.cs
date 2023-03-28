using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.Network.MarketTypesOfRequests;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketInGameShopList : CenterSlotList
    {
        [SerializeField] private MarketItemDisplay _marketItemDisplay;
        
        private MarketTypesOfRequests _marketTypesOfRequests;
        private ManagerMarket _managerMarket;
        
        private void Awake()
        {
            base.Awake();
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            _managerMarket = ServiceLocator.GetService<ManagerMarket>();
        }

        public async void UpdateList()
        {
            var shopItems = _managerMarket.MarketInGameShop;

            CreateSlots(shopItems);
            
            shopItems = await _marketTypesOfRequests.GET_InGameShop();
            
            CreateSlots(shopItems);
        }
        
        private void CreateSlots(List<MarketInGameShop> inGameShopItems)
        {
            DestroyAllSlots();
            
            if(inGameShopItems == null || inGameShopItems.Count == 0) return;

            for (int i = 0; i < inGameShopItems.Count; i++)
            {
                var item = inGameShopItems[i];
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out MarketInGameShopSlot shopSlot))
                {
                    shopSlot.SetData(item);
                    shopSlot.SetMaster(this);
                }
            }
        }

        public async UniTask<bool> Purchase(MarketInGameShop shopItem)
        {
            InGameShopBuyBox box = new();

            box.tierId = shopItem.tierId;

            return await _marketTypesOfRequests.POST_InGameShopBuyBox(box);
        }

        public void ShowInfoPopup(MarketInGameShop shopItem)
        {
            _marketItemDisplay.SetShopItem(shopItem);
        }
    }
}