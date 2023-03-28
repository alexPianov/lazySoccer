using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using Scripts.Infrastructure.ScriptableStore;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace Scripts.Infrastructure.Managers
{
    public class ManagerSprites : MonoBehaviour
    {
        [SerializeField] private EmblemSprites EmblemSprites;
        [SerializeField] private StatusSprites StatusSprites;
        [SerializeField] private SkillsSprites SkillsSprites;

        public Sprite GetUnionSprite(Union union)
        {
            if (union.emblem == null)
            {
                Debug.Log("Embiem is null: " + union.name);
                return null;
            }

            return GetUnionSprite(union.emblem.emblemId);
        }
        
        public Sprite GetUnionSprite(int embelmId)
        {
            return EmblemSprites.GetEmblemSprite(embelmId).emblemSprite;
        }

        public Sprite GetTeamSprite(Team team)
        {
            if (team.teamEmblem == null) return null;
            return GetTeamSprite(team.teamEmblem.emblemId);
        }

        public Sprite GetTeamSprite(int emblemId)
        {
            return EmblemSprites.GetEmblemSprite(emblemId).emblemSprite;
        }

        public StatusSprites GetStatusSprites()
        {
            return StatusSprites;
        }
        
        public EmblemSprites GetEmblemSprites()
        {
            return EmblemSprites;
        }
        
        public Sprite GetSkillSprite(TeamPlayerSkill skill)
        {
            if (skill == null) return null;
            return SkillsSprites.GetSkillSprite(skill.name);
        }

    }
}