using System;
using UnityEngine;
using System.Collections;
using Lean.Gui;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour
{
    [SerializeField] private LeanToggle _toggle;
    [SerializeField] private LeanButton _button;
    [SerializeField] private TMP_Text textDate;
    private ManagerAudio _managerAudio;

    private void Start()
    {
        _managerAudio = ServiceLocator.GetService<ManagerAudio>();
    }
    
    public void ActiveButton(bool state)
    {
        if (_button) _button.interactable = state;
    }

    public void SetDate(int date)
    {
        textDate.text = date.ToString();
    }

    public int GetDate()
    {
        return int.Parse(textDate.text);
    }

    public void SelectButton()
    {
        Debug.Log("Set Trigger: " + textDate.text);
        if (_toggle) _toggle.On = true;
        // if (_button) _button.Select();
        CalendarController._calendarInstance.OnDateItemClick(this);
    }
    
    public void OnDateItemClick()
    {
        if(CalendarController._calendarInstance) 
            CalendarController._calendarInstance.OnDateItemClick(this);
    }
}
