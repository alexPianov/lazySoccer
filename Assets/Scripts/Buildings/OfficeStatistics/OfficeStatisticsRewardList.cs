using System;
using System.Collections.Generic;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeStatistics
{
    public class OfficeStatisticsRewardList : MonoBehaviour
    {
        public GameObject rewardPrefab;
        public Transform rewardPrefabContainer;

        public void CreateRewards(List<GeneralClassGETRequest.TeamReward> rewards)
        {
            ClearContainer();
            
            if (rewards == null)
            {
                Debug.Log("Failed to find rewards"); return;
            }

            foreach (var reward in rewards)
            {
                var traitSlot = Instantiate(rewardPrefab, rewardPrefabContainer);

                if (traitSlot.TryGetComponent(out OfficeStatisticsRewardSlot component))
                {
                    component.SetRewardInfo(reward, null);
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