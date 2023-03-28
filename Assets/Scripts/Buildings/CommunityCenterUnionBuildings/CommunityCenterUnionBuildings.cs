using LazySoccer.SceneLoading.Buildings.OfficeStatistics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionBuildings : MonoBehaviour
    {
        [Title("Refs")]
        [SerializeField] private CommunityCenterUnion centerUnion;
        [SerializeField] private CommunityCenterUnionBuilding buildingsDisplay;

        [Title("Slots")]
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotContainer;
        
        public void UpdateBuildings()
        {
            if(centerUnion.CurrentUnion == null) return;
            
            ClearSlotContainer();
            
            var unionBuildings = centerUnion.CurrentUnion
                .unionBuildings;

            for (int i = 0; i < unionBuildings.Count; i++)
            {
                var slotInstance = CreateSlotInstance();
                
                if (slotInstance.TryGetComponent(out CommunityCenterUnionBuildingSlot slot))
                {
                    slot.SetInfo(unionBuildings[i], i + 1);
                    slot.SetDisplay(buildingsDisplay);
                }
            }
        }

        private GameObject CreateSlotInstance()
        {
            return Instantiate(slotPrefab, slotContainer);
        }

        private void ClearSlotContainer()
        {
            for (int i = 0; i < slotContainer.childCount; i++)
            {
                Destroy(slotContainer.GetChild(i).gameObject);
            }
        }
    }
}