using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class TrainingCenterMessages : MonoBehaviour
    {
        private TrainingCenterTypesOfRequests _trainingCenter;
        private TeamTypesOfRequests _team;

        private void Awake()
        {
            _trainingCenter = ServiceLocator.GetService<TrainingCenterTypesOfRequests>();
            _team = ServiceLocator.GetService<TeamTypesOfRequests>();
        }
    
        public async UniTask<bool> SwitchTrainingMode(int playerId)
        {
            return await _trainingCenter.POST_TrainingCenter
                (TrainingCenterRequests.TrainingPlayers, playerId);
        }
        
        public async UniTask<bool> CancelTrainingMode(int playerId)
        {
            return await _trainingCenter.POST_TrainingCenter
                (TrainingCenterRequests.CancelTrainingPlayer, playerId);
        }
        
        public async UniTask<bool> InstantTraining(int playerId)
        {
            return await _trainingCenter.POST_TrainingCenterInstant
                (TrainingCenterRequests.InstantTrainingPlayers, playerId);
        }

        public async UniTask<List<TeamPlayerSkill>> GetPlayerSkills(int playerId)
        {
            return await ServiceLocator.GetService<TeamTypesOfRequests>().GET_PlayerSkill(playerId);
        }

        public async UniTask UpdateTeam()
        {
            await _team.GET_TeamPlayers(false);
        }
    }
}