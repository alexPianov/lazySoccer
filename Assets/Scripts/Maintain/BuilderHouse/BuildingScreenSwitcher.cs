using System;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BuildingScreenSwitcher : MonoBehaviour
{
    [Title("Current House Data")]
    [SerializeField] private TypeHouse type;
    
    [Title("Popup")]
    [SerializeField] private StatusBuilding openWindow;
    
    [Title("Buttons")]
    [SerializeField] private Button button;
    [SerializeField] private Button closeButton;
    
    private BuildingWindowStatus _buildingWindowStatus;
    private ManagerBuilding _building;
    
    public TypeHouse Type
    {
        get { return type; }
        private set { type = value; }
    }

    private void Awake()
    {
        _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
        _building = ServiceLocator.GetService<ManagerBuilding>();
    }

    private void Start()
    {
        if(button) button.onClick.AddListener(ButtonOpenPanel);
        if(closeButton) closeButton.onClick.AddListener(ButtonClosePanel);
    }

    private void ButtonOpenPanel()
    {
        if (type != TypeHouse.None)
        {
            _building.OpenHouse = type;
        }
        
        _buildingWindowStatus.SetAction(openWindow);
    }

    private void ButtonClosePanel()
    {
        _buildingWindowStatus.SetAction(StatusBuilding.None);
    }
}
