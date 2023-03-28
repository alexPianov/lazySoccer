using LazySoccer.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Utils;

namespace LazySoccer.SceneLoading.Buildings.OfficeBalance
{
    public class OfficeBalanceSlot : MonoBehaviour
    {
        [Title("Mode")]
        public StatisticStatus.BalanceTypes balanceType;
        
        [Title("Text")]
        public TMP_Text seasonValue;
        public TMP_Text yesterdayValue;

        public void SetSeasonValue(double value)
        {
            seasonValue.text = value.ToString();

            if(value < 0) seasonValue.color = DataUtils.GetColorFromHex("D90000");
            if(value > 0) seasonValue.color = DataUtils.GetColorFromHex("52B400");
        }

        public void SetDailyValue(double value)
        {
            yesterdayValue.text = value.ToString();
            
            if(value < 0) yesterdayValue.color = DataUtils.GetColorFromHex("D90000");
            if(value > 0) yesterdayValue.color = DataUtils.GetColorFromHex("52B400");
        }
    }
}