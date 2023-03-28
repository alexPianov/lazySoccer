using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using XCharts.Runtime;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficeBalance
{
    public class OfficeBalancePieGraph: MonoBehaviour
    {
        [Title("Pie Graph")]
        public PieChart PieChart;
    
        [Title("Graph Legent Slots")]
        public List<OfficeBalanceSlot> balanceLegend;
    
        public void Clear()
        {
            PieChart.ClearData();
        }
        
        public void SetGraph(List<FinancialStatistics> financialStatistics, OfficeBalance.BalanceType type)
        {
            var mainSummary = GetMainSummary(financialStatistics);
            
            Debug.Log("Set Graph | mainSummary: " + mainSummary + " | balanceLegend count: " + balanceLegend.Count);
            
            for (var i = 0; i < balanceLegend.Count; i++)
            {
                var balanceName = balanceLegend[i].balanceType.ToString();

                var availableBalance = financialStatistics
                    .FindAll(statistics => statistics.amountOf == balanceName);

                double summary = 0;

                if (availableBalance != null && availableBalance.Count > 0)
                {
                    foreach (var balanceUnit in availableBalance)
                    {
                        summary += balanceUnit.amount;
                    }
                }

                if (type == OfficeBalance.BalanceType.Season)
                {
                    balanceLegend[i].SetSeasonValue(summary);

                    var percent = GetPercent(mainSummary, summary);
                    Debug.Log("percent: " + percent);

                    if (percent < 0) percent = 0;
                    
                    PieChart.AddData("Pie", percent, balanceName, balanceName);
                }
                
                if (type == OfficeBalance.BalanceType.PastDay)
                {
                    balanceLegend[i].SetDailyValue(summary);
                }
            }
        }

        private double GetMainSummary(List<FinancialStatistics> financialStatistics)
        {
            double summary = 0;

            for (var i = 0; i < balanceLegend.Count; i++)
            {
                var balanceName = balanceLegend[i].balanceType.ToString();

                var availableBalance = financialStatistics
                    .FindAll(statistics => statistics.amountOf == balanceName);

                if (availableBalance != null)
                {
                    foreach (var balanceUnit in availableBalance)
                    {
                        summary += balanceUnit.amount;
                    }
                }
            }

            return summary;
        }
        
       private double GetPercent(double mainSummary, double typeSummary)
       {
           if (mainSummary == 0) return 0;
            return System.Math.Round((100 * typeSummary) / mainSummary);
        }
    }
}