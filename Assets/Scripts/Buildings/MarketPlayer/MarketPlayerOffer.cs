using LazySoccer.Network;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerOffer : MonoBehaviour
    {
        [Title("Input")] 
        [SerializeField] private TMP_InputField inputOfferPrice;
        [SerializeField] private Button buttonSend;
        [SerializeField] private MarketPlayer marketPlayer;

        private ManagerBalanceUpdate _managerBalanceUpdate;
        private ManagerPlayerData _managerPlayerData;
        private MarketTypesOfRequests _marketTypesOfRequests;

        protected void Start()
        {
            _managerBalanceUpdate = ServiceLocator.GetService<ManagerBalanceUpdate>();
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();

            //inputOfferPrice.onValueChanged.AddListener(InputPrice);
            inputOfferPrice.onValueChanged.AddListener(arg0 => DonationSum());
            buttonSend.interactable = false;
            buttonSend.onClick.AddListener(ClickSave);
        }

        private int _price;
        private void DonationSum()
        {
            if (inputOfferPrice.text != null && inputOfferPrice.text != "" 
                                             && inputOfferPrice.text.Length < 9)
            {
                _price = int.Parse(inputOfferPrice.text);
            }
            else
            {
                _price = 0;
            }
            
            var balance = _managerPlayerData.PlayerHUDs.Balance.Value;

            ActiveSendButton(_price > 0 && _price <= balance && _price >= marketPlayer.Transfer.startPrice);
        }

        private void ActiveSendButton(bool state)
        {
            buttonSend.interactable = state;
        }

        private async void ClickSave()
        {
            buttonSend.interactable = false;

            MarketTypesOfRequests.RequestPlayerForTransferSendPriceOffer request = new();
            request.price = _price;
            request.playerTransferId = marketPlayer.Transfer.playerTransferId;
            var result = await _marketTypesOfRequests.POST_PlayerForTransferSendPriceOffer(request);

            if (result)
            {
                await _managerBalanceUpdate.UpdateBalance(request.price);

                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Offer was sent");
                ServiceLocator.GetService<MarketPopupStatus>().SetAction(StatusMarketPopup.None);
                ServiceLocator.GetService<BuildingWindowStatus>().SetAction(StatusBuilding.MarketMain);

                var marketStatus = ServiceLocator.GetService<MarketStatus>();
                marketStatus.StatusAction = marketStatus.lastStatus;
            }

            buttonSend.interactable = true;
        }
    }
}