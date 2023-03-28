using System;
using System.Collections.Generic;
using I2.Loc;
using LazySoccet.Building;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    [RequireComponent(typeof(Button))]
    public class CommunityCenterUnionBuildingSlot : MonoBehaviour
    {
        [Title("Image")]
        [SerializeField] private Image buildingImage;
        
        [Title("Text")]
        [SerializeField] private TMP_Text buildingNumber;
        [SerializeField] private TMP_Text buildingName;
        [SerializeField] private TMP_Text buildingLevel;
        
        [Title("Slider")]
        [SerializeField] private GameObject panelSlider;
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private TMP_Text textSliderProgress;

        [Title("Button")] 
        [SerializeField] private Button buttonAction;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ShowDisplay);
        }

        private UnionBuilding currentBuilding;
        public void SetInfo(UnionBuilding building, int number = 0)
        {
            currentBuilding = building;
            
            // if (buildingName) buildingName.text = TypeHouseGetter
            //     .GetHouseName(building.buildingLvL.building);
            
            buildingName.GetComponent<Localize>().SetTerm("Building-" + building.buildingLvL.building);
            
            if (buildingNumber) buildingNumber.text = $"{number}.";
            if (buildingLevel) buildingLevel.text = building.buildingLvL.level.ToString();
            if (sliderProgress) SetSlider();
        }

        private void SetSlider()
        {
            panelSlider.SetActive(currentBuilding.isDonationEnabled);
            
            if (currentBuilding.isDonationEnabled)
            {
                sliderProgress.maxValue = currentBuilding.buildingLvL.nextLvL.buildingCost;
                sliderProgress.value = currentBuilding.donations;
                textSliderProgress.text = $"({currentBuilding.donations} / {currentBuilding.buildingLvL.nextLvL.buildingCost})";
            }
        }

        private CommunityCenterUnionBuilding buildingsPopup;
        public void SetDisplay(CommunityCenterUnionBuilding display)
        {
            buildingsPopup = display;
        }

        private void ShowDisplay()
        {
            if (buildingsPopup)
            {
                buildingsPopup.SetInfo(currentBuilding);
            }
        }
    }
}