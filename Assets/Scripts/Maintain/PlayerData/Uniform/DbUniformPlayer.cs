using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LazySoccer.User.Uniform
{
    [CreateAssetMenu(fileName = "UniformPlayer", menuName = "Player/Uniform", order = 1)]
    public class DbUniformPlayer : ScriptableObject
    {
        #region Uniform Player
        [Title("Uniform Player")]
        [SerializeField] private UniformPlayerDictionaty uniform;

        public OneUniform GetUniformPlayer(TypeUniform type)
        {
            if (uniform.ContainsKey(type))
                return uniform[type];
            else return null;
        }

        public void SetUniformPlayer(TypeUniform type, int id, Color color1, Color color2)
        {
            if (uniform.ContainsKey(type))
            {
                uniform[type].id = id;
                uniform[type].color1 = color1;
                uniform[type].color2 = color2;
            }
        }

        public void RandomColor(TypeUniform type)
        {
            if (uniform.ContainsKey(type))
            {
                uniform[type].color1 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
                uniform[type].color2 = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 
            }
        }
#if UNITY_EDITOR
        [Button]
        private void RandomUniform()
        {
            var key = allUniform.Keys.ToList();
            if (uniform.Count != 0) uniform.Clear();
            foreach (TypeUniform type in Enum.GetValues(typeof(TypeUniform)))
            {
                uniform.Add(type, new OneUniform(key[Random.Range(0, key.Count)]));
                RandomColor(type);
            }
        }
#endif
        #endregion

        #region All Uniform
        [Title("All Uniform")]
        [SerializeField] private AllUniformDictinory allUniform;
        #endregion

        #region Prototype Uniform
        [Title("Prototype Uniform")]
        [SerializeField] private List<PrototypeUniform> pattern;
#if UNITY_EDITOR
        [Button]
        private void RandomPattern(int max)
        {
            if(pattern.Count != 0) pattern.Clear();
            for (int i = 0; i < max; i++)
            {
                PrototypeUniform prototype = new PrototypeUniform();
                var all = allUniform[Random.Range(0, allUniform.Count)];
                prototype.uniformShorts = all.uniformShorts;

                prototype.uniformShirt = all.uniformShirt;
                prototype.colorUniformShirt = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                prototype.uniformDetails = all.uniformDetails;
                prototype.colorUniformDetails = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

                pattern.Add(prototype);
            }
        }
#endif
        #endregion
    }

    #region Uniform Player

    [Serializable]
    public class UniformPlayerDictionaty : UnitySerializedDictionary<TypeUniform, OneUniform> { }

    [Serializable]
    public class OneUniform
    {
        public OneUniform(int id)
        {
            this.id = id;
        }
        public int id;
        [HideLabel] public Color color1;
        [HideLabel] public Color color2;
    }

    public enum TypeUniform
    {
        Home,
        Guest,
        Goalkeeper
    }
    #endregion

    #region All Uniform
    [Serializable]
    public class AllUniformDictinory : UnitySerializedDictionary<int, Uniform> { }
    [Serializable]
    public class Uniform
    {
        public Sprite uniformShorts;
        public Sprite uniformShirt;
        public Sprite uniformDetails;
    }
    #endregion

    #region Uniform Player

    [Serializable]
    public class PrototypeUniform 
    {
        public Sprite uniformShorts;

        public Sprite uniformShirt;
        public Color colorUniformShirt;
        public Sprite uniformDetails;
        public Color colorUniformDetails;
    }

    #endregion
}
