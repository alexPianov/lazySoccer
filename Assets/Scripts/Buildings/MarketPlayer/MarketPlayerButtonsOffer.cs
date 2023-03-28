using System.Collections.Generic;
using LazySoccer.Network;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerButtonsOffer : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] private Button buttonChangePrice;
        [SerializeField] private Button buttonOfferPrice;
        [SerializeField] private List<GameObject> buttonOfferCatalog;
        
        [Header("Refs")]
        [SerializeField] private MarketPlayer _marketPlayer;
        
        private ManagerPlayerData _managerPlayerData;

        private void Start()
        {
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            
            ActiveOfferListButtons(true);
        }

        public void ActiveButtons(bool state)
        {
            if (state)
            {
                OpenOfferButtons();
            }
            else
            {
                CloseOfferButtons();
            }
        }

        private void ActiveOfferListButtons(bool state)
        {
            foreach (var button in buttonOfferCatalog)
            {
                button.SetActive(state);
            }
        }

        private void OpenOfferButtons()
        {
            if (OwnOffer())
            {
                buttonChangePrice.gameObject.SetActive(true);   
                buttonOfferPrice.gameObject.SetActive(false);   
            }
            else
            {
                buttonChangePrice.gameObject.SetActive(false);   
                buttonOfferPrice.gameObject.SetActive(true);   
            }
        }

        private void CloseOfferButtons()
        {
            buttonChangePrice.gameObject.SetActive(false);   
            buttonOfferPrice.gameObject.SetActive(false);   
        }

        private bool OwnOffer()
        {
            var offer = _marketPlayer.Offers
                .Find(offer => offer.manager.userId == _managerPlayerData.PlayerData.UserId);

            return offer != null;
        }
    }
}