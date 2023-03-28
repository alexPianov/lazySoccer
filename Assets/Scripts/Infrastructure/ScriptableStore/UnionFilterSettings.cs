using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings
{
    [CreateAssetMenu(fileName = "Unions", menuName = "Unions/Filter", order = 1)]
    public class UnionFilterSettings : ScriptableObject
    {
        [Title("Search String")] 
        public string searchString;
        
        [Title("Toggles")]
        public bool toggleHideClosed;
        public bool toggleHideFull;
        
        [Title("Slider")]
        [Range(0, 200)] public int sliderRatingMax;
        [Range(0, 200)] public int sliderRatingMin;

        [Title("Setup")] 
        public int maxMembers = 50;
        public int sliderBounds = 200;
    }
}