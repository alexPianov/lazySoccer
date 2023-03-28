using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeCustomize
{
    [CreateAssetMenu(fileName = "UniformPatterns", menuName = "Building/UniformPatterns", order = 1)]
    public class UniformPatterns: ScriptableObject
    {
        [SerializeField]
        private List<UniformPattern> uniforms;

        [System.Serializable]
        public class UniformPattern
        {
            public int uniformId;
            public GameObject patternPrefab;
        }

        public UniformPattern GetUniformPattern(int uniformId)
        {
            return uniforms.Find(pattern => pattern.uniformId == uniformId);
        }

        public List<UniformPattern> GetAllPatterns()
        {
            return uniforms;
        }
        
    }
}