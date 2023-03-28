using System;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficeStatistics
{
    public class OfficeStatistics : MonoBehaviour
    {
        [SerializeField] private OfficeStatisticsPosition officeStatisticsPosition;
        [SerializeField] private OfficeStatisticsBuildingList statisticsBuildingList;
        [SerializeField] private OfficeStatisticsRewardList statisticsRewardList;
        [SerializeField] private OfficeStatisticsTrophiesList statisticsTrophiesList;

        [SerializeField] private ManagerTeamData _managerTeamData;

        private void Awake()
        {
            officeStatisticsPosition = GetComponent<OfficeStatisticsPosition>();
            statisticsBuildingList = GetComponent<OfficeStatisticsBuildingList>();
            statisticsRewardList = GetComponent<OfficeStatisticsRewardList>();
            statisticsTrophiesList = GetComponent<OfficeStatisticsTrophiesList>();

            _managerTeamData = ServiceLocator.GetService<ManagerTeamData>();
        }

        public void UpdateStatistics()
        {
            var data = _managerTeamData.teamStatisticData;
            var rewards = _managerTeamData.teamRewardList;
            var trophies =_managerTeamData.teamTrophiesList;
            
            var houses = ServiceLocator
                .GetService<ManagerBuilding>().GetAllHouses();

            officeStatisticsPosition.UpdateInfo(data);
            statisticsBuildingList.CreateBuildings(houses);
            statisticsRewardList.CreateRewards(rewards);
            statisticsTrophiesList.CreateTrophies(trophies);
        }
    }
}