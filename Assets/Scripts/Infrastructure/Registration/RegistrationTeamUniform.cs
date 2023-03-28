using LazySoccer.SceneLoading.Infrastructure.Customize;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamUniform : CustomizeTeamUniform
    {
        [Title("ButtonSelect")]
        [SerializeField] private Button _backLayout;
        [SerializeField] private Button _nextLayout;

        private void Start()
        {
            base.Start();
            
            _backLayout.onClick.AddListener(OnClickBack);
            _nextLayout.onClick.AddListener(Save);
        }

        private async void Save()
        {
            var success = await base.Save();
            
            if (success)
            {
                ServiceLocator.GetService<CreateTeamStatus>().StatusAction 
                    = StatusCreateTeam.Tutorial;
            }
        }
        
        private void OnClickBack()
        {
            ServiceLocator.GetService<CreateTeamStatus>().StatusAction 
                = StatusCreateTeam.CreateTeam;
        }
    }
}