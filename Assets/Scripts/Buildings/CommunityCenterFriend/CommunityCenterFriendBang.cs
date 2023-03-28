using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.ScriptableStore;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriend
{
    public class CommunityCenterFriendBang : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] private TMP_Text m_NamePlayer;
        [SerializeField] private TMP_Text m_NameTeam;
        [SerializeField] private TMP_Text m_NameUnion;
        [SerializeField] private TMP_Text m_CountMatch;
        [SerializeField] private TMP_Text m_CountWin;
        
        [Title("Image")]
        [SerializeField] private Image m_ImageAvatar;
        [SerializeField] private Image m_ImageTeamEmblem;
        [SerializeField] private Image m_ImageUnionEmblem;

        [Title("Refs")] 
        [SerializeField] private CommunityCenterFriend centerFriend;

        [SerializeField] private GameObject panelUnion;

        public void UpdateBang()
        {
            m_NamePlayer.text = centerFriend.CurrentUser.userName;
            m_NameTeam.text = centerFriend.CurrentUser.team.name;
            
            m_CountMatch.text = centerFriend.CurrentTeamStatistics.globalStat.matchPlayed.ToString();
            m_CountWin.text = centerFriend.CurrentTeamStatistics.globalStat.wins.ToString();

            CheckUnionInfo();
            
            var sprite = GetEmblemSprite(centerFriend.CurrentTeamStatistics);
            if (sprite)
            {
                Debug.Log("sprite: " + sprite.name);
                m_ImageTeamEmblem.sprite = sprite;
            }
        }

        private void CheckUnionInfo()
        {
            var hasUnion = centerFriend.CurrentTeamStatistics.union != null;
            
            panelUnion.SetActive(hasUnion);
            
            if (hasUnion)
            {
                m_NameUnion.text = centerFriend.CurrentTeamStatistics.union.name;

                var unionSprite = GetUnionEmblemSprite
                    (centerFriend.CurrentTeamStatistics.union.emblem);
                
                if (unionSprite != null)
                {
                    m_ImageUnionEmblem.sprite = unionSprite;
                }
            }
        }

        private Sprite GetEmblemSprite(TeamStatistic teamStatistic)
        {
            if (teamStatistic.emblem == null) return null;
            
            return ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(teamStatistic.emblem.emblemId);
        }
        
        private Sprite GetUnionEmblemSprite(Emblem unionEmblem)
        {
            if (unionEmblem == null) return null;
            
            return ServiceLocator.GetService<ManagerSprites>()
                .GetUnionSprite(unionEmblem.emblemId);
        }
    }
}