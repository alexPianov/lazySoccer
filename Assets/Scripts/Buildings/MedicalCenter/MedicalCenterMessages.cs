using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;

public class MedicalCenterMessages : MonoBehaviour
{
    private MedicalCenterTypesOfRequests _medicalCenter;
    private TeamTypesOfRequests _team;
    private ManagerLoading _managerLoading;

    private void Awake()
    {
        _medicalCenter = ServiceLocator.GetService<MedicalCenterTypesOfRequests>();
        _team = ServiceLocator.GetService<TeamTypesOfRequests>();
        _managerLoading = ServiceLocator.GetService<ManagerLoading>();
    }
    
    public async UniTask<bool> Heal(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.HealingPlayers, playerId);
    }
    
    public async UniTask<bool> CancelHealing(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.CancelHealingPlayers, playerId);
    }

    public async UniTask<bool> InstantHeal(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.InstantHealingPlayers, playerId);
    }

    public async UniTask<bool> Examinate(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.ExaminationPlayers, playerId);
    }
    
    public async UniTask<bool> CancelExaminate(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.CancelExaminationPlayers, playerId);
    }
    
    public async UniTask<bool> InstantExaminate(int playerId)
    {
        return await _medicalCenter.POST_MedicalCenter(MedicalCenterRequests.InstantExaminationPlayers, playerId);
    }
    
    public async UniTask UpdateTeam()
    {
        await _team.GET_TeamPlayers(false);
    }
}
