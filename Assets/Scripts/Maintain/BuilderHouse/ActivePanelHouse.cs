using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;
using I2.Loc;
using UnityEngine.Events;

public class ActivePanelHouse : MonoBehaviour
{
    [Title("Name House")]
    [SerializeField] private TMP_Text nameHouse;
    // public string NameHouse
    // {
    //     get { return nameHouse.text; }
    //     set 
    //     { 
    //         nameHouse.text = value;
    //     }
    // }
    
    public string NameHouseLoc
    {
        get { return nameHouse.text; }
        set 
        { 
            nameHouse.GetComponent<Localize>().SetTerm("Building-" + value);
        }
    }

    [Title("Text Level")]
    [SerializeField] private TMP_Text m_LevelText;

    [Title("Icon New Active")]
    [SerializeField] private Image ImageNew;
    public bool ActiveActionNew
    {
        private get { return ImageNew.enabled; }
        set { ImageNew.enabled = value; }
    }

    [Title("Active")]
    [SerializeField] private Image Exclamatory;
    public bool ActiveExclamatory
    {
        private get { return Exclamatory.enabled; }
        set { Exclamatory.enabled = value; }
    }

    public void LevelTarget(int level)
    {
        if (m_LevelText && m_LevelText.GetComponent<LocalizationParamsManager>())
        {
            m_LevelText.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", level.ToString());   
        }
    }

    [Title("Button")]
    [SerializeField] private Button openPanel;

    public void OnClick(UnityAction a)
    {
        openPanel.onClick.AddListener(a);
    }
}
