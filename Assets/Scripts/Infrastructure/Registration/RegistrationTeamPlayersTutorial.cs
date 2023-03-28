using Cysharp.Threading.Tasks;
using DanielLochner.Assets.SimpleScrollSnap;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.Audio;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamPlayersTutorial : MonoBehaviour
    {
        [Title("Button Select")]
        [SerializeField] private Button buttonNext;
        [SerializeField] private Button buttonBack;
        [SerializeField] private Button buttonSkip;

        [Title("Text")]
        [SerializeField] private TMP_Text textButtonNext;
        
        [Title("Scroll Snap")] 
        [SerializeField] private SimpleScrollSnap scrollSnap;
        
        void Start()
        {
            buttonBack.onClick.AddListener(OnClickBack);
            buttonNext.onClick.AddListener(OnClickNext);
            buttonSkip.onClick.AddListener(OnClickSkip);
            scrollSnap.OnPanelCentered.AddListener(OnCentred);
        }

        public void OnClickBack()
        {
            ConfirmButton(false);
            
            scrollSnap.GoToPreviousPanel();
            
            if (scrollSnap.SelectedPanel <= 0)
            {
                Exit();
            }
        }

        public void OnCentred(int panel, int p)
        {
            ConfirmButton(scrollSnap.CenteredPanel == scrollSnap.ItemsCount); 
        }
        
        private void OnClickNext()
        {
            ClickSound();
            
            ConfirmButton(false);
            
            scrollSnap.GoToNextPanel();
        }
        
        private void OnClickSkip()
        {
            scrollSnap.GoToPanel(scrollSnap.ItemsCount);
        }

        private void ConfirmButton(bool state)
        {
            buttonNext.onClick.RemoveAllListeners();
            
            if (state)
            {
                textButtonNext.GetComponent<Localize>().SetTerm("2-Tutorial-Button-Generate");
                buttonNext.onClick.AddListener(Generate);
            }
            else
            {
                textButtonNext.GetComponent<Localize>().SetTerm("2-Tutorial-Button-Next");
                buttonNext.onClick.AddListener(OnClickNext);
            }
            
            buttonSkip.gameObject.SetActive(!state);
        }

        private void ClickSound()
        {
            buttonNext.GetComponent<AudioButton>().PlaySound();
        }

        private async void Generate()
        {
            ClickSound();
            
            var status = ServiceLocator.GetService<CreateTeamStatus>();
            var createTeamRequests = ServiceLocator.GetService<CreateTeamTypesOfRequest>();
            var teamRequests = ServiceLocator.GetService<TeamTypesOfRequests>();
            var commandRequests = ServiceLocator.GetService<CommandTypesOfRequests>();
            
            status.StatusAction = StatusCreateTeam.CreateTeamLoading;

            await commandRequests.POST_Command(CommandRequests.AddNFTTests, false); // Delete on release
            await createTeamRequests.GeneratePlayersReceiptList();
            await commandRequests.POST_Command(CommandRequests.AddPlayersTraitsTest, false); // Delete on release
            await teamRequests.GET_TeamPlayers(false);

            status.StatusAction = StatusCreateTeam.StartingPlayers;
        }
        
        private void Exit()
        {
            ServiceLocator.GetService<CreateTeamStatus>()
                .StatusAction = StatusCreateTeam.CustomizeUniform;
        }
    }
}