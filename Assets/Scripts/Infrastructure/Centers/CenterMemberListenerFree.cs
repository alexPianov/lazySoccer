using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberListenerFree: MonoBehaviour
    {
        private BuildingWindowStatus _popupStatus;
        
        private void Start()
        {
            _popupStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            GetComponent<Button>().onClick.AddListener(OpenTeamList);
        }

        private StatusBuilding currentStatus;
        public void SetOpenWindow(StatusBuilding building)
        {
            currentStatus = building;
        }

        private void OpenTeamList()
        {
            _popupStatus.SetAction(currentStatus);
        }
    }
}