using System;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.Table
{
    public class SlotPlayerForm : MonoBehaviour
    {
        public Slider restoringFormTime;
        public TMP_Text restoringFormText;

        private const int baseValue = 104;
        private const int updateTime = 10;
        public void SetCenterLevel(int level)
        {
            restoringFormTime.maxValue = baseValue + level;
        }

        public void SetValue(GeneralClassGETRequest.TeamPlayer teamPlayer)
        {
            var timeRemain = teamPlayer.dateOfCompletion.Value.ToLocalTime() 
                             - DateTime.Now.ToLocalTime();
            
            restoringFormTime.value = updateTime - timeRemain.Hours;
            restoringFormTime.maxValue = updateTime;
            
            restoringFormText.GetComponent<Localize>().SetTerm("3-General-Slider-DaysLeft");
            
            restoringFormText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param1", timeRemain.Hours.ToString());
            
            restoringFormText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param2",timeRemain.Minutes.ToString());
        }
    }
}