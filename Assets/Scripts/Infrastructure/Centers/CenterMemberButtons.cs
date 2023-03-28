using Cysharp.Threading.Tasks;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Buildings;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Table;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberButtons : MonoBehaviour
    {
        [Title("Refs")] 
        [SerializeField] protected Button buttonRemove;
        [SerializeField] protected Button buttonInstant;
        [SerializeField] protected TMP_Text textInstant;
        
        [Title("Player Data")] 
        public SlotPlayer currentPlayer { get; set; }
        public int currentPlayerId { get; set; }
        
        [Title("Economy")] 
        protected Economy economy;

        protected QuestionPopup _questionPopup;
        protected GeneralPopupMessage _generalPopupMessage;
        private ManagerBalanceUpdate _managerBalanceUpdate;
        
        private void Start()
        {
            _managerBalanceUpdate = ServiceLocator.GetService<ManagerBalanceUpdate>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            economy = ServiceLocator.GetService<ManagerEconomy>().GetEconomy();
        }
        
        public void SetButtonsInteracible(bool state)
        {
            buttonRemove.interactable = state;
            buttonInstant.interactable = state;
        }

        public void RemoveButtonListeners()
        {
            buttonRemove.onClick.RemoveAllListeners();
            buttonInstant.onClick.RemoveAllListeners();
        }
        
        public virtual void SetButtonsFunction(TeamPlayerStatus status) { }
        
        protected async UniTask UpdateBalanceLocally(int price)
        {
            Debug.Log("UpdateBalanceLocally: " + price);
            await _managerBalanceUpdate.UpdateBalance(price, false);
        }
    }
}