using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private string _nameHouse;

    [SerializeField] private int _level;

    [SerializeField] private DateTime startUpgrade;
    [SerializeField] private DateTime finishtUpgrade;
    [SerializeField] private int timeUpgrade;

    [Button]
    private void Upgrade()
    {
        Debug.Log($"{startUpgrade} \n {finishtUpgrade} \n {DateTime.Now}");
        if ((finishtUpgrade - DateTime.Now).TotalMinutes < 0)
        {
            startUpgrade = DateTime.Now;
            finishtUpgrade = startUpgrade.AddMinutes(timeUpgrade);
            Debug.Log(startUpgrade.ToString());
            Debug.Log(finishtUpgrade.ToString());
        }
        else
        {
            TimeSpan start = DateTime.Now - startUpgrade;
            TimeSpan end = finishtUpgrade - DateTime.Now;

            var proj = start.TotalMinutes * 100 / (start + end).TotalMinutes;
            Debug.Log($"ָהוע upgrade {proj}%");

            Debug.Log($"ָהוע upgrade {end}%");
            Debug.Log($"ָהוע upgrade {start}%");
        }
    }
}
