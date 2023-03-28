using System;
using System.Linq;
using I2.Loc;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerTraitSpeciality : MonoBehaviour
    {
        public TMP_Text playerSpeciality;
        public TMP_Text playerDailySalary;
        private TeamStatistic _teamStatistic;

        public void UpdateSpeciality(Position position, int salary)
        {
            //playerDailySalary.text = string.Format("<b>{0}</b> LAZY / day", salary);
            playerDailySalary.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", salary.ToString());
            playerSpeciality.text = DataUtils.GetSpeciality(position);
        }
    }
}