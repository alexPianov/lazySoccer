using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerReward: MonoBehaviour
    {
        [Title("Text")]
        public TMP_Text rewardName;
        public TMP_Text rewardDescription;
        public TMP_Text rewardLevel;
        
        [Title("Image")]
        public Image rewardImage;

        public void SetInfo(TeamPlayerReward reward, Sprite sprite = null)
        {
            // rewardName.text = string.Format("{0} level", reward.rewardLVL);
            // rewardDescription.text = reward.description;
            Debug.Log("SetInfo: " + reward.rewardSTR + " | " + reward.rewardLVL);
            rewardName.GetComponent<Localize>().SetTerm($"TeamPlayer-Reward-{reward.reward}-{reward.rewardLVL}");
            rewardDescription.GetComponent<Localize>().SetTerm($"TeamPlayer-Reward-{reward.reward}-{reward.rewardLVL}-Description");
            rewardLevel.GetComponent<LocalizationParamsManager>().SetParameterValue("param", reward.rewardLVL.ToString());
            
            if(sprite) rewardImage.sprite = sprite;
        }
    }
}