using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamUniform : MonoBehaviour
    {
        [SerializeField]
        private List<OfficeCustomizeUniformPatternManager> uniformManagers;

        private TeamTypesOfRequests _teamTypesOfRequests;

        protected void Start()
        {
            _teamTypesOfRequests = ServiceLocator.GetService<TeamTypesOfRequests>();
        }

        public async UniTask<bool> Save()
        {
            if (uniformManagers.Count == 0)
            {
                Debug.Log("Failed to find uniform managers");
                return false;
            }
            
            var uniformsList = new List<Uniform>();

            foreach (var manager in uniformManagers)
            {
                var slot = manager.PickedSlot;
                
                if (slot && slot.CurrentPattern != null && slot.CurrentPattern.CurrentUniform != null)
                {
                    uniformsList.Add(manager.PickedSlot.CurrentPattern.CurrentUniform);
                }
            }

            if (uniformsList.Count > 0)
            {
                var result = await _teamTypesOfRequests
                    .POST_ChangeUniform(TeamRequests.ChangeUniforms, uniformsList);

                if (result)
                {
                    Debug.Log("Change uniform Success");
                    return true;
                }
                
                Debug.LogError("Change uniform Error: " + result);
                return false;
            }
            
            Debug.Log("Nothing to save");
            return true;
        }
    }
}