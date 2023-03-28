using Scripts.Infrastructure.Managers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionsCreateButton : MonoBehaviour
    {
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private CommunityCenterUnionsMode UnionsMode;
        
        private ManagerCommunityData _managerCommunityData;

        public void UpdateCreateButton()
        {
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            var userHaveUnion = _managerCommunityData.GetOwnUnionProfile();

            if (UnionsMode.userCanCreateUnions)
            {
                buttonObject.SetActive(userHaveUnion == null);
            }
            else
            {
                buttonObject.SetActive(false);
            }
            
        }
    }
}