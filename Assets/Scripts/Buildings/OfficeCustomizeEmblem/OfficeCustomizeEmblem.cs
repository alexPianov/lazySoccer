using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.OfficeEmblem
{
    public class OfficeCustomizeEmblem : CustomizeTeamEmblem
    {
        public Button saveButton;
        public Button backButton;

        private OfficeCustomizeStatus _officeCustomizeStatus;
        private QuestionPopup _questionPopup;
        
        private void Start()
        {
            base.Start();
            
            _officeCustomizeStatus = ServiceLocator.GetService<OfficeCustomizeStatus>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();
            
            saveButton.onClick.AddListener(Save);
            backButton.onClick.AddListener(Back);

            UpdateCurrentEmblemId();
        }
        
        public async void Save()
        {
            var success = await base.Save(true);

            if (success)
            {
                _officeCustomizeStatus.SetAction(StatusOfficeCustomize.TeamMenu);
            }
        }

        private async void Back()
        {
            var result = await _questionPopup.OpenQuestion("Сhanges will not be saved", "Are you sure?");
            
            if(!result) return;

            UpdateCurrentEmblemId();
            
            _officeCustomizeStatus.SetAction(StatusOfficeCustomize.TeamMenu);
        }

        public void UpdateCurrentEmblemId()
        {
            if (_managerStatisticData.teamStatisticData == null)
            {
                Debug.Log("Team statistic is null");
                return;
            }
            
            if (_managerStatisticData.teamStatisticData.emblem == null)
            {
                CurrentEmblemId = 0;
                return;
            }
            
            CurrentEmblemId = _managerStatisticData.teamStatisticData.emblem.emblemId;
        }
    }
}