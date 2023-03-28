using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnions : CenterSlotList
    {
        [Title("Refs")] 
        [SerializeField] private CommunityCenterUnion.CommunityCenterUnion unionDisplay;
        
        [Title("Text")]
        [SerializeField] private TMP_Text textFriendsCount;
        [SerializeField] private TMP_Text textFriendsCountButton;
        
        [Title("Mode")] 
        [SerializeField] private bool getUserUnions;

        [Title("List")] 
        [SerializeField] private List<CommunityCenterUnionSlot> unionSlots = new();

        private ManagerCommunityData _managerCommunityData;
        
        private void Awake()
        {
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            base.Awake();
        }
        
        public async UniTask UpdateUnionsList()
        {
            var unions = GetUnions();
            
            await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .GET_Unions(false, false);

            unions = GetUnions();

            DeleteAllSlots();

            if(unions == null || unions.Count == 0) return;
            
            for (var i = 0; i < unions.Count; i++)
            {
                if(unions[i] == null) continue;
                
                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out CommunityCenterUnionSlot slot))
                {
                    slot.SetInfo(unions[i], i + 1);
                    slot.SetUnionEmblem(GetUnionEmblem(unions[i]));
                    
                    CreateUnionListener(slot);
                    
                    unionSlots.Add(slot);
                }
            }
        }

        private List<Union> GetUnions()
        {
            var unions = _managerCommunityData.GetUnions(getUserUnions);

            textFriendsCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({unions.Count})");
            textFriendsCountButton.GetComponent<LocalizationParamsManager>().SetParameterValue("param", $"({unions.Count})");
            //textFriendsCount.text = $"{titleName} ({unions.Count})";
            //textFriendsCountButton.text = $"{titleName} ({unions.Count})";
            
            return unions;
        }

        private Sprite GetUnionEmblem(Union union)
        {
            return ServiceLocator.GetService<ManagerSprites>().GetUnionSprite(union);
        }

        private void CreateUnionListener(CommunityCenterUnionSlot slot)
        {
            slot.gameObject.AddComponent<CommunityCenterUnionSlotListener>()
                .SetUnionDisplay(unionDisplay);
        }

        private void DeleteAllSlots()
        {
            DestroyAllSlots();
            unionSlots.Clear();
        }
    }
}