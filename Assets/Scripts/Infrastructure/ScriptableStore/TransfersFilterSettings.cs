using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.MarketTypesOfRequests;

namespace LazySoccer.SceneLoading.Buildings
{
    [CreateAssetMenu(fileName = "Transfers", menuName = "Market/Transfers", order = 1)]
    public class TransfersFilterSettings : ScriptableObject
    {
        [Title("Price Slider")]
        [Range(0, 200000)] public int sliderPriceMax;
        [Range(0, 200000)] public int sliderPriceMin;
        
        [Title("Power Slider")]
        [Range(0, 1000)] public int sliderPowerMax;
        [Range(0, 1000)] public int sliderPowerMin;

        [Title("Slider Setup")]
        public int sliderPriceBounds = 10000;
        public int sliderPowerBounds = 50;

        [Title("Range")] 
        public TransferOrder range;

        [Title("Position")] 
        public int positionId;
        
        [Title("Traits")] 
        public int trait_1;
        public int trait_2;
        public int trait_3;
        public int trait_4;

        public List<int?> GetTraits()
        {
            var traits = new List<int?>();
            if(trait_1 != 0) traits.Add(trait_1);
            if(trait_2 != 0) traits.Add(trait_2);
            if(trait_3 != 0) traits.Add(trait_3);
            if(trait_4 != 0) traits.Add(trait_4);
            return traits;
        }
    }
}