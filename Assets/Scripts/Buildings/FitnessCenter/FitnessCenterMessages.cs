using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;

public class FitnessCenterMessages : MonoBehaviour
{
    private FitnessCenterTypesOfRequests _fitnessCenter;
    private TeamTypesOfRequests _team;
    private ManagerLoading _managerLoading;

    private void Awake()
    {
        _fitnessCenter = ServiceLocator.GetService<FitnessCenterTypesOfRequests>();
        _team = ServiceLocator.GetService<TeamTypesOfRequests>();
        _managerLoading = ServiceLocator.GetService<ManagerLoading>();
    }
    
    public async UniTask<bool> StartRestoringForm(int playerId)
    {
        return await _fitnessCenter.POST_FitnessCenter(FitnessCenterRequests.RestorePlayerForm, playerId);
    }

    public async UniTask<bool> InstantRestoreForm(int playerId)
    {
        return await _fitnessCenter.POST_FitnessCenter(FitnessCenterRequests.InstantRestorePlayerForm, playerId);
    }
    
    public async UniTask<bool> CancelRestoringForm(int playerId)
    { 
        return await _fitnessCenter.POST_FitnessCenter(FitnessCenterRequests.CancelRestorePlayerForm, playerId);
    }

    public async UniTask UpdateTeam()
    {
        await _team.GET_TeamPlayers(false);
    }
}
