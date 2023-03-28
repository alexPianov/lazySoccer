using I2.Loc;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumCountries
{
    public class StadiumCountryExpandContainer : CenterSlotList
    {
        public UiUtilsExpand UtilsExpand;
        [SerializeField] private TMP_Text textCountryName;
        
        private const int requiredNationalCupRank = 1;
        
        public void CreateDivisionList(NationalCountries country, Tournament tournament, StadiumDivision.StadiumDivision stadiumDivision)
        {
            textCountryName.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", country.countryId.ToString());
            
            foreach (var division in country.divisions)
            {
                if (tournament == Tournament.National_Cup && division.rank != requiredNationalCupRank) continue;

                var slotInstance = CreateSlot();

                // if (slotInstance.TryGetComponent(out StadiumCountryDivisionButton divisionButton))
                // {
                //     divisionButton.SetDivisionStadium(division, tournament);
                //     divisionButton.SetMaster(stadiumDivision);
                // }
            }
        }
    }
}