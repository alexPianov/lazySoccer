using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.StadiumCountries;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivisions
{
    public class StadiumDivisions : CenterSlotList
    {
        [SerializeField] private StadiumDivision.StadiumDivision stadiumDivision;
        [SerializeField] private StadiumDivisionsCountryList countryList;

        private const int requiredNationalCupRank = 1;

        public Tournament currentTournament;
        public bool dontReopen;
        public async UniTask SetTournament(Tournament tournament)
        {
            Debug.Log("SetTournament: " + tournament);

            currentTournament = tournament;
            var countryId = 0;

            DestroyAllSlots();
            countryList.ResetData();
            
            var nationalCountries = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetNationalCountries();

            if (ServiceLocator.GetService<ManagerTeamData>().teamStatisticData.division == null)
            {
                ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);

                countryId = await countryList.GetCountryId(nationalCountries);

                ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(true);

                countryList.ResetData();
                
                dontReopen = false;
                Debug.Log("dontReopen 1 " + dontReopen);
            }
            else
            {
                countryId = ServiceLocator.GetService<ManagerTeamData>()
                    .teamStatisticData.division.country.countryId;
                dontReopen = true;
                Debug.Log("dontReopen 2 " + dontReopen);
            }

            if (currentTournament == Tournament.National_Cup)
            {
                stadiumDivision.SetDivisionInfo(null, currentTournament, countryId);
                return;
            }

            var mineCountry = nationalCountries
                .Find(countries => countries.countryId == countryId);

            if (mineCountry.divisions.Count == 1)
            {
                var division = mineCountry.divisions[0];
                stadiumDivision.SetDivisionInfo(division, currentTournament, countryId);
                
                Debug.Log("dontReopen 3 " + dontReopen);
                return;
            }
            
            ServiceLocator.GetService<BuildingWindowStatus>().OpenQuickLoading(false);
            
            dontReopen = false;
            
            Debug.Log("dontReopen 4 " + dontReopen);
            foreach (var division in mineCountry.divisions)
            {
                if (currentTournament == Tournament.National_Cup && division.rank != requiredNationalCupRank)
                {
                    continue;
                }

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumDivisionsButton divisionButton))
                {
                    divisionButton.SetDivisionStadium(division, currentTournament, countryId);
                    divisionButton.SetMaster(stadiumDivision);
                }
            }
        }
    }
}