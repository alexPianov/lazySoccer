using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionMemberStatus : MonoBehaviour
    {
        [Title("Status")] 
        [SerializeField] private Sprite[] spriteStatuses;
        
        public Sprite GetMemberSprite(GeneralClassGETRequest.MemberType memberType)
        {
            if (memberType == GeneralClassGETRequest.MemberType.Simple)
            {
                return null;
            }

            if (memberType == GeneralClassGETRequest.MemberType.Master)
            {
                return spriteStatuses[0];
            }

            if (memberType == GeneralClassGETRequest.MemberType.Officer)
            {
                return spriteStatuses[1];
            }
            
            return spriteStatuses[(int) memberType];
        }
    }
}