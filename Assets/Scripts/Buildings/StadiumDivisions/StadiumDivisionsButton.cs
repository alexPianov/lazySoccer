using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumCountries
{
    public class StadiumDivisionsButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDivision;
        [SerializeField] private Image imageMineDivision;
        
        private DivisionStadium _divisionStadium;
        private Tournament _tournament;
        private StadiumDivision.StadiumDivision _stadiumDivision;
        private int _countryId;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenDivisionStatistics);
        }

        public void SetDivisionStadium(DivisionStadium divisionStadium, Tournament tournament, int countryId)
        {
            _divisionStadium = divisionStadium;
            _tournament = tournament;
            _countryId = countryId;

            CheckMineDivision(divisionStadium.divisionId);
            
            textDivision.GetComponent<Localize>().SetTerm("Stadium-General-Division");
            
            textDivision.GetComponent<LocalizationParamsManager>()
            .SetParameterValue("param", divisionStadium.divisionId.ToString());
        }

        public void CheckMineDivision(int divisionId)
        {
            var teamStatistic = ServiceLocator
                .GetService<ManagerTeamData>().teamStatisticData;

            if (teamStatistic.division == null)
            {
                imageMineDivision.enabled = false; return;
            }
            
            imageMineDivision.enabled = teamStatistic.division.divisionId == divisionId;
        }

        public void SetMaster(StadiumDivision.StadiumDivision stadiumDivision)
        {
            _stadiumDivision = stadiumDivision;
        }

        public void OpenDivisionStatistics()
        {
            _stadiumDivision.SetDivisionInfo(_divisionStadium, _tournament, _countryId);
        }
    }
}