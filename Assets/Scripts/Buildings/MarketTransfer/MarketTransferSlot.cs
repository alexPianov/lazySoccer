using System;
using System.Collections.Generic;
using System.Linq;
using LazySoccer.Table;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransferSlot : MonoBehaviour
    {
        [Header("Text Player")]
        public TMP_Text textPlayerName;
        public TMP_Text textGoals;
        public TMP_Text textPosition;

        [Header("Player Traits")] 
        public Transform traitContainer;
        
        [Header("Text Transfer")]
        public TMP_Text textStartDate;
        public TMP_Text textDeadlineDate;
        public TMP_Text textBuyer;
        public TMP_Text textPrice;
        public TMP_Text textRank;
        
        [Header("User Image")]
        public Image imageBuyer;

        public MarketPlayerTransfer Transfer { get; private set; }
        public void SetPlayerInfo(MarketPlayerTransfer transfer)
        {
            Transfer = transfer;
            
            textPlayerName.text = transfer.player.name;
            textGoals.text = transfer.player.goalCount.ToString();
            SetPosition(transfer.player.position);
            
            if (textBuyer && transfer.buyer != null)
            {
                textBuyer.text = transfer.buyer.name;
            }
            else
            {
                if(imageBuyer) imageBuyer.gameObject.SetActive(false);
                if(textBuyer) textBuyer.gameObject.SetActive(false);
            }
        }
        
        public void SetTraitIcons(List<Trait> traits, GameObject traitPrefab)
        {
            if (traits == null || traits.Count == 0)
            {
                return;
            }

            for (var i = 0; i < traitContainer.childCount; i++)
            {
                Destroy(traitContainer.GetChild(i).gameObject);
            }

            int traitIconCount = 0;
            int maxTraitIconCount = 4;
            
            foreach (var trait in traits)
            {
                traitIconCount++;
                
                if(traitIconCount > maxTraitIconCount) return;
                
                var traitIcon = Instantiate(traitPrefab, traitContainer);
                
                if (traitIcon.TryGetComponent(out SlotPlayerTrait slotTrait))
                {
                    var result = slotTrait.SetTraitInfo(trait.name);
                    if (result == null) { Destroy(traitIcon); }
                }
            }
        }

        private void SetPosition(Position position)
        {
            var fieldPosition = position.position.First().ToString();

            if (fieldPosition == "G")
            {
                fieldPosition = "GK";
            }

            var fieldLocation = "";
            if (position.fieldLocation != null)
            {
                fieldLocation = position.fieldLocation.First().ToString();
            }
            
            textPosition.text = fieldLocation + fieldPosition;
        }
        
        public void SetTransfer(MarketPlayerTransfer transfer, int rank)
        {
            textRank.text = $"{rank}.";
            textStartDate.text = DataUtils.GetDate(transfer.startDate);
            
            if (textDeadlineDate) textDeadlineDate.text = DataUtils
                .GetDate(transfer.deadLineDate);
            
            textPrice.text = transfer.currentPrice.ToString();
        }

        public void SetEmblemSprite(Sprite sprite)
        {
            if(imageBuyer && sprite) imageBuyer.sprite = sprite;
        }
    }
}