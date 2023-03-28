using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionBuildingContributorSlot : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] private TMP_Text textRate;
        [SerializeField] private TMP_Text textTeamName;
        [SerializeField] private TMP_Text textContribution;

        [Title("Image")]
        [SerializeField] private Image imageEmblem;
        [SerializeField] private Image imageStatus;

        public void SetTeamInfo(GeneralClassGETRequest.UnionTeam unionTeam, int rate)
        {
            textRate.text = $"{rate}.";
            textContribution.text = unionTeam.contribution.ToString();
            textTeamName.text = unionTeam.user.team.name;
        }

        public void SetEmblem(Sprite sprite)
        {
            if (imageEmblem && sprite) imageEmblem.sprite = sprite;
        }
        
        
        public void SetStatus(Sprite spriteStatus)
        {
            imageStatus.enabled = spriteStatus;
            
            if(imageStatus && spriteStatus) imageStatus.sprite = spriteStatus;
        }
    }
}