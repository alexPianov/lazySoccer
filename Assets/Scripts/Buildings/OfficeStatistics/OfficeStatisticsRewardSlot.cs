using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class OfficeStatisticsRewardSlot : MonoBehaviour
{
    public TMP_Text rewardName;
    public TMP_Text rewardDescription;
    public Image rewardImage;

    public void SetRewardInfo(TeamReward reward, Sprite sprite = null)
    {
        //rewardName.text = reward.reward;
        //rewardTitle.text = reward.title;
        
        rewardName.GetComponent<Localize>().SetTerm($"Team-Reward-{reward.rewardSTR}");
        rewardDescription.GetComponent<Localize>().SetTerm($"Team-Reward-{reward.rewardSTR}-Description");
        
        if(sprite) rewardImage.sprite = sprite;
    }
}
