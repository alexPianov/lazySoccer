using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnion;
using LazySoccer.Status;
using LazySoccer.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreate : MonoBehaviour
    {
        [SerializeField] private CommunityCenterUnionEmblem unionEmblem;
        
        [Title("Toggle")]
        [SerializeField] private Toggle toggleOpenStatus;

        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;

        private void Awake()
        {
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
        }

        public async UniTask<bool> Apply(string nameUnion)
        {
            var emblemId = unionEmblem.CurrentEmblemId;
            var policy = 0;
            
            Debug.Log("Embelm Id: " + emblemId + " Name: " + nameUnion);

            var result = await _communityCenterTypesOfRequest
                .POST_UnionCreate(nameUnion, 0);

            if (result)
            {
                var request = new GeneralClassGETRequest.UnionUpdateRequest();

                request.emblemId = emblemId;

                if (toggleOpenStatus.isOn)
                {
                    request.recruitingPolicy = GeneralClassGETRequest.RecruitingPolicy.Open;
                }
                else
                {
                    request.recruitingPolicy = GeneralClassGETRequest.RecruitingPolicy.Close;
                }

                await _communityCenterTypesOfRequest.POST_UnionUpdate(request);
                await _communityCenterTypesOfRequest.GET_UserUnionProfile(false);
                await _communityCenterTypesOfRequest.GET_Unions(false, false);
                
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Union is created!");
                
                ServiceLocator.GetService<CommunityCenterPopupStatus>()
                    .SetAction(StatusCommunityCenterPopup.None);
                
                ServiceLocator.GetService<BuildingWindowStatus>()
                    .SetAction(StatusBuilding.CommunityCenter);
                
                ServiceLocator.GetService<CommunityCenterStatus>()
                    .SetAction(StatusCommunityCenter.Unions);
            }
            
            return result;
        }
    }
}