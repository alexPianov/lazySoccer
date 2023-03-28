using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionsUpdate : MonoBehaviour
    {
        public CommunityCenterUnions myUnion;
        public CommunityCenterUnions friendUnions;
        public CommunityCenterUnionsJoinRequest invites;
        public CommunityCenterUnionsJoinRequest requests;
        
        public void UpdateUnions()
        {
            UpdateUnionsTask();
        }
        
        public async UniTask UpdateUnionsTask()
        {
            await myUnion.UpdateUnionsList();
            await friendUnions.UpdateUnionsList();
            await invites.UnionJoinRequests();
            await requests.UnionJoinRequests();
        }
    }
}