using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace Scripts.Infrastructure.ScriptableStore
{
    [CreateAssetMenu(fileName = "SkillsSprites", menuName = "Player/SkillsSprites", order = 1)]
    public class SkillsSprites : ScriptableObject
    {
        [SerializeField]
        private List<SkillSprite> skills;

        [System.Serializable]
        public class SkillSprite
        {
            public SkillName skillName;
            public Sprite skillSprite;
        }

        public Sprite GetSkillSprite(SkillName skillName)
        {
            var skill = skills.Find(emblem => emblem.skillName == skillName);
            if (skill == null) return null;
            return skill.skillSprite;
        }
    }
}