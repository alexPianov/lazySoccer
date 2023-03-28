using LazySoccer.Network;
using LazySoccer.Network.Get;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.StadiumBuilding
{
    public class StadiumBuildingFeatures : MonoBehaviour
    {
        [SerializeField] private TMP_Text textMaximumCapacity;
        [SerializeField] private TMP_Text textMaximumTicketPrice;
        [SerializeField] private TMP_Text textMaximumCampingEfficiency;

        public void UpdateInfo()
        {
            var stadium = ServiceLocator.GetService<ManagerBuilding>()
                .BuildingAll.Find(all => all.buildingType == TypeHouse.Stadium);
            
            if(stadium == null) { Debug.LogError("Failed to find stadium info in buildings"); return; }
            if(stadium.influence == null) { Debug.LogError("Failed to find stadium influence in buildings"); return; }

            var capacity = stadium.influence.Find(influence => influence.influence == GeneralClassGETRequest.InfluenceFor
                    .StadiumMaxVisitorsCount);

            if (capacity != null)
            {
                textMaximumCapacity.text = capacity.value.ToString();
            }
            else
            {
                Debug.Log("Failed to find MaximumCapacity int stadium influence"); 
            }

            var maxTicketPrice = stadium.influence
                .Find(influence => influence.influence == GeneralClassGETRequest.InfluenceFor
                    .StadiumMaxTicketPrice);

            if (maxTicketPrice != null)
            {
                textMaximumTicketPrice.text = maxTicketPrice.ToString();
            }
            else
            {
                Debug.Log("Failed to find TicketPrice int stadium influence"); 
            }

            var campingEffectiency = stadium.influence
                .Find(influence => influence.influence == GeneralClassGETRequest.InfluenceFor
                    .StadiumAdvertisementCampaignEfficiency);

            if (campingEffectiency != null)
            {
                textMaximumCampingEfficiency.text = campingEffectiency.value.ToString();
            }
            else
            {
                Debug.Log("Failed to find CampingEfficiency int stadium influence"); 
            }
        }
    }
}