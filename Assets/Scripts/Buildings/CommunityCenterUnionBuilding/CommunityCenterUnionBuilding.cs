using System;
using System.Collections.Generic;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionBuilding : MonoBehaviour
    {
        [Title("Image")] 
        [SerializeField] private Image buildingImage;

        [Title("Text")] 
        [SerializeField] private TMP_Text buildingName;
        [SerializeField] private TMP_Text buildingLevel;
        [SerializeField] private TMP_Text buildingMaintenance;
        [SerializeField] private TMP_Text buildingDescription;
        
        [Title("Refs")] 
        public CommunityCenterUnion Union;
        public CommunityCenterUnionBuildingContributors Contributors;
        public CommunityCenterUnionBuildingDonate Donate;
        
        private UnionBuilding CurrentBuilding;

        public void UpdatePopupInfo()
        {
            var building = Union.CurrentUnion.unionBuildings.Find(building =>
                building.unionBuildingId == CurrentBuilding.unionBuildingId);
            
            SetInfo(building);
        }
        
        public void SetInfo(UnionBuilding building)
        {
            CurrentBuilding = building;
            
            ServiceLocator.GetService<CommunityCenterPopupStatus>()
                .SetAction(StatusCommunityCenterPopup.UnionBuilding);

            //buildingName.text = TypeHouseGetter.GetHouseName(building.buildingLvL.building);
            
            buildingName.GetComponent<Localize>().SetTerm("Building-" + building.buildingLvL.building);
            
            buildingLevel.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", building.buildingLvL.level.ToString());

            buildingMaintenance.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", building.buildingLvL.maintenanceCost.ToString());
            
            buildingDescription.text = building.buildingLvL.description;
            buildingDescription.GetComponent<Localize>().SetTerm("Building-Description-" + building.buildingLvL.building);
            
            Contributors.SetContributors(Union.CurrentUnion.unionTeams);
            Donate.SetUnionBuilding(building);
        }
    }
}