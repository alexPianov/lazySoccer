using System;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketInGameShopSlot : MonoBehaviour
    {
        [SerializeField] private Image imageBox;
        [SerializeField] private TMP_Text textBoxName;
        [SerializeField] private TMP_Text textBoxPrice;

        [SerializeField] private Button buttonPurchase;
        [SerializeField] private Button buttonInfo;

        private void Start()
        {
            buttonPurchase.onClick.AddListener(Purchase);
            buttonInfo.onClick.AddListener(ShowInfo);
        }

        private MarketInGameShopList _master;
        public void SetMaster(MarketInGameShopList master)
        {
            _master = master;
        }

        public void SetImage(Sprite sprite)
        {
            if (imageBox && sprite) imageBox.sprite = sprite;
        }

        private MarketInGameShop _shopItem;
        public void SetData(MarketInGameShop shopItem)
        {
            _shopItem = shopItem;

            textBoxName.GetComponent<Localize>().SetTerm("LootBox-Title-" + shopItem.tierId);
            textBoxPrice.text = $"<b>{shopItem.price} LAZY</b>";
        }
        
        private async void Purchase()
        {
            buttonPurchase.interactable = false;

            var success = await _master.Purchase(_shopItem);
            
            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Box was purchased");
                
                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Shop;
            }
            
            if(buttonPurchase) buttonPurchase.interactable = true;
        }

        private void ShowInfo()
        {
            _master.ShowInfoPopup(_shopItem);
        }
    }
}