using System;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using LazySoccer.Status;
using LazySoccer.Table;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.Network.MarketTypesOfRequests;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransferEdit : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField inputPrice;
        [SerializeField] private TMP_InputField inputPriceBlitz;

        [Header("Toggle")]
        [SerializeField] private Toggle toggleBlitzPrice;
        [SerializeField] private GameObject panelBlitz;
        
        [Header("Deadline")]
        [SerializeField] private MarketTransferDeadlinePlanner _deadlinePlanner;
        [SerializeField] private TMP_Text textDeadlineDate;
        [SerializeField] private TMP_Dropdown dropdownDeadline;
        
        [Header("Save")]
        [SerializeField] private Button buttonOpenRoster; 
        [SerializeField] private Button buttonSave;

        [Header("Player")]
        [SerializeField] private MarketItemPlayerPicker _marketItemPlayerPicker;
        [SerializeField] private SlotPlayer _slotPlayer;
        [SerializeField] private GameObject panelCardPlayer;

        private MarketTypesOfRequests _marketTypesOfRequests;
        private DateTime _deadlineDate;
        private int _deadLineDaysCount = 1;

        private void Start()
        {
            ToggleBlitz(false);
            buttonSave.interactable = false;
            
            _marketTypesOfRequests = ServiceLocator.GetService<MarketTypesOfRequests>();
            
            toggleBlitzPrice.onValueChanged.AddListener(ToggleBlitz);
            if(buttonOpenRoster) buttonOpenRoster.onClick.AddListener(OpenRoster);
            //buttonOpenDeadlinePlanner.onClick.AddListener(OpenDeadlinePlanner);
            dropdownDeadline.onValueChanged.AddListener(SetDeadlineDate);
            buttonSave.onClick.AddListener(Save);
            inputPrice.onValueChanged.AddListener(InputPrice);
            inputPriceBlitz.onValueChanged.AddListener(InputPriceBlitz);
        }

        public void ClearData()
        {
            inputPrice.text = "0";
            inputPriceBlitz.text = "0";
            toggleBlitzPrice.isOn = false;
            dropdownDeadline.value = 0;
            ClosePlayerCard();
        }
        
        public void UpdateInfo(MarketPlayerTransfer transfer)
        {
            var player = ServiceLocator.GetService<ManagerTeamData>()
                .GetTeamPlayer(transfer.player.playerId);
            
            if(player == null) { return; }

            OpenPlayerCard(player);

            inputPrice.text = transfer.startPrice.ToString();

            textDeadlineDate.text = DataUtils.GetDate(transfer.deadLineDate.Value);
            _deadLineDaysCount = transfer.deadLineDate.Value.Day - DateTime.Now.ToUniversalTime().Day;
            
            if (transfer.priceStep == null)
            {
                ToggleBlitz(false);
            }
            else
            {
                ToggleBlitz(true);
                inputPriceBlitz.text = transfer.priceStep.ToString();
            }
        }

        private void ToggleBlitz(bool state)
        {
            panelBlitz.SetActive(state);
            
            if (!state)
            {
                _priceBlitz = 0;
                inputPriceBlitz.text = "";
            }
        }

        private async void OpenRoster()
        {
            var player = await _marketItemPlayerPicker
                .GetTeamPlayer(StatusMarketPopup.NewPlayer);

            if (player == null) return;

            if (player.status == TeamPlayerStatus.Charged || player.status == TeamPlayerStatus.Healthy)
            {
                OpenPlayerCard(player);   
            }
            else
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Player must be healthy for being a member");
                
                OpenRoster();
            }
        }

        private async void OpenDeadlinePlanner()
        {
            _deadlineDate = await _deadlinePlanner.GetDeadlineDate();
            textDeadlineDate.text = DataUtils.GetDate(_deadlineDate);
            _deadLineDaysCount = _deadlineDate.Day - DateTime.Now.ToUniversalTime().Day;
            
            CheckAccess();
        }

        private void SetDeadlineDate(int value)
        {
            _deadLineDaysCount = value + 1;
            Debug.Log("_deadLineDaysCount: " + _deadLineDaysCount);
            CheckAccess();
        }

        private int _playerId;
        public void OpenPlayerCard(TeamPlayer player)
        {
            panelCardPlayer.SetActive(true);
            _slotPlayer.SetInfo(player);
            _playerId = player.playerId;

            CheckAccess();
        }

        private void ClosePlayerCard()
        {
            panelCardPlayer.SetActive(false);
            _playerId = 0;
            
            CheckAccess();
        }

        private int _price;
        private void InputPrice(string value)
        {
            _price = GetPrice(value);

            CheckAccess();
        }

        private void CheckAccess()
        {
            Debug.Log("Price: " + _price + " Id: " + _playerId + " Count: " 
                      + _deadLineDaysCount);
            
            buttonSave.interactable = _price > 0 && _playerId != 0 && 
                                      _deadLineDaysCount < 4 && _deadLineDaysCount > 0;
        }

        private int _priceBlitz = 0;
        private void InputPriceBlitz(string value)
        {
            _priceBlitz = GetPrice(value);
        }

        private int maxValue = 99999999;
        private int GetPrice(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0; 
            
            if (value.Length >= 10) return 0;
            
            var parsedValue = int.Parse(value);
                
            var inBalance = parsedValue < maxValue;
                
            if (inBalance) return parsedValue; 

            return 0;
        }

        private async void Save()
        {
            buttonSave.interactable = false;

            RequestPlayerForTransfer playerForTransfer = new();
            
            playerForTransfer.price = _price;
            playerForTransfer.deadLineDaysCount = _deadLineDaysCount;
            playerForTransfer.playerId = _playerId;
            
            if (_priceBlitz == 0)
            {
                playerForTransfer.blitzPrice = null;
            }
            else
            {
                playerForTransfer.blitzPrice = _priceBlitz;
            }
            
            var success = await _marketTypesOfRequests
                .POST_PlayerForTransfer(playerForTransfer);

            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Player transfer was saved");

                ServiceLocator.GetService<MarketPopupStatus>()
                    .StatusAction = StatusMarketPopup.None;
                
                ServiceLocator.GetService<MarketStatus>()
                    .StatusAction = StatusMarket.MyTransfers;
            }

            buttonSave.interactable = true;
        }
    }
}