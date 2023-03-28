using System;
using System.Collections.Generic;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeStatistics
{
    public class OfficeStatisticsTrophiesList : MonoBehaviour
    {
        public GameObject trophyPrefab;
        public Transform trophyPrefabContainer;

        public void CreateTrophies(List<GeneralClassGETRequest.TeamTrophy> trophies)
        {
            ClearContainer();
            
            if (trophies == null)
            {
                Debug.Log("Failed to find trophies"); return;
            }

            foreach (var trophy in trophies)
            {
                var traitSlot = Instantiate(trophyPrefab, trophyPrefabContainer);

                if (traitSlot.TryGetComponent(out OfficeStatisticsTrophiesSlot component))
                {
                    component.SetTrophyInfo(trophy, null);
                }
            }
        }
        
        private void ClearContainer()
        {
            for (int i = 0; i < trophyPrefabContainer.childCount; i++)
            {
                if (trophyPrefabContainer.GetChild(i))
                {
                    Destroy(trophyPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}