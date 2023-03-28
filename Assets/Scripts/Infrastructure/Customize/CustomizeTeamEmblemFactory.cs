using System.Collections.Generic;
using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamEmblemFactory : MonoBehaviour
    {
        [SerializeField] private GameObject emblemSlotPrefab;
        [SerializeField] private Transform emblemContainer;
        [SerializeField] private Scrollbar emblemListScrollbar;
        [SerializeField] private Transform selectFrame;
        [HideInInspector] public CustomizeTeamEmblem TeamEmbem;
        private ManagerSprites _managerSprites;
        
        private ManagerTeamData _managerStatisticData;
        private List<CustomizeTeamEmblemSlot> emblemSlotList = new();

        private void Awake()
        {
            TeamEmbem = GetComponent<CustomizeTeamEmblem>();
            _managerSprites = ServiceLocator.GetService<ManagerSprites>();
        }

        public void UpdateEmblemList()
        {
            if (emblemSlotList.Count == 0)
            {
                CreateSlots();
                return;
            }
            
            foreach (var slot in emblemSlotList)
            {
                CheckForSelect(slot);
            }
        }

        private void CheckForSelect(CustomizeTeamEmblemSlot slot)
        {
            if (TeamEmbem.CurrentEmblemId == slot.Emblem.emblemId)
            {
                PickSlot(slot);
            }
        }

        private void CreateSlots()
        {
            var emblems = _managerSprites.GetEmblemSprites().GetAllEmblems();

            foreach (var emblem in emblems)
            {
                var emblemSlot = Instantiate(emblemSlotPrefab, emblemContainer);

                if (emblemSlot.TryGetComponent(out CustomizeTeamEmblemSlot slot))
                {
                    slot.SetEmblemFactory(this);
                    slot.SetEmblem(emblem);
                    emblemSlotList.Add(slot);
                    CheckForSelect(slot);
                }
            }
        }

        public void PickSlot(int emblemId)
        {
            foreach (var emblemSlot in emblemSlotList)
            {
                Debug.Log("PickSlot 1: " + emblemId);
                if (emblemSlot.Emblem.emblemId == emblemId)
                {
                    Debug.Log("PickSlot 2: " + emblemId);
                    PickSlot(emblemSlot);
                    break;
                }
            }
        }

        public void PickSlot(CustomizeTeamEmblemSlot emblemSlot)
        {
            Debug.Log("PickSlot with emblem id: " + emblemSlot.Emblem.emblemId);
            
            DeselectAllSlots();
            emblemSlot.SelectDot(true);
            MoveSelectFrame(emblemSlot.transform);
            TeamEmbem.PickEmblem(emblemSlot.Emblem);
        }

        public void UpdateScrollbarValue(float value)
        {
            float emblemsCount = _managerSprites.GetEmblemSprites().GetAllEmblems().Count;
            emblemListScrollbar.value = value / emblemsCount; 
            Debug.Log("emblemListScrollbar value: " + emblemListScrollbar.value 
                                                    + " value " + value 
                                                    + " | count: " + emblemsCount);
        }

        public void DeselectAllSlots()
        {
            foreach (var emblemSlot in emblemSlotList)
            {
                emblemSlot.SelectDot(false);
            }
        }

        private void MoveSelectFrame(Transform parent)
        {
            selectFrame.SetParent(parent);
            selectFrame.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}