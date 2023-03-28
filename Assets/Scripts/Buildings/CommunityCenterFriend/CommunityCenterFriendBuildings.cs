using System.Collections.Generic;
using LazySoccer.Network.Get;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterFriendBuildings : MonoBehaviour
    {
        public GameObject buildingPrefab;
        public Transform buildingPrefabContainer;

        private ManagerBuilding _managerBuilding;
        private void Awake()
        {
            _managerBuilding = ServiceLocator.GetService<ManagerBuilding>();
        }

        public void CreateBuildings(List<GeneralClassGETRequest.BuildingAll> houses)
        {
            ClearContainer();
            
            if(houses == null) return;

            for (int i = 0; i < houses.Count; i++)
            {
                var traitSlot = Instantiate(buildingPrefab, buildingPrefabContainer);

                if (traitSlot.TryGetComponent(out CommunityCenterFriendBuildingSlot component))
                {
                    component.SetInfo(houses[i], i + 1, 
                        _managerBuilding.GetHouseSprite(houses[i].id));
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