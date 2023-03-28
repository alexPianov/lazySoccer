using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivisions
{
    public class StadiumDivisionsCountryList : CenterSlotList
    {
        private int? PickedCountryId;

        public void ResetData()
        {
            DestroyAllSlots();
            ClearCountryData();
        }
        
        public async UniTask<int> GetCountryId(List<GeneralClassGETRequest.NationalCountries> countries)
        {
            Debug.Log("GetCountryId");
            
            ResetData();
            
            foreach (var country in countries)
            {
                if (country.divisions.Count == 0) continue;

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumDivisionsCountryButton countrySlot))
                {
                    countrySlot.SetNumber(country.countryId);
                    countrySlot.SetMaster(this);
                }
            }

            await UniTask.WaitUntil(() => PickedCountryId != null);

            return (int) PickedCountryId;
        }

        public void SetCountry(int countryId)
        {
            PickedCountryId = countryId;
        }

        public void ClearCountryData()
        {
            PickedCountryId = null;
        }
    }
}