using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Status;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image imageItem;
        [SerializeField] private TMP_Text textItemName;
        [SerializeField] private TMP_Text textItemPrice;
        [SerializeField] private TMP_Text textItemDescription;
        [SerializeField] private TMP_Text textItemLootDescription;
        [SerializeField] private TMP_Text textItemLootType;
        [SerializeField] private TMP_Text textItemLootChance;
        
        [SerializeField] private GameObject textItemPricePanel;
        [SerializeField] private GameObject panelShop;
        [SerializeField] private GameObject panelLoot;
        
        [Header("Buttons")]
        [SerializeField] private Button buttonBuy;
        [SerializeField] private Button buttonUse;
        [SerializeField] private Button buttonOpen;

        [Header("Player Picker")] 
        [SerializeField] private MarketItemPlayerPicker _marketItemPlayerPicker;

        private MarketTypesOfRequests _marketTypesOfRequests;
        private GeneralPopupMessage _generalPopupMessage;
        private void Start()
        {
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            
            buttonBuy.onClick.AddListener(ButtonBuy);
            buttonOpen.onClick.AddListener(ButtonOpen);
            buttonUse.onClick.AddListener(ButtonUse);
        }

        private MarketInGameShop shopItem;
        public void SetShopItem(MarketInGameShop item)
        {
            ServiceLocator.GetService<MarketPopupStatus>().StatusAction 
                = StatusMarketPopup.ItemDisplay;

            shopItem = item;
            
            panelShop.SetActive(true);
            panelLoot.SetActive(false);

            SetTierInfo(item.tierId);
            
            textItemPricePanel.SetActive(true);
            textItemPrice.text = $"{item.price} LAZY";
            
            ButtonActivation();
        }

        private MarketInventory inventoryItem;
        public void SetInventoryItem(MarketInventory item)
        {
            ServiceLocator.GetService<MarketPopupStatus>().StatusAction 
                = StatusMarketPopup.ItemDisplay;

            inventoryItem = item;
            
            panelShop.SetActive(false);
            panelLoot.SetActive(false);
            textItemPricePanel.SetActive(false);

            if (item.status == InventoryStatus.NotActive)
            {
                panelLoot.SetActive(true);
                
                if (inventoryItem.lootBox.loot == LootName.SumOfMoney)
                {
                    textItemName.GetComponent<Localize>()
                        .SetTerm($"LootBox-{inventoryItem.lootBox.loot}-" +
                                 $"{inventoryItem.tierId}-" +
                                 $"{inventoryItem.lootBox.lootType}");
                }
                else
                {
                    textItemName.GetComponent<Localize>()
                        .SetTerm($"LootBox-{item.lootBox.loot}-{item.tierId}");
                }
                
                textItemLootDescription.GetComponent<Localize>()
                    .SetTerm($"LootBox-{item.lootBox.loot}-{item.tierId}-Description");
                
                textItemLootType.GetComponent<Localize>()
                    .SetTerm($"LootBoxSize-{item.lootBox.lootType}");
                
                textItemLootChance.GetComponent<Localize>()
                    .SetTerm($"LootBoxChance-{item.lootBox.dropChance}");
            }
            
            if (item.status == InventoryStatus.NotUnpacked)
            {
                panelShop.SetActive(true);
                
                SetTierInfo(item.tierId);
            }

            ButtonActivation(inventoryItem);
        }

        private void SetTierInfo(int tierId)
        {
            textItemName.GetComponent<Localize>().SetTerm("LootBox-Title-" + tierId);
            textItemDescription.GetComponent<Localize>().SetTerm("LootBox-Description-" + tierId);
        }

        private void ButtonActivation(MarketInventory inventoryItem = null)
        {
            if (inventoryItem == null)
            {
                buttonBuy.gameObject.SetActive(true);
                buttonOpen.gameObject.SetActive(false);
                buttonUse.gameObject.SetActive(false);
                return;
            }
            
            buttonBuy.gameObject.SetActive(false);

            if (inventoryItem.status == InventoryStatus.NotUnpacked)
            {
                buttonOpen.gameObject.SetActive(true);
                buttonUse.gameObject.SetActive(false);
            }

            if (inventoryItem.status == InventoryStatus.NotActive)
            {
                buttonOpen.gameObject.SetActive(false);
                buttonUse.gameObject.SetActive(true);
            }

            if (inventoryItem.status == InventoryStatus.Active)
            {
                buttonOpen.gameObject.SetActive(false);
                buttonUse.gameObject.SetActive(false);
            }
        }

        private async void ButtonBuy()
        {
            buttonBuy.interactable = false;

            MarketTypesOfRequests.InGameShopBuyBox buyBox = new();

            buyBox.tierId = shopItem.tierId;
            
            var success = await _marketTypesOfRequests.POST_InGameShopBuyBox(buyBox);

            if (success)
            {
                _generalPopupMessage.ShowInfo("Box was purchased");
                
                ServiceLocator.GetService<MarketPopupStatus>().StatusAction 
                    = StatusMarketPopup.None;
                
                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Shop;
            }

            buttonBuy.interactable = true;
        }

        private async void ButtonOpen()
        {
            buttonOpen.interactable = false;

            MarketTypesOfRequests.InGameShopOpenBox request = new();

            request.inventoryId = inventoryItem.inventoryId;

            var success = await _marketTypesOfRequests.POST_InGameShopOpenBox(request);

            if (success != null)
            {
                _generalPopupMessage.ShowInfo("Box was opened");
                
                ServiceLocator.GetService<MarketPopupStatus>().StatusAction 
                    = StatusMarketPopup.None;
                
                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Inventory;
            }

            buttonOpen.interactable = true;
        }
        
        private async void ButtonUse()
        {
            buttonUse.interactable = false;

            MarketTypesOfRequests.InGameShopUseLoot request = new();

            request.inventoryId = inventoryItem.inventoryId;
            request.chosenId = await _marketItemPlayerPicker.GetChosenId(inventoryItem, StatusMarketPopup.ItemDisplay);

            if (request.chosenId == null)
            {
                buttonUse.interactable = true;
                return;
            }

            var success = await _marketTypesOfRequests.POST_InGameShopUseLoot(request);

            if (success != null)
            {
                _generalPopupMessage.ShowInfo("Loot was used");
                
                ServiceLocator.GetService<MarketPopupStatus>().StatusAction 
                    = StatusMarketPopup.None;

                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Inventory;
            }

            buttonUse.interactable = true;
        }
    }
}