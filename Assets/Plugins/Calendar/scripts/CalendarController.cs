using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text _yearNumText;
    public TMP_Text _monthNumText;

    [Header("Dropdown")] 
    public TMP_Dropdown dropdownHours;
    public TMP_Dropdown dropdownMinutes;
    public TMP_Dropdown dropdownDayTime;

    [Header("Day Item")]
    public CalendarDateItem _item;
    
    [Header("Days")]
    public List<CalendarDateItem> _dateItems = new ();
    
    [Header("Buttons Month")] 
    [SerializeField] private Button buttonNextMonth;
    [SerializeField] private Button buttonPrevMonth;

    [Header("Toggles")] 
    [SerializeField] private ToggleGroup _toggleGroup;
    
    const int _totalDateNum = 42;
    
    private DateTime _dateTime;
    public static CalendarController _calendarInstance;
    private int _day = 0;
    private int _hours = 0;
    private int _minutes = 0;
    public bool bounds;
    public int dayBounds;

    public UnityEvent<bool> DayIsSelected;

    private void Awake()
    {
        dropdownHours.onValueChanged.AddListener(SetHours);
        dropdownMinutes.onValueChanged.AddListener(SetMinutes);
        dropdownDayTime.onValueChanged.AddListener(SetDaytime);
        
        buttonNextMonth.onClick.AddListener(MonthNext);
        buttonPrevMonth.onClick.AddListener(MonthPrev);

        dropdownHours.value = 9;
        dropdownMinutes.value = 0;
        dropdownDayTime.value = 1;
    }

    private List<GameObject> itemList = new ();
    public void UpdateCalendar()
    {
        _calendarInstance = this;
        
        DestroyAllItems();
        
        _dateItems.Clear();
        _dateItems.Add(_item);

        Debug.Log("UpdateCalendar");
        
        for (var i = 1; i < _totalDateNum; i++)
        {
            var item = Instantiate(_item.gameObject);
            
            itemList.Add(item);
            
            item.name = "Item_Date_" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;

            var startPos = _item.transform.localPosition;
            
            item.transform.localPosition = new Vector3
                ((i % 7) * 31 + startPos.x, startPos.y - (i / 7) * 31, startPos.z);

            var dateItem = item.GetComponent<CalendarDateItem>();
            
            _dateItems.Add(dateItem);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();
    }

    private void DestroyAllItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        
        itemList.Clear();
    }
    
    public DateTime GetDate()
    {
        DateTime res = DateTime.Now.ToUniversalTime();

        var hours = _hours;
        
        if (_dayTime == DayTime.PM)
        {
            hours += 12;
        }
        
        res = new DateTime(_dateTime.Year, _dateTime.Month, 
            _day, hours, _minutes, 0);

        return res;
    }
    
    private void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        
        int index = GetDays(firstDay.DayOfWeek);
        int date = 0;
        var currentMonth = CurrentMonth();
        
        for (int i = 0; i < _totalDateNum; i++)
        {
            var label = _dateItems[i];
            
            _dateItems[i].gameObject.SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].gameObject.SetActive(true);

                    var currentDate = date + 1;
                    
                    var dayIsPassed = currentMonth && currentDate <= DateTime.Now.Day;
                    label.ActiveButton(!dayIsPassed);

                    if (bounds)
                    {
                        if (currentMonth)
                        {
                            label.ActiveButton(!dayIsPassed && currentDate <= DateTime.Now.Day + dayBounds);
                        }
                        else
                        {
                            label.ActiveButton(false);
                        }
                    }

                    label.SetDate(currentDate);
                    
                    var currentDay = currentMonth && currentDate == DateTime.Now.Day + 1;

                    if (currentDay)
                    {
                        label.SelectButton();
                    }
                    
                    var dayInOtherMonth = !currentMonth && currentDate == 1;

                    if (dayInOtherMonth)
                    {
                        label.SelectButton();
                    }
                    
                    date++;

                    if (DateTime.Now.Day + 1 > DateTime.DaysInMonth
                        (DateTime.Now.Year, DateTime.Now.Month))
                    {
                        //MonthNext();
                    }
                }
            }
        }
        
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.GetComponent<Localize>().SetTerm("3-CommCenterPopup-MatchPlanner-" + _dateTime.Month);
        
        buttonPrevMonth.gameObject.SetActive(!currentMonth);
    }
    
    private bool CurrentMonth()
    {
        return _dateTime.Year == DateTime.Now.Year && _dateTime.Month == DateTime.Now.Month;
    }
    
    private int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }
    
    public void SetHours(int value)
    {
        _hours = int.Parse(dropdownHours.options[value].text);
    }

    public void SetMinutes(int value)
    {
        _minutes = int.Parse(dropdownMinutes.options[value].text);
    }

    private DayTime _dayTime;
    public enum DayTime
    {
        AM,PM
    }
    public void SetDaytime(int value)
    {
        _dayTime = (DayTime) value;
    }

    public void OnDateItemClick(CalendarDateItem item)
    {
        DayIsSelected.Invoke(item != null);
        _day = item.GetDate();
    }
}
