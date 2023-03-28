using System.Collections.Generic;
using I2.Loc;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using LazySoccer.Table;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerInfo : MonoBehaviour
    {
        [Title("Header")] 
        [SerializeField] private MarketPlayer marketPlayer;
        
        [Title("Text")] 
        [SerializeField] private TMP_Text textPlayerName;
        [SerializeField] private TMP_Text textBlitzPrice;
        [SerializeField] private TMP_Text textCurrentHigestPrice;
        [SerializeField] private TMP_Text textDeadline;
        [SerializeField] private TMP_Text textPlayerGoals;
        [SerializeField] private TMP_Text textPlayerPosition;

        [Title("Image")] 
        [SerializeField] private Image imagePlayer;

        [Title("Trait")]
        [SerializeField] private Transform traitContainer;
        [SerializeField] private GameObject traitPrefab;
        
        public void UpdateInfo()
        {
            textPlayerName.text = marketPlayer.Transfer.player.name;

            Debug.Log("marketPlayer.Transfer: " + marketPlayer.Transfer.priceStep);
            
            if (marketPlayer.Transfer.priceStep == 0 || marketPlayer.Transfer.priceStep == null)
            {
                textBlitzPrice.gameObject.SetActive(false);
                Debug.Log("textBlitzPrice: " + false);
            }
            else
            {
                textBlitzPrice.gameObject.SetActive(true);

                textBlitzPrice.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", marketPlayer.Transfer.priceStep.ToString());
                
                Debug.Log("textBlitzPrice: " + true);
            }
            
            textCurrentHigestPrice.GetComponent<LocalizationParamsManager>().SetParameterValue("param", marketPlayer.Transfer.currentPrice.ToString());
            textDeadline.GetComponent<LocalizationParamsManager>().SetParameterValue("param", DataUtils.GetDate(marketPlayer.Transfer.deadLineDate));
            
            textPlayerGoals.text = marketPlayer.Transfer.player.goalCount.ToString();
            textPlayerPosition.text = DataUtils.GetSpecialityAbbreviation(marketPlayer.Transfer.player.position);
            
            CreateTraitIcons(marketPlayer.Transfer.player.traits);
        }

        private void CreateTraitIcons(List<Trait> traits)
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
    }
}