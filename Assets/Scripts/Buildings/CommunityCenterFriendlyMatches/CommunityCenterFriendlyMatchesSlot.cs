using LazySoccer.Table;
using Playstel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriendlyMatches
{
    public class CommunityCenterFriendlyMatchesSlot : SlotPlayerMatch
    {
        [Title("Button")] 
        [SerializeField] private Button buttonMenu;
        [SerializeField] private Button buttonMenuClose;
        [SerializeField] private Button buttonCancelMatch;
        [SerializeField] private Button buttonConfirmMatch;
        
        [Title("Panel")] 
        [SerializeField] private UiTransparency panel;
        

        public void Awake()
        {
            buttonMenu.onClick.AddListener(OpenPanel);
            buttonMenuClose.onClick.AddListener(ClosePanel);
            buttonCancelMatch.onClick.AddListener(CancelMatch);
            buttonConfirmMatch.onClick.AddListener(ConfirmMatch);
        }

        private CommunityCenterFriendlyMatches _master;
        public void SetMaster(CommunityCenterFriendlyMatches master)
        {
            _master = master;

            if(Request == null) return;
            
            var masterTeam = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.TeamId;
            buttonConfirmMatch.gameObject.SetActive(Request.sender.team.teamId != masterTeam);
        }

        public void OpenPanel()
        {
            panel.BlocksRaycasts(true);
            panel.Transparency(false);
        }

        public void ClosePanel()
        {
            panel.BlocksRaycasts(false);
            panel.Transparency(true);
        }

        private async void CancelMatch()
        {
            ClosePanel();
            buttonCancelMatch.interactable = false;
            var result = await _master.CancelMatch(RequestId);
            buttonCancelMatch.interactable = true;
            
            if (result)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"Match was rejected");
                
                _master.UpdateMatchesList();
            }
        }
        
        
        private async void ConfirmMatch()
        {
            ClosePanel();
            buttonCancelMatch.interactable = false;
            var result = await _master.CoinfirmMatch(RequestId);
            buttonCancelMatch.interactable = true;
            
            if (result)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"Match was confirmed");
                
                _master.UpdateMatchesList();
            }
        }
    }
}