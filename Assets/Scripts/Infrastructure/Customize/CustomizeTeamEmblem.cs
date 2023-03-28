using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.SceneLoading.Buildings.OfficeEmblem.EmblemSprites;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamEmblem : MonoBehaviour
    {
        public CustomizeTeamEmblemFactory quickTable;
        public CustomizeTeamEmblemFactory popupTable;
        public GameObject popupViewAll;
        public int CurrentEmblemId { get; set; }
        public Image emblemDisplay;
        public Button viewAllButton;
        
        private OfficeTypesOfRequests _officeRequests;
        protected ManagerTeamData _managerStatisticData;
        private ManagerPlayerData _managerPlayerData;

        protected void Start()
        {
            _managerStatisticData = ServiceLocator.GetService<ManagerTeamData>();
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _officeRequests = ServiceLocator.GetService<OfficeTypesOfRequests>();
            
            viewAllButton.onClick.AddListener(OpenPopupTable);
            
            CurrentEmblemId = 0;
        }
        
        public void UpdateQuickTable()
        {
            Debug.Log("UpdateQuickTable");
            quickTable.UpdateEmblemList();
            UpdateScrollbars();
        }

        public void OpenPopupTable()
        {
            Debug.Log("OpenPopupTable");
            popupViewAll.SetActive(true);
            popupTable.UpdateEmblemList();
            UpdateScrollbars();
        }

        public void ClosePopupTable()
        {
            Debug.Log("ClosePopupTable");
            popupViewAll.SetActive(false);
            quickTable.UpdateEmblemList();
            UpdateScrollbars();
        }
        
        public void PickEmblem(EmblemSprite emblem)
        {
            CurrentEmblemId = emblem.emblemId;
            emblemDisplay.sprite = emblem.emblemSprite;
        }

        public void UpdateScrollbars()
        {
            quickTable.UpdateScrollbarValue(CurrentEmblemId);
        }

        public async UniTask<bool> Save(bool globalLoading = true)
        {
            if (_managerStatisticData.teamStatisticData == null)
            {
                Debug.LogError("Failed to find team statistic data");
                return false;
            }
            
            var success = await _officeRequests.POST_ChangeEmblem
                (OfficeRequests.ChangeEmblem, CurrentEmblemId, globalLoading);

            if (!success)
            {
                return false;
            }
            
            _managerStatisticData.teamStatisticData.emblem = new GeneralClassGETRequest.Emblem()
            {
                emblemId = CurrentEmblemId,
                type = "",
                colors = new string[] { "" }
            };
            
            _managerPlayerData.PlayerHUDs.SetEmblem(CurrentEmblemId);

            return true;
        }
    }
}