using System;
using I2.Loc;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using Utils;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficeStatistics
{
    public class OfficeStatisticsPosition : MonoBehaviour
    {
        public TMP_Text divisionInfo;
        public TMP_Text divisionWorldRating;
        public TMP_Text teamPower;
        public TMP_Text teamWins;
        public TMP_Text teamDraws;
        public TMP_Text teamLosses;
        public TMP_Text teamWinRate;

        public void UpdateInfo(TeamStatistic data)
        {
            if (data.division != null)
            {
                divisionInfo.text = DataUtils
                    .GetDivisionDetailsInfo(data.division, data.divisionPlace);
            }
            
            //divisionWorldRating.text = data.worldPlace + " in the world rating";
            divisionWorldRating.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", data.worldPlace.ToString());
            
            teamPower.text = data.totalPower.ToString();

            if (data.globalStat != null)
            {
                teamWins.text = data.globalStat.wins.ToString();
                teamDraws.text = data.globalStat.draws.ToString();
                teamLosses.text = data.globalStat.loses.ToString();
            }
            else
            {
                Debug.Log("Failed to find global stat");
            }

            teamWinRate.text = data.winRate + "%";
        }

    }
}