using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionMembers : CenterSlotList
    {
        [Title("Refs")] 
        [SerializeField] private CommunityCenterUnion communityCenterUnion;
        [SerializeField] private CommunityCenterUnionMemberStatus memberStatus;
        public CommunityCenterFriend.CommunityCenterFriend CommunityCenterFriend;

        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private ManagerPlayerData _managerPlayerData;
        
        private void Awake()
        {
            base.Awake();

            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
        }

        public async UniTask UpdateUnionInfo()
        {
            await communityCenterUnion.RefreshUnion(StatusCommunityCenterUnion.Members);
            UpdateMemberList();
        }
        
        public void UpdateMemberList()
        {
            DestroyAllSlots();
            
            var union = communityCenterUnion.CurrentUnion;
            
            if (union == null || union.unionTeams == null) return;

            var userType = GetPlayerAccessType(union);
            var userTeamName = _managerPlayerData.PlayerHUDs.NameTeam.Value;

            for (var i = 0; i < union.unionTeams.Count; i++)
            {
                var memberTeam = union.unionTeams[i];

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterUnionMemberSlot slot))
                {
                    slot.SetInfo(memberTeam, i + 1);
                    slot.SetEmblem(GetEmblemSprite(memberTeam.user.team));
                    slot.SetStatusEmbem(memberStatus.GetMemberSprite(memberTeam.type));
                    slot.SetMemberList(this);
                    
                    if (userTeamName == memberTeam.user.team.name || 
                        memberTeam.type == MemberType.Master)
                    {
                        slot.MenuAccess(MemberType.Simple);
                    }
                    else
                    {
                        if (userType == MemberType.Officer && 
                            memberTeam.type == MemberType.Officer)
                        {
                            slot.MenuAccess(MemberType.Simple); continue;
                        }
                        
                        slot.MenuAccess(userType);
                    }
                }
            }
        }

        private MemberType GetPlayerAccessType(UnionProfile unionProfile)
        {
            var playerTeamName = _managerPlayerData.PlayerHUDs.NameTeam.Value;
            
            foreach (var userTeam in unionProfile.unionTeams)
            {
                if (userTeam.user.team == null)
                {
                    Debug.LogError("Team data is null");
                    return userTeam.type;
                }
                
                if (userTeam.user.team.name == playerTeamName)
                {
                    return userTeam.type;
                } 
            }

            return MemberType.Simple;
        }

        public async UniTask<bool> Promote(int _memberTeamUnionId)
        {
            return await _communityCenterTypesOfRequest
                .POST_UnionMemberPromote(_memberTeamUnionId);
        }
        
        public async UniTask<bool> Demote(int _memberTeamUnionId, string _replacementMemberId = "")
        {
            return await _communityCenterTypesOfRequest
                .POST_UnionMemberDemote(_memberTeamUnionId, _replacementMemberId);
        }
        
        public async UniTask<bool> Kick(int _memberTeamUnionId, string _replacementMasterId = "")
        {
            return await _communityCenterTypesOfRequest
                .POST_UnionMemberKick(_memberTeamUnionId, _replacementMasterId);
        }
    }
}