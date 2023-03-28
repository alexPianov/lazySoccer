using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiUtilsExpandGroup : MonoBehaviour
{
    [SerializeField]
    private List<UiUtilsExpand> expandObjectList = new();

    [SerializeField] private bool enableFolderAtStart = true;
    [SerializeField] private int enablingFolderNumber = 0;
    
    
    private void Awake()
    {
        expandObjectList = GetComponentsInChildren<UiUtilsExpand>().ToList();
        
        if (expandObjectList != null && expandObjectList.Count > 0)
        {
            if(enableFolderAtStart) expandObjectList[enablingFolderNumber].ActiveSimple();
        }
    }

    public void AddExpandingObject(UiUtilsExpand expand)
    {
        expandObjectList.Add(expand);
    }

    public void CloseAll()
    {
        foreach (var expandObject in expandObjectList)
        {
            expandObject.CloseExpandContainer();
        }
    }
}