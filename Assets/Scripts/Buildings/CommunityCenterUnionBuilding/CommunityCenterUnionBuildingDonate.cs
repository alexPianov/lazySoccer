using System;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Popup;
using LazySoccer.Status;
using LazySoccer.Windows;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionBuildingDonate : MonoBehaviour
    {
        [Title("Slider")]
        [SerializeField] private Slider sliderProgress;
        [SerializeField] private TMP_Text textSliderProgress;

        [Title("Button")]
        [SerializeField] private Button buttonDonationStart;
        [SerializeField] private Button buttonDonatePopup;
        [SerializeField] private Button buttonDonationCancel;
        [SerializeField] private Button buttonDonate;
        [SerializeField] private Button buttonBuildingUpgrade;
        
        [Title("Input")]
        [SerializeField] private GameObject panelInput;
        [SerializeField] private TMP_InputField inputDonation;
        
        [Title("Panel")] 
        [SerializeField] private GameObject panelDonationBlock;
        [SerializeField] private TMP_Text textDontaionBlock;

        [Title("Refs")] 
        [SerializeField] private CommunityCenterUnion Union;
        [SerializeField] private CommunityCenterUnionBuilding Building;

            private UnionBuilding _unionBuilding;
        
        private ManagerCommunityData _managerCommunityData;
        private ManagerPlayerData _managerPlayerData;
        private GeneralPopupMessage _generalPopupMessage;
        private QuestionPopup _questionPopup;
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;

        private void Awake()
        {
            _communityCenterTypesOfRequest = ServiceLocator.GetService<CommunityCenterTypesOfRequest>();
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
            _questionPopup = ServiceLocator.GetService<QuestionPopup>();

            inputDonation.onValueChanged.AddListener(arg0 => DonationSum());
        }

        public void SetUnionBuilding(UnionBuilding unionBuilding)
        {
            _unionBuilding = unionBuilding;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            UpdateLock();
            UpdateDonationSlider();
            SetDonateButtons();
        }
        
        private void UpdateLock()
        {
            panelDonationBlock.SetActive(!_unionBuilding.isDonationEnabled);
            textDontaionBlock.GetComponent<Localize>().SetTerm("3-CommCenterPopup-Building-Text-Closed");
        }

        private const int updateTime = 8;
        private void UpdateDonationSlider()
        {
            sliderProgress.gameObject.SetActive(_unionBuilding.isDonationEnabled);
            
            if (_unionBuilding.isDonationEnabled)
            {
                sliderProgress.maxValue = _unionBuilding.buildingLvL.nextLvL.buildingCost;
                sliderProgress.value = _unionBuilding.donations;
                
                textSliderProgress.GetComponent<Localize>().SetTerm("3-General-Slider-DontaionLeft");
                
                textSliderProgress.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param1", $"{_unionBuilding.donations}");
                
                textSliderProgress.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param2", $"{_unionBuilding.buildingLvL.nextLvL.buildingCost}");
            }

            Debug.Log("Update dateOfCompletion: " + _unionBuilding.dateOfCompletion);
            
            if (_unionBuilding.dateOfCompletion != null)
            {
                sliderProgress.gameObject.SetActive(true);

                var building = ServiceLocator.GetService<ManagerBuilding>().BuildingAll.Find(all =>
                    (int) all.buildingType == _unionBuilding.unionBuildingId);
 
                var dateOfCompletion = _unionBuilding.dateOfCompletion.Value.ToUniversalTime();
                var dateNow = DateTime.Now.ToUniversalTime();
                var hoursLeft = updateTime - (dateOfCompletion.Hour - dateNow.Hour);
 
                sliderProgress.maxValue = updateTime;
                sliderProgress.value = hoursLeft;
                
                textSliderProgress.GetComponent<Localize>().SetTerm("3-General-Slider-HoursLeft");
                
                textSliderProgress.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param1", hoursLeft.ToString());
                
                textSliderProgress.GetComponent<LocalizationParamsManager>()
                    .SetParameterValue("param2", updateTime.ToString());
                
                textDontaionBlock.GetComponent<Localize>().SetTerm("3-CommCenterPopup-Building-Text-Upgrading");
            }
        }

        private void SetDonateButtons()
        {
            if (Union.GetUserTeam() == null)
            {
                Debug.Log("No user team");
                Close(false);
                return;
            }
            
            var memberType = Union.GetUserTeam().type;

            if (memberType == MemberType.Master)
            {
                Debug.Log("Master | Donation enabled: " + _unionBuilding.isDonationEnabled);

                if (_unionBuilding.buildingLvL.nextLvL == null)
                {
                    Debug.Log("Next lvl is null");
                }

                buttonDonatePopup.interactable = true;
                
                if (_unionBuilding.isDonationEnabled && 
                    _unionBuilding.buildingLvL.nextLvL != null)
                {
                    var readyForUpgrade = _unionBuilding
                        .donations >= _unionBuilding.buildingLvL.nextLvL.buildingCost;
                    
                    buttonBuildingUpgrade.gameObject.SetActive(readyForUpgrade);
                    buttonDonatePopup.gameObject.SetActive(!readyForUpgrade);
                }
                else
                {
                    buttonBuildingUpgrade.gameObject.SetActive(false);
                    buttonDonatePopup.gameObject.SetActive(false);
                }

                buttonDonationCancel.gameObject.SetActive(_unionBuilding.isDonationEnabled);
                
                var building = _managerCommunityData.GetDonationBuilding();

                buttonDonationStart.interactable = building == null;
            }
            else
            {
                Close(true);
            }
        }

        private void Close(bool userInUnion)
        {
            buttonBuildingUpgrade.gameObject.SetActive(false);
            buttonDonatePopup.interactable = false;

            if (userInUnion)
            {
                buttonDonatePopup.interactable = _unionBuilding.isDonationEnabled;
            }
                
            buttonDonationCancel.gameObject.SetActive(false);
            buttonDonationStart.interactable = false;
        }

        public void OpenPopup()
        {
            panelInput.SetActive(true);
            UpdateDonationSum();
        }
        
        private void UpdateDonationSum()
        {
            inputDonation.text = "";
        }

        public void ClosePopup()
        {
            panelInput.SetActive(false);
        }

        private void DonationSum()
        {
            if (inputDonation.text != null && inputDonation.text != "" 
                                           && inputDonation.text.Length < 9)
            {
                _donationSum = int.Parse(inputDonation.text);
            }
            else
            {
                _donationSum = 0;
            }
            
            var balance = _managerPlayerData.PlayerHUDs.Balance.Value;

            var allowedDonationSum = _unionBuilding.buildingLvL
                .nextLvL.buildingCost - _unionBuilding.donations;

            var allow = _donationSum <= balance && _donationSum <= allowedDonationSum;

            if (_donationSum > allowedDonationSum)
            {
                _generalPopupMessage.ShowInfo("Donation amount exceeds the available value", false);
            }
            
            if (_donationSum > balance)
            {
                _generalPopupMessage.ShowInfo("Donation amount is greater than your balance", false);
            }
            
            buttonDonate.interactable = _donationSum > 0 && allow;
            
            
        }

        private int _donationSum;
        public async void Donate()
        {
            var result = await _questionPopup.OpenQuestion
            ($"Donate DONATION to HOUSE",
                "Initiate donations", Param1:_donationSum.ToString(), 
                Param2: TypeHouseGetter.GetHouseName(_unionBuilding.buildingLvL.building));
            
            if(!result) return;
            
            buttonDonate.interactable = false;
            
            var success = await _communityCenterTypesOfRequest
                .POST_UnionBuildingDonate(_unionBuilding.unionBuildingId, _donationSum);

            if (success)
            {
                await UpdateAll();
                ClosePopup();
                
                _generalPopupMessage.ShowInfo
                ($"HOUSE received your donation", Param1: TypeHouseGetter.GetHouseName(_unionBuilding.buildingLvL.building));
            }
            
            buttonDonate.interactable = true;
        }
        
        public async void CancelDonation()
        {
            var result = await _questionPopup.OpenQuestion
            ($"All progress will be lost. Are you sure you want to cancel donation?", 
                "Cancel donation");
            
            if(!result) return;
            
            buttonDonationCancel.interactable = false;
            
            var success = await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .POST_UnionBuildingDonationAbort(_unionBuilding.unionBuildingId);
            
            if(success)
            {
                await UpdateAll();
                
                _generalPopupMessage.ShowInfo
                ($"Donation for HOUSE was canceled", Param1: TypeHouseGetter.GetHouseName(_unionBuilding.buildingLvL.building));
            }
            
            buttonDonationCancel.interactable = true;
        }

        private async UniTask UpdateAll()
        {
            //await _communityCenterTypesOfRequest.GET_UserUnionProfile();
            await Union.RefreshUnion(StatusCommunityCenterUnion.Buildings);
            //await ServiceLocator.GetService<UserTypesOfRequests>().GetUserRequest();
            Building.UpdatePopupInfo();
        }
        
        public async void StartDonation()
        {
            buttonDonationStart.interactable = false;
            
            var success = await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .POST_UnionBuildingDonationStart(_unionBuilding.unionBuildingId);
            
            if(success)
            {
                await UpdateAll();
                
                _generalPopupMessage.ShowInfo
                ($"HOUSE is open for donation", Param1: TypeHouseGetter.GetHouseName(_unionBuilding.buildingLvL.building));
            }
            
            buttonDonationStart.interactable = true;
        }
        
        public async void UpgradeBuilding()
        {
            var result = await _questionPopup.OpenQuestion
            ($"Union building upgrade", "Confirm building upgrade?");
            
            if(!result) return;
            
            buttonBuildingUpgrade.interactable = false;
            
            var success = await ServiceLocator.GetService<CommunityCenterTypesOfRequest>()
                .POST_UnionBuildingUpdate(_unionBuilding.unionBuildingId);
            
            if(success)
            {
                _generalPopupMessage.ShowInfo
                ($"Upgrade for HOUSE was started", Param1: TypeHouseGetter.GetHouseName(_unionBuilding.buildingLvL.building));

                await UpdateAll();
            }
            
            buttonBuildingUpgrade.interactable = true;
        }
    }
}