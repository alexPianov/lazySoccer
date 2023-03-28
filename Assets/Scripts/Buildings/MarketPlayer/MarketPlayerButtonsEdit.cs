using LazySoccer.Network;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerButtonsEdit : MonoBehaviour
    {
        [SerializeField] private Button buttonDetailsPrice;
        [SerializeField] private Button buttonRemovePrice;
        [SerializeField] private MarketTransferEdit _marketTransfer;
        [SerializeField] private MarketPlayer _marketPlayer;

        private MarketTypesOfRequests _marketTypesOfRequests;
        
        private void Start()
        {
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            
            if(buttonDetailsPrice) buttonDetailsPrice.onClick.AddListener(DetailsPrice);
            buttonRemovePrice.onClick.AddListener(RemoveTransfer);
            
        }

        public void ActiveButtons(bool state)
        {
            if(buttonDetailsPrice) buttonDetailsPrice.gameObject.SetActive(state);
            buttonRemovePrice.gameObject.SetActive(state);
        }

        private void DetailsPrice()
        {
            ServiceLocator.GetService<MarketPopupStatus>()
                .SetAction(StatusMarketPopup.EditPlayerTransfer);
            
            _marketTransfer.UpdateInfo(_marketPlayer.Transfer);
        }

        private async void RemoveTransfer()
        {
            buttonRemovePrice.interactable = false;
            
            MarketTypesOfRequests.RequestPlayerForTransferCancel request = new();

            request.playerTransferId = _marketPlayer.Transfer.playerTransferId;
            
            var success = await _marketTypesOfRequests
                .POST_PlayerForTransferCancel(request);

            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Transfer was deleted");
                ServiceLocator.GetService<MarketPopupStatus>().SetAction(StatusMarketPopup.None);
                ServiceLocator.GetService<BuildingWindowStatus>().SetAction(StatusBuilding.MarketMain);
                ServiceLocator.GetService<MarketStatus>().StatusAction = StatusMarket.MyTransfers;
            }
            
            buttonRemovePrice.interactable = true;
        }
    }
}