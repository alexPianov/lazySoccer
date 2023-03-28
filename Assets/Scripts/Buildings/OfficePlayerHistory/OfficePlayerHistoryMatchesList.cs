using System.Collections.Generic;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayerHistory
{
    public class OfficePlayerHistoryMatchesList : MonoBehaviour
    {
        [SerializeField] private GameObject matchesPrefab;
        [SerializeField] private Transform matchesPrefabContainer;
        
        public void CreateMatches(List<Match> matchList)
        {
            ClearContainer();
            
            if (matchList == null || matchList.Count == 0)
            {
                return;
            }
            
            foreach (var match in matchList)
            {
                if(match.status == GameStatus.WaitingForLineup) continue;
                
                var slot = Instantiate(matchesPrefab, matchesPrefabContainer);
            
                if (slot.TryGetComponent(out SlotPlayerMatch component))
                {
                    component.SetInfo(match);
                }
            }
        }

        private void ClearContainer()
        {
            for (int i = 0; i < matchesPrefabContainer.childCount; i++)
            {
                if (matchesPrefabContainer.GetChild(i))
                {
                    Destroy(matchesPrefabContainer.GetChild(i).gameObject);
                }
            }
        }
    }
}