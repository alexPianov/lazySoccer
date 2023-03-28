using System.Collections.Generic;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionBuildingContributors : CenterSlotList
    {
        [Title("Contributors")]
        [SerializeField] private CommunityCenterUnionMemberStatus memberStatus;

        public void SetContributors(List<GeneralClassGETRequest.UnionTeam> list)
        {
            DestroyAllSlots();

            if (list == null || list.Count == 0) return;

            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].contribution == 0) continue;

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterUnionBuildingContributorSlot slot))
                {
                    slot.SetTeamInfo(list[i], i + 1);
                    slot.SetEmblem(GetEmblemSprite(list[i].user.team));
                    slot.SetStatus(memberStatus.GetMemberSprite(list[i].type));
                }
            }
        }
    }
}