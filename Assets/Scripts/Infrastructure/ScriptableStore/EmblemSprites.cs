using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeEmblem
{
    [CreateAssetMenu(fileName = "EmblemSprites", menuName = "Building/EmblemSprites", order = 1)]
    public class EmblemSprites: ScriptableObject
    {
        [SerializeField]
        private List<EmblemSprite> emblems;

        [System.Serializable]
        public class EmblemSprite
        {
            public int emblemId;
            public Sprite emblemSprite;
        }

        public EmblemSprite GetEmblemSprite(int emblemId)
        {
            return emblems.Find(emblem => emblem.emblemId == emblemId);
        }

        public List<EmblemSprite> GetAllEmblems()
        {
            return emblems;
        }
    }
}