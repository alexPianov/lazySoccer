using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBuilding : MonoBehaviour
{
    [SerializeField] private GameObject Watch;
    [SerializeField] private Image Time;
    private void Start()
    {
        if(Watch) Watch.SetActive(false);
    }
    public void StartTimer()
    {
        EnableTimer(true);
    }
    public bool EndTimer()
    {
        EnableTimer(false);
        return true;
    }
    public void EnableTimer(bool value)
    {
        if(Watch) Watch.SetActive(value);
    }
    public void Timer(float time)
    {
        if(Time) Time.fillAmount = time;
    }
}
