using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Buildings;
using LazySoccer.SceneLoading.Buildings.MedicalCenter;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Status;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberList: SlotPlayerFactory
    {
        [Title("List")] 
        public List<SlotPlayer> playerSlots { get; } = new();
        private List<GameObject> freeSlotList = new ();
        private List<GameObject> lockedSlotList = new ();
        
        [SerializeField] private Transform selectFrame;
        [SerializeField] private GameObject freeSlotPrefab;
        [SerializeField] private GameObject lockedSlotPrefab;
        [SerializeField] private CenterMemberDisplay display;

        protected BuildingWindowStatus _buildingWindowStatus;
        protected GeneralPopupMessage _generalPopupMessage;
        
        private void Awake()
        {
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
        }

        #region Update Member List

        public void UpdateMemberList()
        {
            SetSelectFrame(null);
            RemoveAllSlots();
            CreatePlayerList();
            CreateSlots();
            
            AddSelectListener();
            CancelScrollbar();

            if (playerSlots.Count > 0)
            {
                display.SetMember(playerSlots[0]);
            }
            else
            {
                display.SetMember(null);
            }
        }

        protected virtual void CreatePlayerList() {}
        
        protected virtual void CreateSlots() {}

        private void AddSelectListener()
        {
            if(playerSlots == null) return;
            
            for (var i = 0; i < playerSlots.Count; i++)
            {
                var listener = playerSlots[i].gameObject.AddComponent<CenterMemberListener>();
                
                listener.SetTable(this);

                if (i == 0)
                {
                    SelectMember(playerSlots[i]);
                }
            }
        }
        
        private void RemoveAllSlots()
        {
            if(playerSlots == null) return;
            
            for (var i = 0; i < playerSlots.Count; i++)
            {
                Destroy(playerSlots[i].gameObject);
            }
            
            playerSlots.Clear();
        }
        
        #endregion

        #region Free Slots

        protected void CreateFreeSlots(int count, StatusBuilding windowOnClick)
        {
            ClearFreeSlots();
            
            for (var i = 0; i < count; i++)
            {
                CreateFreeSlot(windowOnClick);
            }
        }

        private void CreateFreeSlot(StatusBuilding statusBuilding)
        {
            var freeSlot = CreateSlot(null, freeSlotPrefab, true);
            
            freeSlot.transform.SetAsLastSibling();
            freeSlot.AddComponent<CenterMemberListenerFree>()
                .SetOpenWindow(statusBuilding);
            
            freeSlotList.Add(freeSlot);
        }

        private void ClearFreeSlots()
        {
            if(freeSlotList == null) return;
            
            for (int i = 0; i < freeSlotList.Count; i++)
            {
                Destroy(freeSlotList[i]);
            }

            freeSlotList.Clear();
        }

        #endregion
        
        #region Locked Slots

        protected void CreateLockedSlots(int maxMembers)
        {
            ClearLockedSlots();

            var count = maxMembers - (freeSlotList.Count + playerSlots.Count);
            
            for (var i = 0; i < count; i++)
            {
                CreateLockedSlot();
            }
        }

        private void CreateLockedSlot()
        {
            var lockedSlot = CreateSlot(null, lockedSlotPrefab, true);
            
            lockedSlot.transform.SetAsLastSibling();
            
            lockedSlotList.Add(lockedSlot);
        }

        private void ClearLockedSlots()
        {
            if(lockedSlotList == null) return;
            
            for (int i = 0; i < lockedSlotList.Count; i++)
            {
                Destroy(lockedSlotList[i]);
            }

            lockedSlotList.Clear();
        }

        #endregion

        public void AddMember(SlotPlayer slotPlayer)
        {
            if (playerSlots.Contains(slotPlayer))
            {
                SelectMember(slotPlayer);
                return;
            }
            
            UpdateMemberStatus(slotPlayer);
        }

        protected virtual void UpdateMemberStatus(SlotPlayer slot) { }

        private void SelectMember(SlotPlayer slotPlayer)
        {
            display.SetMember(slotPlayer);
            SetSelectFrame(slotPlayer.transform);
        }
        
        private void SetSelectFrame(Transform selectedSlot)
        {
            selectFrame.SetParent(selectedSlot);
            
            selectFrame.transform.localPosition = new Vector3(0, 0, 0);

            var selectImage = selectFrame.GetComponent<Image>();
            
            if (!selectImage.enabled)
            {
                selectImage.enabled = true;
            }
            
            if (selectedSlot == null)
            {
                selectFrame.transform.localPosition = new Vector3(0, 500, 0);
            }
        }

        public async UniTask UpdateTeamPlayers()
        {
            await ServiceLocator.GetService<TeamTypesOfRequests>().GET_TeamPlayers(false);
        }
    }
}