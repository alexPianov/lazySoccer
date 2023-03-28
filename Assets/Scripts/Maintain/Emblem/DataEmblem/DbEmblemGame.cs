using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.User.Emblem
{
    [CreateAssetMenu(fileName = "EmblemList", menuName = "Db/User/ListEmblem")]
    public class DbEmblemGame : ScriptableObject
    {
        public GameObject BaseContainerEmblem;
        public DbEmblemClass Emblems;


#if UNITY_EDITOR
        [SerializeField] private List<Sprite> emblem;
        [Button]
        private void AutoFillDb()
        {
            if(emblem.Count == 0) return;

            for(int i = 0; i < emblem.Count; i++)
            {
                Emblems.Add(i + 1, emblem[i]);
            }
        }
#endif
    }

    [Serializable]
    public class DbEmblemClass: UnitySerializedDictionary<int, Sprite> { }
}
