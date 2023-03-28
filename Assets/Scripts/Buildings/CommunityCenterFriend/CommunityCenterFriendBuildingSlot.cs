using I2.Loc;
using LazySoccer.Network.Get;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterFriendBuildingSlot : MonoBehaviour
    {
        public TMP_Text buildingNumber;
        public TMP_Text buildingInfo;
        public TMP_Text buildingCost;
        public TMP_Text buildingLevel;
        public Image buildingImage;

        public void SetInfo(GeneralClassGETRequest.BuildingAll buildingAll, int number = 0, Sprite sprite = null)
        {
            if (buildingNumber) buildingNumber.text = $"{number}.";

            buildingInfo.GetComponent<Localize>().SetTerm("Building-" + buildingAll.buildingType);
        
            buildingLevel.GetComponent<Localize>().SetTerm("Building-Level");
            buildingLevel.GetComponent<LocalizationParamsManager>().SetParameterValue("param", buildingAll.level.ToString());
        
            if(buildingCost) buildingCost.text = buildingAll.maintenanceCost.ToString();
        
            buildingImage.sprite = sprite;
        }
    }
}