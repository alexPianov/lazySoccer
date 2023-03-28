using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingPlayersTeam : MonoBehaviour
{
    protected ManagerLoading _managerLoading;
    protected LoadingScene _loadingScene;

    private CreateTeamTypesOfRequest _createTeamNetwork; 

    [SerializeField] private Button GenerationReloutTeam;
    [SerializeField] private Button endButton;

    private void Awake()
    {
        _createTeamNetwork = ServiceLocator.GetService<CreateTeamTypesOfRequest>();
        _managerLoading = ServiceLocator.GetService<ManagerLoading>();
        _loadingScene = ServiceLocator.GetService<LoadingScene>();
    }

    private void Start()
    {
        if(GenerationReloutTeam == null) return;
        GenerationReloutTeam.interactable = false;

        endButton.onClick.AddListener(Confirm);
        GenerationReloutTeam.onClick.AddListener(Reroll);
    }

    public void Confirm()
    { 
        LoadParam().Forget();        
    }
    private void Reroll()
    {
        Debug.Log("Reroll");
    }
    
    private async UniTask LoadParam()
    {
        Debug.Log("LoadParam");
        
        _managerLoading.ControlLoading(true);
        
        // await _managerLoading.ActiveteLoading
        //      (ServiceLocator.GetService<AuthPayloadRequest>().GetPayload());

         await _managerLoading.ActiveteLoading
             (ServiceLocator.GetService<UserTypesOfRequests>()
                 .GetUserRequest());
        
         await _managerLoading.ActiveteLoading
             (ServiceLocator.GetService<BuildingTypesOfRequests>()
                 .Get_AllBuildingRequest());
         
         var task = ServiceLocator.GetService<TeamTypesOfRequests>()
             .GET_TeamPlayers();
            
         await _managerLoading.ActiveteLoading(task);
         
        await _loadingScene.AssetLoaderScene("GameLobby", StatusBackground.Active, new UniTask());
        
        _managerLoading.ControlLoading(false);
    }
}
