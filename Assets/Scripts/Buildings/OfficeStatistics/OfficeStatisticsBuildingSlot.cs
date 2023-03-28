using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using LazySoccet.Building;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class OfficeStatisticsBuildingSlot : MonoBehaviour
{
    public TMP_Text buildingNumber;
    public TMP_Text buildingInfo;
    public TMP_Text buildingCost;
    public TMP_Text buildingLevel;
    public Image buildingImage;
    public void SetInfo(ObjectHouse house, int number = 0)
    {
        if (house.NameHouse == null)
        {
            Debug.LogError("Failed to find house name");
            return;
        }

        if (house.Level == null)
        {
            Debug.LogError("Failed to find house level");
            return;
        }

        if (buildingNumber) buildingNumber.text = $"{number}.";
        
        Debug.Log("house: " + house.TypeHouse);

        buildingInfo.GetComponent<Localize>().SetTerm("Building-" + house.TypeHouse);
        
        buildingLevel.GetComponent<Localize>().SetTerm("Building-Level");
        buildingLevel.GetComponent<LocalizationParamsManager>().SetParameterValue("param", house.Level.Value.ToString());
        
        if(buildingCost) buildingCost.text = house.MaintenanceCost.Value.ToString();
        
        buildingImage.sprite = house.CurrentSprite;
    }
}
