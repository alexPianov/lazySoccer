using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure.VisualEffects
{
    public class VisualEffectsStore : MonoBehaviour
    {
        [SerializeField] private GameObject firework;
        [SerializeField] private GameObject fireworkLong;
        
        public enum Effect
        {
            Firework, FireworkLong
        }

        public GameObject GetEffect(Effect effect)
        {
            switch (effect)
            {
                case Effect.Firework: return firework;
                case Effect.FireworkLong: return fireworkLong;
                default: return firework;
            }
        }
    }
}