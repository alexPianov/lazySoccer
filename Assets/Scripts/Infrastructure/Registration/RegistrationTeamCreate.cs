using System;
using LazySoccer.Network;
using LazySoccer.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamCreate : MonoBehaviour
    {
        [SerializeField] private Button createButton;
        [SerializeField] private RegistrationTeamName teamName;
        [SerializeField] private RegistrationTeamEmbem teamEmbem;
            
        private ManagerLoading _managerLoading;
        private QuestionPopup _questionPopup;

        private void Start()
        {
            _managerLoading = ServiceLocator.GetService<ManagerLoading>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();
            createButton.onClick.AddListener(Save);
        }

        [HideInInspector]
        public bool teamIsCreated;
        private async void Save()
        {
            if (!teamName.IsValid()) return;

            if (!teamIsCreated)
            {
                var result = await _questionPopup
                    .OpenQuestion("Once a team is created, the team name can only be changed in the Office", "Create a team?", "Create");

                if (!result)
                {
                    return;
                }
                
                _managerLoading.ControlLoading(true);

                teamIsCreated = await ServiceLocator.GetService<CreateTeamTypesOfRequest>()
                    .CreateTeam();

                if (!teamIsCreated)
                {
                    teamName.teamName.GetComponent<ValidationField>().ActiveErrorHighlight();
                    _managerLoading.ControlLoading(false);
                    return;
                }
                
                await ServiceLocator.GetService<OfficeTypesOfRequests>()
                    .GetTeamInfo(false);
            }
            else
            {
                _managerLoading.ControlLoading(true);
            }

            var emblemIsSaved = await teamEmbem.Save(false);
            
            _managerLoading.ControlLoading(false);
            
            if (!emblemIsSaved) return; 

            ServiceLocator.GetService<CreateTeamStatus>().StatusAction
                = StatusCreateTeam.CustomizeUniform;
        }
    }
}