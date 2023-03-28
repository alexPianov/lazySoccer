using System.Collections.Generic;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerStatisticsRewardsList : MonoBehaviour
    {
        [SerializeField] private GameObject rewardPrefab;
        [SerializeField] private Transform rewardPrefabContainer;

        public void CreateRewards(List<TeamPlayerReward> rewards)
        {
            if (rewards == null || rewards.Count == 0)
            {
                return;
            }
            
            ClearContainer();
            
            foreach (var reward in rewards)
            {
                var slot = Instantiate(rewardPrefab, rewardPrefabContainer);
            
                if (slot.TryGetComponent(out SlotPlayerReward component))
                {
                    component.SetInfo(reward, null);
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