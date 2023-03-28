using I2.Loc;
using LazySoccer.SceneLoading.Buildings.Stadium;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace LazySoccer.SceneLoading.Buildings.StadiumDivisionGroupStandings
{
    public class StadiumDivisionGroupStandings : CenterSlotList
    {
        [SerializeField] private StadiumDivision.StadiumDivision StadiumDivision;
        [SerializeField] private TMP_Text textDivisionName;

        public void UpdateGroupStandings()
        {
            Debug.Log("UpdateGroupStandings");

            if (StadiumDivision.DivisionStadium == null)
            {
                textDivisionName.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", null);
            }
            else
            {
                textDivisionName.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param", StadiumDivision.DivisionStadium.divisionId.ToString());
            }
            
            DestroyAllSlots();

            foreach (var tournament in StadiumDivision.MatchesStatistics)
            {
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out StadiumDivisionGroupStandingsSlot slot))
                {
                    slot.SetInfo(tournament);
                }
            }
        }
    }
}