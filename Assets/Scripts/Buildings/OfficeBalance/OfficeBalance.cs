using System;
using System.Collections.Generic;
using LazySoccer.Network;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficeBalance
{
    [DisallowMultipleComponent]
    public class OfficeBalance : MonoBehaviour
    {
        [Title("Display")] 
        [SerializeField] private TMP_Text seasonBalanceText;
        [SerializeField] private TMP_Text dailyBalanceText;

        [Title("Color")] 
        public Color positiveColor;
        public Color negativeColor;
        public Color neutralColor;

        [Title("Graphs")] 
        public OfficeBalancePieGraph BalanceIncomeGraph;
        public OfficeBalancePieGraph BalanceExpensesGraph;
        public OfficeBalanceMonthGraph BalanceMonthGraph;
        
        public enum BalanceType
        {
            Season, Month, PastDay
        }
        
        public void UpdateBalanceTable()
        {
            var _managerStatistic = ServiceLocator.GetService<ManagerTeamData>();
            
            ShowSummary(seasonBalanceText, _managerStatistic.seasonBalance);
            ShowSummary(dailyBalanceText, _managerStatistic.pastDayBalance);

            BalanceIncomeGraph.Clear();
            BalanceIncomeGraph.SetGraph(_managerStatistic.seasonBalance, BalanceType.Season);
            BalanceIncomeGraph.SetGraph(_managerStatistic.pastDayBalance, BalanceType.PastDay);
            
            BalanceExpensesGraph.Clear();
            BalanceExpensesGraph.SetGraph(_managerStatistic.seasonBalance, BalanceType.Season);
            BalanceExpensesGraph.SetGraph(_managerStatistic.pastDayBalance, BalanceType.PastDay);
            
            BalanceMonthGraph.UpdateBalance();
        }

        private void ShowSummary(TMP_Text balanceText, List<FinancialStatistics> statisticsList)
        {
            double seasonSummary = 0;

            if (statisticsList == null)
            {
                Debug.Log("Statistics list is null");
                return;
            }
            
            for (var i = 0; i < statisticsList.Count; i++)
            {
                seasonSummary += statisticsList[i].amount;
            }

            if (seasonSummary == 0)
            {
                balanceText.color = neutralColor;
                balanceText.text = seasonSummary.ToString();
            }
            
            if (seasonSummary > 0)
            {
                balanceText.color = positiveColor;
                balanceText.text = "+" + seasonSummary;
            }
            
            if (seasonSummary < 0)
            {
                balanceText.color = negativeColor;
                balanceText.text = seasonSummary.ToString();
            }
        }
        
    }
}