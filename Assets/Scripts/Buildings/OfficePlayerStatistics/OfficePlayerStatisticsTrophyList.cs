using System.Collections.Generic;
using System.Linq;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerStatisticsTrophyList : MonoBehaviour
    {
        [SerializeField] private GameObject rewardPrefab;
        [SerializeField] private Transform rewardPrefabContainer;

        public void CreateTrophy(List<TeamPlayerTrophy> trophies)
        {
            if (trophies == null || trophies.Count == 0) return; 
            
            ClearContainer();

            foreach (var trophy in trophies)
            {
                var slot = Instantiate(rewardPrefab, rewardPrefabContainer);
            
                if (slot.TryGetComponent(out SlotPlayerTrophy component))
                {
                    component.SetInfo(trophy, null);
                }
            }
        }

        private void ClearContainer()
        {
            for (int i = 0; i < rewardPrefabContainer.childCount; i++)
            {
                if (rewardPrefabContainer.GetChild(i))
                {
                    Destroy(rewardPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}