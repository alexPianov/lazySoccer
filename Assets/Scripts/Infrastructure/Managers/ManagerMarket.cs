using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class ManagerMarket : MonoBehaviour
    {
        public List<MarketInventory> MarketInventories { get; private set; }
        public List<MarketPlayerTransfer> MarketGlobalTransfers { get; private set; }
        public List<MarketPlayerTransfer> MarketPlayerTransfers { get; private set; }
        public List<MarketInGameShop> MarketInGameShop { get; private set; }
            
        public void SetMarketInventory(List<MarketInventory> inventoryList)
        {
            MarketInventories = inventoryList;
        }

        public void SetMarketGlobalTransfers(List<MarketPlayerTransfer> globalTransfers)
        {
            MarketGlobalTransfers = globalTransfers;
        }
        
        public void SetMarketPlayerTransfers(List<MarketPlayerTransfer> playerTransfers)
        {
            MarketPlayerTransfers = playerTransfers;
        }

        public void SetMarketInGameShop(List<MarketInGameShop> inGameShop)
        {
            MarketInGameShop = inGameShop;
        }
    }
}