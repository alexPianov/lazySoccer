using System;
using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransfers : CenterSlotList
    {
        [SerializeField] private GameObject traitPrefab;
        
        [Header("Refs")]
        [SerializeField] private MarketTransfersFilter _marketTransfersFilter;
        [SerializeField] private MarketPlayer _marketPlayer;
        [SerializeField] private Button buttonAdd;
            
        [Header("Setup")]
        [SerializeField] private bool showPlayerInfo;
        [SerializeField] private bool showMyTransfers;
        
        public async void UpdateList()
        {
            if (buttonAdd)
            {
                buttonAdd.interactable = CheckCommCenterLevel();
            }
            
            var _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            var _marketManager = ServiceLocator.GetService<ManagerMarket>();
            var transfers = _marketManager.MarketGlobalTransfers;
            
            if (showMyTransfers)
            {
                transfers = _marketManager.MarketPlayerTransfers;
            }
            
            CreateSlots(transfers);

            if (showMyTransfers)
            {
                transfers = await _marketTypesOfRequests.GET_MyTransfers();
                Debug.Log("Show my transfer: " + transfers.Count);
            }
            else
            {
                transfers = await _marketTransfersFilter.GetTransfers();
                Debug.Log("Show global transfer: " + transfers.Count);
            }
            
            CreateSlots(transfers);
        }

        private void CreateSlots(List<MarketPlayerTransfer> transfers)
        {
            DestroyAllSlots();

            if(transfers == null || transfers.Count == 0) return;
            
            for (int i = 0; i < transfers.Count; i++)
            {
                var transfer = transfers[i];

                if (transfer.status == TransferStatus.Canceled) continue; 
                if (transfer.status == TransferStatus.Finished) continue; 
                
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out MarketTransferSlot transferSlot))
                {
                    transferSlot.SetPlayerInfo(transfer);
                    transferSlot.SetTraitIcons(transfer.player.traits, traitPrefab);
                    transferSlot.SetTransfer(transfer, i + 1);

                    if (transfer.buyer != null)
                    {
                        transferSlot.SetEmblemSprite
                            (GetEmblemSprite(transfer.buyer.teamEmblem.emblemId));
                    }

                    if (showPlayerInfo)
                    {
                        slotInstance.AddComponent<MarketTransferSlotListener>()
                            .SetMaster(this);
                    }
                }
            }
        }

        public void ShowPlayer(MarketTransferSlot transferSlot)
        {
            _marketPlayer.ShowPlayer(transferSlot.Transfer);
        }

        private const int requiredLevel = 10;
        private bool CheckCommCenterLevel()
        {
            var commCenterLevel = ServiceLocator.GetService<ManagerBuilding>().BuildingAll
                .Find(all => all.buildingType == TypeHouse.Communications_Center).level;  
            return commCenterLevel >= requiredLevel;
        }
    }
}