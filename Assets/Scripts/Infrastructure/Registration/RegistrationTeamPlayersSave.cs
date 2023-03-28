using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamPlayersSave: MonoBehaviour
    {
        [SerializeField] private TeamPlayerStatus currentStatus;
        [SerializeField] private Button confirmButton;
        
        private ManagerPayloadBootstrap _managerPayloadBootstrap;
        private CreateTeamTypesOfRequest _createTeamTypesOfRequest;
        
        private void Start()
        {
            _createTeamTypesOfRequest = ServiceLocator.GetService<CreateTeamTypesOfRequest>();
            _managerPayloadBootstrap = ServiceLocator.GetService<ManagerPayloadBootstrap>();
            
            confirmButton.onClick.AddListener(Confirm);
        }

        public void SetOption(TeamPlayerStatus status)
        {
            currentStatus = status;
        }

        private async void Confirm()
        {
            if (currentStatus == TeamPlayerStatus.GenerationOptionOne ||
                currentStatus == TeamPlayerStatus.GenerationOptionTwo ||
                currentStatus == TeamPlayerStatus.GenerationOptionThree)
            {
                var _managerLoading = ServiceLocator.GetService<ManagerLoading>();
                
                _managerLoading.ControlLoading(true);
                
                await _createTeamTypesOfRequest.SavePlayersGenerationOption(currentStatus, false);
                
                await AddCheats(false); //Delete on release
                
                _managerPayloadBootstrap.Boot(false);
            }
            else
            {
                Debug.LogError("Current status must be one of the three generation options");
            }
        }

        private async UniTask AddCheats(bool globalLoading)
        {
            var commandRequests = ServiceLocator.GetService<CommandTypesOfRequests>();
            
            await commandRequests.POST_TopUpAccount(globalLoading);
            //await commandRequests.SendMessage(CommandRequests.AddPlayersLoseFormTest); 
            //await commandRequests.SendMessage(CommandRequests.AddPlayersTraumasTest);
            await commandRequests.POST_Command(CommandRequests.AddPlayersTrophiesRewardsTest, globalLoading);
            await commandRequests.POST_Command(CommandRequests.AddTeamTrophiesRewardsTest, globalLoading);

            await commandRequests.GET_GameGenerator
                (AdditionClassGetRequest.Tournament.Amateur_conference, false);
        }
    }
}