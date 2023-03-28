using I2.Loc;
using LazySoccer.Status;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image imageBox;
        [SerializeField] private TMP_Text textItemName;
        [SerializeField] private TMP_Text textItemSize;

        [SerializeField] private Button buttonOpen;
        [SerializeField] private Button buttonUse;
        [SerializeField] private Button buttonInfo;

        private void Start()
        {
            buttonOpen.onClick.AddListener(Open);
            buttonUse.onClick.AddListener(Use);
            buttonInfo.onClick.AddListener(ShowInfo);
        }

        private MarketInventoryList _master;
        public void SetMaster(MarketInventoryList master)
        {
            Debug.Log("SetMaster:" + master.name);
            _master = master;
        }

        public void SetImage(Sprite sprite)
        {
            if (imageBox && sprite) imageBox.sprite = sprite;
        }

        private MarketInventory _inventoryItem;
        public void SetData(MarketInventory inventoryItem)
        {
            _inventoryItem = inventoryItem;
            
            if (textItemName)
            {
                if (_inventoryItem.status == InventoryStatus.NotActive)
                {
                    Debug.Log("--- InventoryItem: " + inventoryItem.lootBox.loot);
                    
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
                            .SetTerm($"LootBox-{inventoryItem.lootBox.loot}-{inventoryItem.tierId}");
                    }
                    
                    textItemSize.GetComponent<Localize>()
                        .SetTerm("LootBoxSize-" + inventoryItem.lootBox.lootType);
                }
                
                if (_inventoryItem.status == InventoryStatus.NotUnpacked)
                {
                    textItemName.GetComponent<Localize>()
                        .SetTerm("LootBox-Title-" + inventoryItem.tierId);
                    
                    textItemSize.GetComponent<Localize>()
                        .SetTerm("LootBoxStatus-" + inventoryItem.status);
                }
            }

            ButtonActivation(inventoryItem);
        }

        private void ButtonActivation(MarketInventory inventoryItem)
        {
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

        private async void Open()
        {
            buttonOpen.interactable = false;

            var success = await _master.Open(_inventoryItem);
            
            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Box was opened");
                
                ServiceLocator.GetService<MarketStatus>().StatusAction
                    = StatusMarket.Inventory;
            }
            
            buttonOpen.interactable = true;
        }
        
        private async void Use()
        {
            buttonUse.interactable = false;

            var success = await _master.Use(_inventoryItem);
            
            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Loot was used");
            }
            
            buttonUse.interactable = true;
        }

        private void ShowInfo()
        {
            if(_master == null) Debug.LogError("Failed to find master");
            _master.ShowInfoPopup(_inventoryItem);
        }
    }
}