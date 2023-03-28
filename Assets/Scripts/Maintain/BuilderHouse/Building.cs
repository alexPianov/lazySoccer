using LazySoccet.Building;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using LazySoccer.Status;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System;
using LazySoccer.Windows;

public class Building : MonoBehaviour 
{ 
    [SerializeField] private TypeHouse type;
    [SerializeField] private StatusBuilding statusBuilding;
    
    public TypeHouse Type
    {
        get { return type; }
        private set { type = value; }
    }
    
    [SerializeField] private ObjectHouse house;

    [Title("House")]
    [SerializeField] private Image ImageHouse;

    [Title("Level")]
    [SerializeField] private ActivePanelHouse panelHouse;

    [Title("Timer")]
    [SerializeField] private TimerBuilding Timer;

    private void Start()
    {
        if(panelHouse == null) panelHouse = GetComponentInChildren<ActivePanelHouse>();
        if (Timer == null) Timer = GetComponentInChildren<TimerBuilding>();
        
        panelHouse.OnClick(ButtonOpenPanel);
    }

    public void FirstInstance(ObjectHouse objectHouse)
    {
        house = objectHouse;
        if (house != null)
        {
            Instance();
        }
        else Debug.LogError($"No ObjectHouse in {this.name}");
    }
    
    private void Instance()
    {
        //panelHouse.NameHouse = house.NameHouse;
        panelHouse.NameHouseLoc = house.TypeHouse.ToString();
        ImageHouse.sprite = house.CurrentSprite;
        panelHouse.LevelTarget(house.Level.Value);

        panelHouse.ActiveActionNew = false;
        panelHouse.ActiveExclamatory = false;

        SubscriptionAction();
    }
    
    private void SubscriptionAction()
    {
        house.Level.onActionUser = null;
        house.InUpgrade.onActionUser = null;
        
        house.Level.onActionUser += LevelUpdate;
        house.InUpgrade.onActionUser += Upgrade;
    }
    private void LevelUpdate(int value)
    {
        panelHouse.LevelTarget(value);
        panelHouse.LevelTarget(house.Level.Value);
    }
    private void Upgrade(bool value)
    {
        if (house.InUpgrade.Value == false)
        {
            house.DateOfStart.Value = DateTime.Now;
            if (Timer == null) Timer.EnableTimer(value);
        }
    }

    private void ButtonOpenPanel()
    {
        ServiceLocator.GetService<ManagerBuilding>().OpenHouse = type;
        ServiceLocator.GetService<BuildingWindowStatus>().SetAction(statusBuilding);
    }
}
