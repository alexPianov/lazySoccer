using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCreateTeam : MonoBehaviour
{
    [Title("Button Select")]
    [SerializeField] private Button _nextLayout;
    [SerializeField] private Button _backLayout;

    void Start()
    {
        _nextLayout.onClick.AddListener(OnClickNext);
        _backLayout.onClick.AddListener(OnClickBack);
    }
    
    [Button]
    private async void OnClickNext()
    {
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction 
            = StatusCreateTeam.CreateTeamLoading;

        var createTeamRequests = ServiceLocator.GetService<CreateTeamTypesOfRequest>();
        var commandRequests = ServiceLocator.GetService<CommandTypesOfRequests>();

        await commandRequests.SendMessage(CommandRequests.TopUpAccountTest); // Delete on release
        await commandRequests.SendMessage(CommandRequests.AddNFTTests); // Delete on release
        await UniTask.Delay(1000); // Delete on release
        await createTeamRequests.SendMessage(CreateTeamRequests.CreateTeam);
        await createTeamRequests.SendMessage(CreateTeamRequests.GeneratePlayers);
        await commandRequests.SendMessage(CommandRequests.AddPlayersTraumasTest); // Delete on release
        await commandRequests.SendMessage(CommandRequests.AddPlayersTraitsTest); // Delete on release
        
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction = StatusCreateTeam.StartingPlayers;
    }
    
    [Button]
    private void OnClickBack()
    {
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction = StatusCreateTeam.Back;
    }
}
