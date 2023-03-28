using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreateBuildings : MonoBehaviour
    {
        [SerializeField] private Transform containerBuilding;
        public UnionBuildingDonation CurrentBuilding;
        private List<CommunityCenterUnionCreateBuilding> _buildings = new();
        
        public enum UnionBuildingDonation
        {
            UnionOffice, UnionStategyCenter, UnionStaffCenter, None 
        }
        
        private void Awake()
        {
            _buildings = containerBuilding.GetComponentsInChildren<CommunityCenterUnionCreateBuilding>().ToList();
            
            foreach (var building in _buildings)
            {
                building.SetMaster(this);
            }
        }

        public void PickBuilding(UnionBuildingDonation id)
        {
            CurrentBuilding = id;

            foreach (var building in _buildings)
            {
                if (id == UnionBuildingDonation.None)
                {
                    building.EnableDonation(false);
                    building.ActiveButton(true);
                }
                else
                {
                    var enable = building.CurrentBuilding == id;
                    
                    building.ActiveButton(enable);
                }
            }
        }
    }
}