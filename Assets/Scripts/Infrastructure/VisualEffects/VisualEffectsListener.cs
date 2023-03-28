using System;
using Cysharp.Threading.Tasks;
using Playstel;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.SceneLoading.Infrastructure.VisualEffects.VisualEffectsStore;
using Random = System.Random;

namespace LazySoccer.SceneLoading.Infrastructure.VisualEffects
{
    public class VisualEffectsListener : MonoBehaviour
    {
        public static VisualEffectsListener EffectsListener;
        
        [SerializeField] private Transform container;
        [SerializeField] private UiTransparency background;
        private VisualEffectsStore _effectsStore;

        private void Awake()
        {
            if (EffectsListener == null)
            {
                EffectsListener = this;
            }
            else
            {
                Destroy(EffectsListener);
                EffectsListener = this;
            }
        }

        private void Start()
        {
            _effectsStore = ServiceLocator.GetService<VisualEffectsStore>();
        }

        [Button]
        public void Firework()
        {
            ShowEffect(Effect.Firework);
        }
        
        [Button]
        public async void FireworkLong(int shotsCount = 5)
        {
            background.Transparency(false);
            
            for (int i = 0; i < shotsCount; i++)
            {
                await UniTask.Delay(UnityEngine.Random.Range(150, 350));
                ShowEffect(Effect.FireworkLong);
            }

            await UniTask.Delay(600);
            background.Transparency(true);
        }
        
        private void ShowEffect(Effect effect)
        {
            var effectPrefab = _effectsStore.GetEffect(effect);
            Instantiate(effectPrefab, container);
        }
    }
}