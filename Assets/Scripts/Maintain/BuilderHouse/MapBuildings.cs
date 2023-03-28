using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBuildings : MonoBehaviour
{
    [SerializeField] private ManagerBuilding _managerBuilding;
    [SerializeField] private List<Building> _buildingList;

    public void Start()
    {
        _managerBuilding = ServiceLocator.GetService<ManagerBuilding>();
    }
    
    public void Instance()
    {
        if(_managerBuilding != null)
        {
            _buildingList = GetComponentsInChildren<Building>().ToList();

            SearchSettingHouse();
        }
    }
    private void SearchSettingHouse()
    {
        foreach(var house in _buildingList)
        {
            house.FirstInstance(_managerBuilding.GetHouse(house.Type));
        }
    }
}
