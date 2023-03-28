using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class OfficeBalanceMonthGraph : MonoBehaviour
{
    [SerializeField] private BaseChart balanceGraph;
    [SerializeField] private Scrollbar balanceGraphScrollbar;
    [SerializeField] private TMP_Dropdown balanceMonthlyDropdown;
    
    private Dictionary<int, List<FinancialStatistics>> _monthlyBalances = new();

    private ManagerTeamData _managerStatistic;
    
    private void Start()
    {
        _managerStatistic = ServiceLocator.GetService<ManagerTeamData>();
        
        var xAxis = balanceGraph.GetChartComponent<XAxis>();
        xAxis.type = Axis.AxisType.Value;
        xAxis.boundaryGap = false;

        SetupDropdown();
    }

    private Dictionary<int, List<FinancialStatistics>> MonthlyBalances
        (List<FinancialStatistics> statisticsList)
    {
        Dictionary<int, List<FinancialStatistics>> monthlyBalances = new();
        
        for (int monthNumber = 1; monthNumber <= 12; monthNumber++)
        {
            double monthBalance = 0;
            
            var monthTransactions = statisticsList
                .FindAll(statistics => statistics.createDate.Month == monthNumber);
            
            monthlyBalances.Add(monthNumber, monthTransactions);
        }

        return monthlyBalances;
    }

    private void SetupDropdown()
    {
        balanceMonthlyDropdown.options.Clear();
        
        for (int i = 1; i <= DateTime.Now.Month + 1; i++)
        {
            var data = new TMP_Dropdown.OptionData();
            var zeroPrefix = "0";
            if (i > 9) zeroPrefix = "";
            data.text = String.Format("{0}{1}.{2}", zeroPrefix, i, DateTime.Now.Year);
            balanceMonthlyDropdown.options.Add(data);
        }
    }

    public void UpdateBalance()
    {
        _monthlyBalances = MonthlyBalances(_managerStatistic.seasonBalance);
        balanceMonthlyDropdown.value = DateTime.Now.Month - 1;
    }

    public void ShowMonthBalance(TMP_Dropdown dropdown)
    {
        ShowMonthBalance(dropdown.value + 1);
    }

    public void ShowMonthBalance(int monthNumber)
    {
        Debug.Log("ShowMonthBalance: " + monthNumber);
        balanceGraph.ClearData();

        var currentDay = DateTime.Now.Day;
        var pastDaysInMonth = currentDay;

        if (DateTime.Now.Month != monthNumber)
        {
            currentDay = 1;
            pastDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, monthNumber);
        }

        _monthlyBalances.TryGetValue(monthNumber, out var statisticsList);
        
        double lastDayBalance = GetMonthSummaryBalance(monthNumber - 1);

        for (var i = 1; i <= pastDaysInMonth; i++)
        {
            if (i == 1)
            {
                balanceGraph.AddData("Balance", lastDayBalance, "0", "0");
            }
            
            foreach (var stat in statisticsList)
            {
                if (stat.createDate.Day == i)
                {
                    lastDayBalance += stat.amount;
                }
            }
            
            balanceGraph.AddData("Balance", lastDayBalance, i.ToString(), i.ToString());
        }

        balanceGraphScrollbar.value = ScrollbarValue(currentDay);
        Debug.Log("ScrollbarValue | Day: " + currentDay + " | Value: " + balanceGraphScrollbar.value);
    }

    private float ScrollbarValue(int day)
    {
        switch (day)
        {
            case > 25: return 0.9f;
            case > 20: return 0.7f;
            case > 15: return 0.5f;
            case > 10: return 0.3f;
            case <= 10: return 0f;
            default: return 0.1f;
        }
    }

    private double GetMonthSummaryBalance(int month)
    {
        if (month == 0)
        {
            month = 12;
        }
        
        _monthlyBalances.TryGetValue(month, out var statisticsList);
        
        double summary = 0;

        if (statisticsList == null) return summary;
        
        foreach (var transaction in statisticsList)
        {
            summary += transaction.amount;
        }
        
        return summary;
    }
}
