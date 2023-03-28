using System;
using System.Linq;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerStatisticsPerformance : MonoBehaviour
    {
        [Title("Display")]
        [SerializeField] private TMP_Text playerAverageScore;
        [SerializeField] private TMP_Text playerMatchesPlayed;
        [SerializeField] private TMP_Text playerMinutesOnField;
        [SerializeField] private TMP_Text playerGoals;
        [SerializeField] private TMP_Text playerPower;
        [SerializeField] private TMP_Text playerGrowthMatchesPlayed;
        
        public void SetInfo(TeamPlayerStatistics teamPlayerStatistics, int goals = 0)
        {
            if (teamPlayerStatistics.statistics != null)
            {
                playerAverageScore.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", teamPlayerStatistics.statistics.averageScore.ToString());
                playerMatchesPlayed.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", teamPlayerStatistics.statistics.matchPlayed.ToString());
                playerMinutesOnField.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", teamPlayerStatistics.statistics.minutesOnField.ToString());
                playerGrowthMatchesPlayed.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", teamPlayerStatistics.statistics.matchPlayed.ToString());
            }
            else
            {
                playerAverageScore.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", "0");
                playerMatchesPlayed.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", "0");
                playerMinutesOnField.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", "0");
                playerGrowthMatchesPlayed.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", "0");
                playerPower.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", "0");
            }

            playerPower.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", teamPlayerStatistics.power.ToString());
            
            playerGoals.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", goals.ToString());
        }
    }
}