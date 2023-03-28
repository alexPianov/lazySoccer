using System.Collections.Generic;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerTraitList : MonoBehaviour
    {
        [Title("Traits")]
        public GameObject traitPrefab;
        public Transform traitPrefabContainer;
        
        public void CreateTraits(List<Trait> traits)
        {
            if (traits == null || traits.Count == 0)
            {
                return;
            }

            ClearContainer();

            foreach (var trait in traits)
            {
                var traitSlot = Instantiate(traitPrefab, traitPrefabContainer);

                if (traitSlot.TryGetComponent(out SlotPlayerTrait component))
                {
                    var result = component.SetTraitInfo(trait.name);
                    
                    if (result == null) { Destroy(traitSlot); }
                }
            }
        }

        private void ClearContainer()
        {
            for (int i = 0; i < traitPrefabContainer.childCount; i++)
            {
                if (traitPrefabContainer.GetChild(i))
                {
                    Destroy(traitPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}