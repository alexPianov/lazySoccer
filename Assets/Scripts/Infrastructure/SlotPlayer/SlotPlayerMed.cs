using System;
using I2.Loc;
using LazySoccer.SceneLoading.PlayerData.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerMed : MonoBehaviour
    {
        public Slider healingTime;
        public TMP_Text healingText;

        private int trainingTime = 10;
        
        public void SetTraining(TeamPlayer playerData)
        {
            var timeRemain = playerData.dateOfCompletion.Value.ToLocalTime() 
                             - DateTime.Now.ToLocalTime();

            healingTime.value = trainingTime - timeRemain.Hours;
            healingTime.maxValue = trainingTime;
                
            healingText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param1", timeRemain.Hours.ToString());
                
            healingText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param2", timeRemain.Minutes.ToString());
        }

        public void SetTrauma(int injuriesHealPeriod, TeamPlayer playerData)
        {
            var timeRemain = playerData.dateOfCompletion.Value.ToLocalTime() 
                             - DateTime.Now.ToLocalTime();
            
            Debug.Log("injuriesHealPeriod: " + injuriesHealPeriod);
            
            healingTime.value = injuriesHealPeriod - timeRemain.Hours;
            healingTime.maxValue = injuriesHealPeriod;
                
            healingText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param1", timeRemain.Days.ToString());
                
            healingText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param2", timeRemain.Hours.ToString());
        }
    }
}