using System.Collections.Generic;
using LazySoccer.Network;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumMatchesUpdate : MonoBehaviour
    {
        [SerializeField] private List<StadiumMatches> _stadiumMatchesOld = new();

        public void UpdateMatch(Tournament tournament)
        {
            foreach (var stadiumMatches in _stadiumMatchesOld)
            {
                if (stadiumMatches.Tournament == tournament)
                {
                    stadiumMatches.UpdateList();
                }
            }
        }
        
        public async void GenerateMatchTest()
        {
            await ServiceLocator.GetService<CommandTypesOfRequests>().GET_GameGenerator();
            UpdateMatch(Tournament.All);
        }
    }
}