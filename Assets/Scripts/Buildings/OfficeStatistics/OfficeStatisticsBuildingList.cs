using System;
using System.Collections.Generic;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnion;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using LazySoccet.Building;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeStatistics
{
    public class OfficeStatisticsBuildingList : MonoBehaviour
    {
        public GameObject buildingPrefab;
        public Transform buildingPrefabContainer;

        public void CreateBuildings(List<ObjectHouse> houses)
        {
            ClearContainer();
            
            if(houses == null) return;

            for (int i = 0; i < houses.Count; i++)
            {
                var buildingSlot = Instantiate(buildingPrefab, buildingPrefabContainer);

                if (buildingSlot.TryGetComponent(out OfficeStatisticsBuildingSlot component))
                {
                    component.SetInfo(houses[i], i + 1);
                }
            }
        }
        
        private void ClearContainer()
        {
            for (int i = 0; i < buildingPrefabContainer.childCount; i++)
            {
                if (buildingPrefabContainer.GetChild(i))
                {
                    Destroy(buildingPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}