using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using UnityEngine;
using TMPro;
using LazySoccer.Popup;
using LazySoccet.Building;
using UnityEngine.UI;
using LazySoccer.Status;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Infrastructure.VisualEffects;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using UnityEngine.Events;

namespace LazySoccer.Popup
{
    public class BuildingInformation : MonoBehaviour
    {
        private ManagerPlayerData _playerdata;
        
        [Title("House Open")]
        [SerializeField] private ObjectHouse house;
        
        [Title("Info Building")]
        [SerializeField] private TMP_Text m_NameBuilding;
        [SerializeField] private TMP_Text m_Level;
        [SerializeField] private TMP_Text m_CostMaintenance;

        [Title("Upgrate Buttons")]
        [SerializeField] private GameObject upgradeButtonsPanel;
        [SerializeField] private Button _upgrate;
        [SerializeField] private Button _downgrate;
        [SerializeField] private Button _cancelUpdate;

        [Title("Upgrade Bar")] 
        [SerializeField] private GameObject upgradeBarPanel;
        [SerializeField] private Slider upgradeSlider;
        [SerializeField] private Button buttonInstanceUpgrade;
        [SerializeField] private TMP_Text m_UpdateHours;
        
        [Title("NFT Specialist")]
        [SerializeField] private Image image;

        [Title("Map Building")] 
        [SerializeField] private MapBuildings mapBuildings;
        
        [Title("Close")]
        [SerializeField] private Button _close;

        private string _level = "Level:";
        private string _maintenance = "Daily maintenance:";

        [SerializeField] private BuildingWindowStatus _buildingWindowStatus;
        [SerializeField] private ManagerBuilding _building;
        
        private BuildingTypesOfRequests _buildingTypesOfRequests;
        private QuestionPopup _questionPopup;
        private ManagerBalanceUpdate _managerBalanceUpdate;
        private GeneralPopupMessage _generalPopupMessage;
        
        public UnityEvent updateLevel;
        
        private void Awake()
        {
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            _managerBalanceUpdate = ServiceLocator.GetService<ManagerBalanceUpdate>();
            _buildingTypesOfRequests = ServiceLocator.GetService<BuildingTypesOfRequests>();
            _buildingWindowStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            _building = ServiceLocator.GetService<ManagerBuilding>();
            _playerdata = ServiceLocator.GetService<ManagerPlayerData>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>(); 
        }

        public ObjectHouse GetHouseInfo()
        {
            return house;
        }
        
        public void Open()
        {
            house = _building.GetHouse(_building.OpenHouse);
            
            if (house != null) Init(house);
            else Debug.LogError("No house");
            
            UpgradeBar(house.IsUpgrading);
            CheckButtonsInteration();
        }

        private void UpgradeBar(bool state)
        {
            ActiveUpgradeBar(state);

            house.InUpgrade.Value = state;

            if (!state) return;

            if (house.DateOfCompletion.Value == null || house.DateOfStart.Value == null) return;
            
            if (house.DateOfCompletion.Value.Value != null || house.DateOfStart.Value.Value != null)
            {
                // var dateOfCompletion = house.DateOfCompletion.Value.Value.ToUniversalTime();
                // var dateOfStart = house.DateOfStart.Value.Value.ToUniversalTime();
                // var nowDate = DateTime.Now.ToUniversalTime();
                //
                // var timeSummary = dateOfCompletion - dateOfStart;
                // var timePast = nowDate - dateOfStart;
                // var timeRemain = dateOfCompletion - nowDate;
                
                var timeSummary = house.DateOfCompletion.Value.Value - house.DateOfStart.Value.Value;
                var timePast = DateTime.Now.ToUniversalTime() - house.DateOfStart.Value.Value;
                var timeRemain = house.DateOfCompletion.Value.Value - DateTime.Now.ToUniversalTime();

                upgradeSlider.maxValue = (float)timeSummary.TotalHours;
                upgradeSlider.value = (float)timePast.TotalHours;

                if (m_UpdateHours)
                {
                    //m_UpdateHours.text = $"Upgrading ({timeRemain.Hours} hours remain)";
                    m_UpdateHours.GetComponent<LocalizationParamsManager>().SetParameterValue("param", timeRemain.Hours.ToString());
                }

                if (timeRemain.Hours <= 0)
                {
                    house.InUpgrade.Value = false;
                    house.IsUpgrading = false;
                    ActiveUpgradeBar(false);
                }
            }
        }

        private void ActiveUpgradeBar(bool state)
        {
            upgradeBarPanel.SetActive(state);
            upgradeButtonsPanel.SetActive(!state);
        }

        public void Init(ObjectHouse house)
        {
            image.sprite = house.CurrentSprite;

            //m_NameBuilding.text = house.NameHouse;
            m_NameBuilding.GetComponent<Localize>().SetTerm("Building-" + house.TypeHouse);
            
            m_Level.GetComponent<Localize>().SetTerm("Building-Level");
            m_Level.GetComponent<LocalizationParamsManager>().SetParameterValue("param", house.Level.Value.ToString());
            
            //m_Level.text = $"{_level} {house.Level.Value}";
            
            if (house.Level.Value == 1) _downgrate.interactable = false;
            
            //m_CostMaintenance.text = $"{_maintenance} {house.MaintenanceCost.Value}";
            
            m_CostMaintenance.GetComponent<Localize>().SetTerm("Building-DailyMaintenance");
            m_CostMaintenance.GetComponent<LocalizationParamsManager>().SetParameterValue("param", house.MaintenanceCost.Value.ToString());
            
            SubscriptionAction(house);
            AllowUpgrade();
        }
        
        private void Start()
        {
            _upgrate.onClick.AddListener(ButtonUpgrade);
            _downgrate.onClick.AddListener(Downgrade);
            _cancelUpdate.onClick.AddListener(CancelUpgrade);
            if(_close) _close.onClick.AddListener(Close);
            if(buttonInstanceUpgrade) buttonInstanceUpgrade.onClick.AddListener(ButtonInstanceUpgrade);
        }

        private void ButtonUpgrade()
        {
            Upgrade();
        }

        private void ButtonInstanceUpgrade()
        {
            InstantUpgrade();
        }

        [Button("Full Upgrade")]
        public async void FullUpgrade(int level)
        {
            for (int i = 0; i < level; i++)
            {
                await Upgrade(true);
                await UniTask.Delay(100);
                await InstantUpgrade(true);
            }
        }
        
        private void SubscriptionAction(ObjectHouse house)
        {
            house.MaintenanceCost.onActionUser += CostMaintenanceUpdate;
            house.Level.onActionUser += LevelUpdate;
        }
        private void CostMaintenanceUpdate(int? value)
        {
            m_CostMaintenance.text = $"{_maintenance} {value}";
        }
        private void LevelUpdate(int value)
        {
            m_Level.text = $"{_level} {value}";
        }

        public void AllowUpgrade()
        {
            if(_playerdata.PlayerHUDs.Balance.Value < house.BuildingCost.Value)
                _upgrate.interactable = false;
            else
                _upgrate.interactable = true;
        }

        private async UniTask DecrementBuildingCost(BaseAction<int?> cost)
        {
            if (cost.Value > 0)
            {
                int serviceCost = (int)cost.Value;
                await _managerBalanceUpdate.UpdateBalance(serviceCost, false);
            }
        }

        private StatusBuilding currentBuildingStatus;
        private async UniTask Upgrade(bool skipQuestion = false)
        {
            if (!skipQuestion)
            {
                string descrition = "Pay COST LAZY and BUILDING will get upgrade";

                var result = await _questionPopup.OpenQuestion(descrition, "Upgrade building?", 
                    Param1: house.BuildingCost.Value.ToString(), Param2: house.NameHouse);
            
                if(!result) return;   
            }
            
            _upgrate.interactable = false;
            _buildingWindowStatus.OpenQuickLoading(true);

            if (house.IdBuild == null)
            {
                Debug.Log("Error! Id build is null");
                _buildingWindowStatus.OpenQuickLoading(false);
                _upgrate.interactable = true;
                CheckButtonsInteration();
                return;
            }
            
            Debug.Log("Start building upgrade: " + house.IdBuild.Value);
            
            var success = await ServiceLocator.GetService<BuildingTypesOfRequests>()
                .Post_BuildingUpgrade(house.IdBuild.Value);

            Debug.Log("Start building upgrade | Result: " + success);
            
            if (success)
            {
                await DecrementBuildingCost(house.BuildingCost);
                
                var buildingsIsUpgrade = await _buildingTypesOfRequests
                    .Get_AllBuildingRequest(false);
                
                Debug.Log("Start building upgrade | All buildings upgrade: " + buildingsIsUpgrade);
                
                UpdateMapBuildings();
                Open();
                
                _generalPopupMessage.ShowInfo($"HOUSE upgrade was started", Param1: house.NameHouse);
                VisualEffectsListener.EffectsListener.FireworkLong(3);
            }
            
            _buildingWindowStatus.OpenQuickLoading(false);
            _upgrate.interactable = true;
            CheckButtonsInteration();
            
        }

        private async void CancelUpgrade()
        {
            string descrition = "HOUSE upgrade will be canceled";
            
            var result = await _questionPopup.OpenQuestion(descrition, "Cancel upgrade?", Param1: house.NameHouse);
            
            if(!result) return;
            
            _cancelUpdate.interactable = false;
            _buildingWindowStatus.OpenQuickLoading(true);
            
            Debug.Log("Cancel upgrade building: " + house.IdBuild.Value);
            
            var success = await ServiceLocator.GetService<BuildingTypesOfRequests>()
                .Post_Downgrade(house.IdBuild.Value);

            if (success)
            {
                await _buildingTypesOfRequests.Get_AllBuildingRequest(false);
                UpdateMapBuildings();
                Open();
                
                _generalPopupMessage.ShowInfo($"HOUSE upgrade was canceled", Param1: house.NameHouse);
            } 
            
            _buildingWindowStatus.OpenQuickLoading(false);
            _cancelUpdate.interactable = true;
            CheckButtonsInteration();
        }

        private async UniTask InstantUpgrade(bool skipQuestion = false)
        {
            if (!skipQuestion)
            {
                string descrition = "Pay COST LAZY and BUILDING will get instant upgrade";
            
                var result = await _questionPopup.OpenQuestion(descrition, "Instant upgrade?", 
                    Param1: house.CostInstantBuilding.Value.ToString(), Param2: house.NameHouse);
            
                if (!result) return;
            }
            
            Debug.Log("Instant upgrade: " + house.IdBuild.Value);

            _upgrate.interactable = false;
            _buildingWindowStatus.OpenQuickLoading(true);

            var success = await ServiceLocator.GetService<BuildingTypesOfRequests>()
                .Post_ImmediateUpgrade(house.IdBuild.Value);
            
            Debug.Log("Instant upgrade | success: " + success);
            
            if (success)
            {
                await DecrementBuildingCost(house.CostInstantBuilding);

                var buildingsIsUpgrade = await _buildingTypesOfRequests
                    .Get_AllBuildingRequest(false);
                
                Debug.Log("Instant upgrade | All buildings upgrade: " + buildingsIsUpgrade);
                
                UpdateMapBuildings();
                Open();
                UpdateLevelEvent();
                
                _generalPopupMessage.ShowInfo($"HOUSE was upgraded", Param1:house.NameHouse);
                VisualEffectsListener.EffectsListener.FireworkLong(6);
            }

            _buildingWindowStatus.OpenQuickLoading(false);
            _upgrate.interactable = true;
            CheckButtonsInteration();
            
        }

        private bool disableDowngrade;
        public void DisableDowngrade(bool state)
        {
            disableDowngrade = state;
        }
        
        private async void Downgrade()
        {
            if (disableDowngrade)
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("To apply the downgrade, reduce the number of players in the building");
                return;
            }
            
            string descrition = "HOUSE will get downgrade";
            
            var result = await _questionPopup.OpenQuestion(descrition, "Downgrade building?",
                Param1: house.NameHouse);
            
            if(!result) return;
            
            Debug.Log("Downgrade Building: " + house.IdBuild.Value);
            
            _downgrate.interactable = false;
            _buildingWindowStatus.OpenQuickLoading(true);
            
            var success = await ServiceLocator.GetService<BuildingTypesOfRequests>()
                .Post_Downgrade(house.IdBuild.Value);

            if (success)
            {
                await _buildingTypesOfRequests.Get_AllBuildingRequest(false);
                UpdateMapBuildings();
                Open();
                UpdateLevelEvent();
                _generalPopupMessage.ShowInfo($"HOUSE was downgraded", Param1: house.NameHouse);
            }

            _buildingWindowStatus.OpenQuickLoading(false);
            CheckButtonsInteration();
        }

        private void CheckButtonsInteration()
        {
            _downgrate.interactable = house.Level.Value != 1;
            _upgrate.interactable = house.Level.Value < 20;
        }

        private void UpdateLevelEvent()
        {
            if(updateLevel != null) updateLevel.Invoke();
        }

        private void UpdateMapBuildings()
        {
            mapBuildings.Instance();
        }
        
        private void Close()
        {
            house = null;
            _buildingWindowStatus.SetAction(StatusBuilding.None);
        }

#if UNITY_EDITOR
        [Button]
        private void TestInit(TypeHouse house)
        {
            Init(_building.GetHouse(house));
        }
#endif
    }
}
