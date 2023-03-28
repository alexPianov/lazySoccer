using System;
using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using LazySoccer.Status;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficeCustomize
{
    public class OfficeCustomizeUniformSave : CustomizeTeamUniform
    {
        [SerializeField] protected Button saveButton;
        [SerializeField] protected Button backButton;

        private OfficeCustomizeStatus _officeCustomizeStatus;
        private QuestionPopup _questionPopup;

        private void Start()
        {
            base.Start();
            
            _officeCustomizeStatus = ServiceLocator.GetService<OfficeCustomizeStatus>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();
            
            
            saveButton.onClick.AddListener(Save);
            backButton.onClick.AddListener(Back);
        }

        public async void Save()
        {
            var success = await base.Save();

            if (success)
            {
                _officeCustomizeStatus.SetAction(StatusOfficeCustomize.TeamMenu);
            }
        }

        public async void Back()
        {
            var result = await _questionPopup.OpenQuestion("Сhanges will not be saved", "Are you sure?");
            
            if(!result) return;
            
            _officeCustomizeStatus.SetAction(StatusOfficeCustomize.TeamMenu);
        }
    }
}