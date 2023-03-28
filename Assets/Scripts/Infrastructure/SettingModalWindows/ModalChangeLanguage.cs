using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazySoccer.SceneLoading.Infrastructure;
using LazySoccer.SceneLoading.Infrastructure.ModalWindows;
using UnityEngine;

public class ModalChangeLanguage : BaseModalWindows
{
    public List<ModalChangeLanguageSlot> _languageSlots = new();

    private ManagerPlayerData _managerPlayerData;
    private ManagerLocalization _managerLocalization;
    private void Start()
    {
        _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
        _managerLocalization = ServiceLocator.GetService<ManagerLocalization>();
        
        base.Start();
    }

    public void SetLanguage(ManagerLocalization.Language language)
    {
        _managerLocalization.SetLanguage(language);
    }

    public void UpdateLanguageList()
    {
        SetMaster();
        
        foreach (var slot in _languageSlots)
        {
            slot.Select(slot.Language == _managerPlayerData.PlayerData.Language);
        }
    }

    private void SetMaster()
    {
        _languageSlots = GetComponentsInChildren<ModalChangeLanguageSlot>().ToList();
        
        foreach (var slot in _languageSlots)
        {
            slot.SetLocalizationManager(this);
        }
    }
}
