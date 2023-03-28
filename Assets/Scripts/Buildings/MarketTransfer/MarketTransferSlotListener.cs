using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransferSlotListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddAsPickedPlayer);
        }

        private MarketTransfers _marketTransfers;
        public void SetMaster(MarketTransfers transfers)
        {
            _marketTransfers = transfers;
        }

        private void AddAsPickedPlayer()
        {
            _marketTransfers.ShowPlayer(GetComponent<MarketTransferSlot>());
        }
    }
}