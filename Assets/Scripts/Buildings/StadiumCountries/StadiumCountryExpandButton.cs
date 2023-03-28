using I2.Loc;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.StadiumCountries
{
    public class StadiumCountryExpandButton : UiUtilsExpand
    {
        [SerializeField] private TMP_Text textCountryName;
        public void SetCountry(int countryId)
        {
            textCountryName.GetComponent<LocalizationParamsManager>()
                .SetParameterValue("param", countryId.ToString());
        }
    }
}