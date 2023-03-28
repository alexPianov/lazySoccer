using I2.Loc;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerSkill : MonoBehaviour
    {
        [Title("Text")]
        public TMP_Text skillName;
        public TMP_Text skillLevel;
        
        [Title("Image")]
        public Image skillImage;
        
        public TeamPlayerSkill Skill { get; private set; }

        public void SetInfo(TeamPlayerSkill skill, Sprite sprite = null)
        {
            Skill = skill;
            skillName.GetComponent<Localize>().SetTerm("TeamPlayer-Skill-" + skill.name);
            skillLevel.GetComponent<LocalizationParamsManager>().SetParameterValue("param", skill.level.ToString());
            if(skillImage && sprite) skillImage.sprite = sprite;
        }
    }
}