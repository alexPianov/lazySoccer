using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivisions
{
    public class StadiumDivisionsCountryButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text textCountryName;

        private StadiumDivisionsCountryList _countryList;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(PickCountry);
        }

        private int _countryNumber;

        public void SetNumber(int number)
        {
            _countryNumber = number;
            
            textCountryName.GetComponentInChildren<LocalizationParamsManager>()
                .SetParameterValue("param", _countryNumber.ToString());
        }

        public void SetMaster(StadiumDivisionsCountryList countryList)
        {
            _countryList = countryList;
        }

        private void PickCountry()
        {
            _countryList.SetCountry(_countryNumber);
        }
        
    }
}