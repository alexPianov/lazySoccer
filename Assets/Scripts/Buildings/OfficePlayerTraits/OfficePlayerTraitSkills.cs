using System.Collections.Generic;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerTraitSkills : CenterSlotList
    {
        public void CreateSkills(List<TeamPlayerSkill> skills)
        {
            Debug.Log("CreateSkills: " + skills.Count);
            
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
                }
            }
        }
    }
}