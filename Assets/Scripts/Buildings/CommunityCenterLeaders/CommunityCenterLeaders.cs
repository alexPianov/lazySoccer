using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterLeaders : MonoBehaviour
    {
        [Title("Filter")]
        [SerializeField] private TMP_Dropdown dropdownFilter;

        [Title("Refs")] 
        [SerializeField] private CommunityCenterFriend.CommunityCenterFriend CenterFriend;
        
        private LeaderFilter currentParam;
        
        public enum LeaderFilter
        {
            ByPower = 0, ByPerformance = 1, ByBalance = 2
        }
        
        [Title("Slot")]
        [SerializeField] private GameObject prefabPlayerLeaderboard;
        [SerializeField] private Transform container;

        [Title("List")] 
        [SerializeField] private List<CommunityCenterLeaderSlot> playerSlots = new();

        private ManagerCommunityData _managerCommunityData;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;
        private ManagerSprites _managerSprites;
        private BuildingWindowStatus _statusBuilding;
        
        private void Awake()
        {
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            _managerSprites = ServiceLocator.GetService<ManagerSprites>();
            _statusBuilding = ServiceLocator.GetService<BuildingWindowStatus>();
            dropdownFilter.onValueChanged.AddListener(Filter);
        }

        private int defaultDropdownValue = 2;
        public void UpdateFilter()
        {
            dropdownFilter.SetValueWithoutNotify(defaultDropdownValue);
            currentParam = (LeaderFilter)defaultDropdownValue;
            UpdateUserWorldRatingList();
        }

        private void Filter(int dropdown)
        {
            currentParam = (LeaderFilter)dropdown;
            UpdateUserWorldRatingList();
            
            ServiceLocator.GetService<ManagerAudio>().PlaySound(ManagerAudio.AudioSound.Action);
        }

        private async void UpdateUserWorldRatingList()
        {
            CreateList();
            
            await _communityCenterTypesOfRequest
                .GET_WorldRating(currentParam, 100, 0, false);

            CreateList();
        }

        private void CreateList()
        {
            var users = _managerCommunityData
                .GetUsersWorldRating(currentParam);

            DeleteAllSlots();

            if (users == null) return;
            
            for (var i = 0; i < users.Count; i++)
            {
                var slotInstance = CreatePrefab();

                if (slotInstance.TryGetComponent(out CommunityCenterLeaderSlot slot))
                {
                    slot.SetInfo(users[i], i + 1);
                    slot.SetEmblem(GetEmblemSprite(users[i].emblem));
                    slot.SetFriendWindow(CenterFriend);
                    
                    playerSlots.Add(slot);
                }
            }
        }

        private Sprite GetEmblemSprite(GeneralClassGETRequest.Emblem emblem)
        {
            if (emblem == null) return null;
            
            return _managerSprites.GetTeamSprite(emblem.emblemId);
        }

        private GameObject CreatePrefab()
        {
            return Instantiate(prefabPlayerLeaderboard, container);
        }

        private void DeleteAllSlots()
        {
            for (var i = 0; i < playerSlots.Count; i++)
            {
                Destroy(playerSlots[i].gameObject);
            }
            
            playerSlots.Clear();
        }
    }
}