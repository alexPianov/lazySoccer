using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class TrainingCenterMemberSkill : CenterSlotList
    {
        [SerializeField] private TrainingCenterMessages messages;
        [SerializeField] private GameObject panelSkills;

        private int? pickedSkillId = null;
        public async UniTask<int?> GetPlayerSkillId(int playerId)
        {
            panelSkills.SetActive(true);
            pickedSkillId = null;
            cancel = false;
            DestroyAllSlots();
            var skills = await messages.GetPlayerSkills(playerId);
            CreateSkills(skills);
            await UniTask.WaitUntil(() => pickedSkillId != null || cancel);
            panelSkills.SetActive(false);
            cancel = false;
            return pickedSkillId;
        }

        public async UniTask<TeamPlayerSkill> GetUpdatedSkill(int playerId)
        {
            var skills = await messages.GetPlayerSkills(playerId);
            //var skill = skills.Find(skill => skill.status == status);
            return skills[0];
        }

        public void CreateSkills(List<TeamPlayerSkill> skills)
        {
            if (skills == null || skills.Count == 0)
            {
                return;
            }

            DestroyAllSlots();

            foreach (var skill in skills)
            {
                var traitSlot = CreateSlot();

                if (traitSlot.TryGetComponent(out SlotPlayerSkill component))
                {
                    component.SetInfo(skill, GetSkillSprite(skill));
                    
                    component.gameObject
                        .AddComponent<TrainingCenterMemberSkillListener>()
                        .SetMaster(this);
                }
            }
        }

        public void PickSkill(TeamPlayerSkill skill)
        {
            pickedSkillId = skill.playerSkillId;
        }

        private bool cancel;
        public void Cancel()
        {
            cancel = true;
        }
    }
}