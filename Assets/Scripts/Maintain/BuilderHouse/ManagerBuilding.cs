using LazySoccet.Building;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class ManagerBuilding : MonoBehaviour
{
    public TypeHouse OpenHouse;
    [SerializeField] private List<ObjectHouse> _houses;

    public ObjectHouse GetHouse(TypeHouse type)
    {
        return _houses.Find(x => x.TypeHouse == type);
    }

    public List<ObjectHouse> GetAllHouses()
    {
        return _houses;
    }

    public List<BuildingAll> BuildingAll { get; private set; }
    public void RequestServer(List<BuildingAll> buildings)
    {
        BuildingAll = buildings;
        
        foreach(BuildingAll building in buildings)
        {
            TypeHouse type = building.buildingType;
            var objectHouse = _houses.Find(x => x.TypeHouse == type);
            UpdateBuilding(objectHouse, building);
        }
    }

    public Sprite GetHouseSprite(int idBuild)
    {
        foreach (var house in _houses)
        {
            if (house.IdBuild.Value == idBuild)
            {
                return house.CurrentSprite;
            }
        }

        return null;
    }
    
    private void UpdateBuilding(ObjectHouse house, BuildingAll building)
    {
        if (house != null)
        {
            house.IdBuild.Value = (int)building.id;
            house.DateOfCompletion.Value = building.dateOfCompletion;
            house.DateOfStart.Value = building.dateOfStart;
            house.BuildingLvLId.Value = building.buildingLvLId;
            house.BuildingType.Value = building.buildingType;
            house.Level.Value = building.level;
            if(building.description != null) house.Description.Value = building.description;
            house.MaintenanceCost.Value = building.maintenanceCost;
            if(building.buildingCost != null) house.BuildingCost.Value = building.buildingCost.Value;
            if(building.costInstantBuilding != null) house.CostInstantBuilding.Value = building.costInstantBuilding.Value;
            if(building.buildTime != null) house.BuildTime.Value = building.buildTime.Value;
            house.IsUpgrading = house.DateOfCompletion.Value != null;
            house.Influences = building.influence;
        }
        else
        {
            Debug.LogError("Error! House is null");
        }
    }
}
