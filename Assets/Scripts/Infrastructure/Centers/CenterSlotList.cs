using LazySoccer.Network.Get;
using LazySoccer.Popup;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterSlotList : MonoBehaviour
    {
        [Title("Slot")]
        [SerializeField] private GameObject prefabSlot;
        [SerializeField] private Transform prefabContainer;
        [SerializeField] private ScrollRect scrollRect;

        [HideInInspector] public GeneralPopupMessage GeneralPopupMessage;
        [HideInInspector] public QuestionPopup QuestionPopup;

        private ManagerSprites _managerSprites;
        public void Awake()
        {
            _managerSprites = ServiceLocator.GetService<ManagerSprites>();
            GeneralPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            QuestionPopup = ServiceLocator.GetService<QuestionPopup>();
            //FindSlotTip();
        }

        private GameObject _slotTip;
        private void FindSlotTip()
        {
            for (int i = 0; i < prefabContainer.childCount; i++)
            {
                var slot = prefabContainer.GetChild(i).gameObject;
                
                if (slot.name == "Slot_Tip")
                {
                    _slotTip = slot;
                }
            }
        }

        protected Sprite GetEmblemSprite(GeneralClassGETRequest.Team team)
        {
            return ServiceLocator.GetService<ManagerSprites>().GetTeamSprite(team);
        }
        
        protected Sprite GetSkillSprite(AdditionClassGetRequest.TeamPlayerSkill skill)
        {
            return ServiceLocator.GetService<ManagerSprites>().GetSkillSprite(skill);
        }
        
        protected Sprite GetEmblemSprite(GeneralClassGETRequest.User user)
        {
            if (user == null) return null;
            
            return ServiceLocator.GetService<ManagerSprites>().GetTeamSprite(user.team);
        }

        protected Sprite GetEmblemSprite(int unionEmblemId)
        {
            return ServiceLocator.GetService<ManagerSprites>().GetUnionSprite(unionEmblemId);
        }
        
        protected Sprite GetEmblemSprite(GeneralClassGETRequest.Emblem emblem)
        {
            if (emblem == null) return null;
            return ServiceLocator.GetService<ManagerSprites>().GetUnionSprite(emblem.emblemId);
        }
        
        protected GameObject CreateSlot(GameObject prefab = null)
        {
            if (prefab == null) prefab = prefabSlot;

            ActiveTip(false);
            
            return Instantiate(prefab, prefabContainer);
        }

        protected void DestroyAllSlots()
        {
            for (int i = 0; i < prefabContainer.childCount; i++)
            {
                var slot = prefabContainer.GetChild(i).gameObject;
                
                if (slot == _slotTip)
                {
                    ActiveTip(true);
                    continue;
                }
                
                Destroy(prefabContainer.GetChild(i).gameObject);
            }
        }

        public void DisableScrollRect(bool state)
        {
            if(scrollRect) scrollRect.enabled = !state;
        }

        public void ScrollbarVerticalReset()
        {
            if (scrollRect && scrollRect.verticalScrollbar) scrollRect.verticalScrollbar.value = 0;
        }

        private void ActiveTip(bool state)
        {
            if(_slotTip) _slotTip.SetActive(state);
        }
    }
}