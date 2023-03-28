using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketInventoryList : CenterSlotList
    {
        [SerializeField] private MarketItemDisplay _marketItemDisplay;
        [SerializeField] private MarketItemPlayerPicker _marketItemPlayerPicker;
        
        private MarketTypesOfRequests _marketTypesOfRequests;
        private ManagerMarket _managerMarket;
        
        private void Start()
        {
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            _managerMarket = ServiceLocator.GetService<ManagerMarket>();
        }

        public async void UpdateList()
        {
            var inventories = _managerMarket.MarketInventories;

            CreateSlots(inventories);
            
            inventories = await _marketTypesOfRequests.GET_MarketInventory();
            
            CreateSlots(inventories);
        }

        private void CreateSlots(List<MarketInventory> inventories)
        {
            DestroyAllSlots();
            
            if(inventories == null || inventories.Count == 0) return;

            for (int i = 0; i < inventories.Count; i++)
            {
                var item = inventories[i];

                if (item.status == InventoryStatus.Active)
                {
                    continue;
                }
                
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out MarketInventorySlot inventorySlot))
                {
                    inventorySlot.SetData(item);
                    inventorySlot.SetMaster(this);
                }
            }
        }
        
        public async UniTask<bool> Open(MarketInventory inventoryItem)
        {
            MarketTypesOfRequests.InGameShopOpenBox request = new();
            request.inventoryId = inventoryItem.inventoryId;
            await _marketTypesOfRequests.POST_InGameShopOpenBox(request);

            ServiceLocator.GetService<MarketStatus>().StatusAction
                = StatusMarket.Inventory;

            return true;
        }

        public async UniTask<bool> Use(MarketInventory inventoryItem)
        {
            MarketTypesOfRequests.InGameShopUseLoot request = new();
            
            request.inventoryId = inventoryItem.inventoryId;
            request.chosenId = await _marketItemPlayerPicker.GetChosenId(inventoryItem, StatusMarketPopup.None);

            if (request.chosenId == null)
            {
                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Inventory;
                
                return false;
            }
            
            await _marketTypesOfRequests.POST_InGameShopUseLoot(request);
            
            ServiceLocator.GetService<MarketStatus>().StatusAction
                = StatusMarket.Inventory;
            
            return true;
        }
        
        public void ShowInfoPopup(MarketInventory inventoryItem)
        {
            _marketItemDisplay.SetInventoryItem(inventoryItem);
        }
    }
}