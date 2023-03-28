using I2.Loc;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerTrait : MonoBehaviour
    {
        [Title("Refs")]
        public TMP_Text traitAbbreviation;
        public TMP_Text traitName;
        public TMP_Text traitDescription;
        public Image traitBackground;
        
        [Title("Colors")]
        public Sprite positiveTrait;
        public Color positiveColor;
        public Sprite negativeTrait;
        public Color negativeColor;
        public Image slotBackground;
        
        [Title("Data")]
        public PlayerTraits playerTraits;

        public PlayerTraits.Trait SetTraitInfo(TraitName traitName)
        {
            var trait = playerTraits.GetTraitInfo(traitName);
            if (trait == null)
            {
                return null; 
            }
            SetTraitInfo(trait);
            return trait;
        }
        
        public void SetTraitInfo(PlayerTraits.Trait trait)
        {
            if(trait == null) { Debug.Log("Failed to find trait"); return;}

            if (traitName)
            {
                traitName.GetComponent<Localize>().SetTerm($"TeamPlayer-Trait-{trait.traitNameEnum}");
            }
            
            if (traitDescription)
            {
                traitDescription.GetComponent<Localize>().SetTerm($"TeamPlayer-Trait-{trait.traitNameEnum}-Description");
            }

            if (traitAbbreviation)
            {
                traitAbbreviation.GetComponent<Localize>().SetTerm($"TeamPlayer-Trait-{trait.traitNameEnum}-Short");
            }
            
            SetTraitBackground(trait);
        }

        private void SetTraitBackground(PlayerTraits.Trait trait)
        {
            if (traitBackground)
            {
                if (trait.traitIsNegative)
                {
                    traitBackground.sprite = negativeTrait;
                    
                    if (slotBackground) slotBackground.color = negativeColor; 
                }
                else
                {
                    traitBackground.sprite = positiveTrait;
                    
                    if (slotBackground) slotBackground.color = positiveColor; 
                }
            }
        }
    }
}