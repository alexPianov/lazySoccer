using System.Collections.Generic;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayerHistory
{
    public class OfficePlayerHistoryTransferList : MonoBehaviour
    {
        [SerializeField] private GameObject transferPrefab;
        [SerializeField] private Transform transferPrefabContainer;
        
        public void CreateTransfer(List<TeamPlayerTransfer> playerTransfers)
        {
            Debug.Log("CreateTransfers: " + playerTransfers.Count);
            
            ClearContainer();
            
            if (playerTransfers == null || playerTransfers.Count == 0)
            {
                return;
            }
            
            foreach (var transfer in playerTransfers)
            {
                Debug.Log("Transfer id: " + transfer.playerTransferId);
                
                if(transfer.buyer == null) continue;
                
                var slot = Instantiate(transferPrefab, transferPrefabContainer);
            
                if (slot.TryGetComponent(out SlotPlayerTransfer component))
                {
                    component.SetInfo(transfer);
                }
            }
        }

        private void ClearContainer()
        {
            for (int i = 0; i < transferPrefabContainer.childCount; i++)
            {
                if (transferPrefabContainer.GetChild(i))
                {
                    Destroy(transferPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}