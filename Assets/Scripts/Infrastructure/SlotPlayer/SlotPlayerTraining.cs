using System;
using I2.Loc;
using LazySoccer.SceneLoading.Buildings.TrainingCenter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerTraining : MonoBehaviour
    {
        public Slider sliderTrainingTime;
        public TMP_Text textTrainingTime;
        private int updateTime = 10;
        
        public void SetValue(TeamPlayer teamPlayer)
        {
            var timeRemain = teamPlayer.dateOfCompletion.Value.ToLocalTime() 
                             - DateTime.Now.ToLocalTime();
            
            sliderTrainingTime.value = updateTime - timeRemain.Hours;
            sliderTrainingTime.maxValue = updateTime;
            
            textTrainingTime.GetComponent<Localize>().SetTerm("3-General-Slider-DaysLeft");
            
            textTrainingTime.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param1", timeRemain.Hours.ToString());
            
            textTrainingTime.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param2",timeRemain.Minutes.ToString());
        }
        
    }
}