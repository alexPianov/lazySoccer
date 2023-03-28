using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.Network.MarketTypesOfRequests;
using static LazySoccer.SceneLoading.Buildings.TransfersFilterSettings;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransfersFilter : MonoBehaviour
    {
        [Title("Setting")]
        [SerializeField] private TransfersFilterSettings settingsDefault;
        [SerializeField] private TransfersFilterSettings settingsCustom;
        public TransfersFilterSettings settingsTemporary;

        [Title("Slider Power")] 
        [SerializeField] private Slider sliderPowerMax;
        [SerializeField] private Slider sliderPowerMin;
        [SerializeField] private TMP_Text textPowerMaxValue;
        [SerializeField] private TMP_Text textPowerMinValue;
        private const int minPowerInterval = 100;
        
        [Title("Slider Price")] 
        [SerializeField] private Slider sliderPriceMax;
        [SerializeField] private Slider sliderPriceMin;
        [SerializeField] private TMP_Text textPriceMaxValue;
        [SerializeField] private TMP_Text textPriceMinValue;
        private const int minPriceInterval = 20000;

        [Title("Range")] 
        [SerializeField] private TMP_Dropdown dropdownRange;

        [Title("Position")] 
        [SerializeField] private TMP_Dropdown dropdownPosition;
        
        [Title("Trait")] 
        [SerializeField] private TMP_Dropdown dropdownTrait_1;
        [SerializeField] private TMP_Dropdown dropdownTrait_2;
        [SerializeField] private TMP_Dropdown dropdownTrait_3;
        [SerializeField] private TMP_Dropdown dropdownTrait_4;
        private List<TMP_Dropdown> dropdownList = new();
        
        [Title("Buttons")]
        [SerializeField] private Button buttonSave;
        [SerializeField] private Button buttonReset;

        [Title("Data")]
        public PlayerTraits playerTraits;
        
        private void Awake()
        {
            CreateTemporarySettings();

            buttonSave.onClick.AddListener(Save);
            buttonReset.onClick.AddListener(Reset);
            
            sliderPowerMax.onValueChanged.AddListener(SetMaxPower);
            sliderPowerMin.onValueChanged.AddListener(SetMinPower);
            
            sliderPriceMax.onValueChanged.AddListener(SetMaxPrice);
            sliderPriceMin.onValueChanged.AddListener(SetMinPrice);

            dropdownRange.onValueChanged.AddListener(SetRange);
            dropdownPosition.onValueChanged.AddListener(SetPositionId);

            Traits();
            
            dropdownList.Add(dropdownTrait_1);
            dropdownList.Add(dropdownTrait_2);
            dropdownList.Add(dropdownTrait_3);
            dropdownList.Add(dropdownTrait_4);
            
            LoadSettings(settingsCustom);
        }

        private void Traits()
        {
            SetupDropdown(dropdownTrait_1);
            SetupDropdown(dropdownTrait_2);
            SetupDropdown(dropdownTrait_3);
            SetupDropdown(dropdownTrait_4);

            dropdownTrait_1.onValueChanged.AddListener(SetTrait1);
            dropdownTrait_2.onValueChanged.AddListener(SetTrait2);
            dropdownTrait_3.onValueChanged.AddListener(SetTrait3);
            dropdownTrait_4.onValueChanged.AddListener(SetTrait4);
        }

        private void SetupDropdown(TMP_Dropdown dropdown)
        {
            dropdown.options.Clear();
        
            var firstData = new TMP_Dropdown.OptionData();
            firstData.text = LocalizationManager.GetTranslation("3-MarketPopup-TransferFilter-Dropdown-Traits-None");
            dropdown.options.Add(firstData);
            
            for (int i = 0; i < playerTraits.traits.Count; i++)
            {
                var trait = playerTraits.traits[i];
                
                var data = new TMP_Dropdown.OptionData();

                var traitName = LocalizationManager.GetTranslation("TeamPlayer-Trait-" + trait.traitName);
                
                if (trait.traitIsNegative)
                {
                    data.text = "<color=#8B0000>" + traitName + "</color>";
                }
                else
                {
                    data.text = "<color=#006400>" + traitName + "</color>";
                }
                
                dropdown.options.Add(data);
            }
        }

        private void CreateTemporarySettings()
        {
            settingsTemporary = ScriptableObject.CreateInstance<TransfersFilterSettings>();
        }

        private void Save()
        {
            SaveSettings(settingsTemporary);
            Back(); 
        }

        private void SaveSettings(TransfersFilterSettings temporarySettings)
        {
            settingsCustom.sliderPowerMax = temporarySettings.sliderPowerMax;
            settingsCustom.sliderPowerMin = temporarySettings.sliderPowerMin;
            
            settingsCustom.sliderPriceMax = temporarySettings.sliderPriceMax;
            settingsCustom.sliderPriceMin = temporarySettings.sliderPriceMin;

            settingsCustom.positionId = temporarySettings.positionId;
            settingsCustom.range = temporarySettings.range;

            settingsCustom.trait_1 = temporarySettings.trait_1;
            settingsCustom.trait_2 = temporarySettings.trait_2;
            settingsCustom.trait_3 = temporarySettings.trait_3;
            settingsCustom.trait_4 = temporarySettings.trait_4;
        }

        public RequestTransfersAll GetTransferSettings()
        {
            RequestTransfersAll requestTransfersAll = new();
            
            requestTransfersAll.power = new ();
            requestTransfersAll.power.end = settingsCustom.sliderPowerMax;
            requestTransfersAll.power.start = settingsCustom.sliderPowerMin;
            Debug.Log("settingsCustom.sliderPowerMax: " + settingsCustom.sliderPowerMax);
            Debug.Log("settingsCustom.sliderPowerMin: " + settingsCustom.sliderPowerMin);
            
            requestTransfersAll.price = new ();
            requestTransfersAll.price.end = settingsCustom.sliderPriceMax;
            requestTransfersAll.price.start = settingsCustom.sliderPriceMin;
            Debug.Log("settingsCustom.sliderPriceMax: " + settingsCustom.sliderPriceMax);
            Debug.Log("settingsCustom.sliderPowerMin: " + settingsCustom.sliderPriceMin);

            var traits = settingsCustom.GetTraits();
            
            if (traits.Count > 0)
            {
                requestTransfersAll.traits = traits;
            }

            if (settingsCustom.positionId != 0)
            {
                requestTransfersAll.positionId = settingsCustom.positionId;
            }
            
            requestTransfersAll.skipCount = 0;
            requestTransfersAll.takeCount = 200;

            requestTransfersAll.order = settingsCustom.range;

            return requestTransfersAll;
        }

        public async UniTask<List<MarketPlayerTransfer>> GetTransfers()
        {
            var transfers = await ServiceLocator.GetService<MarketTypesOfRequests>()
                .POST_TransfersAll(GetTransferSettings());

            return transfers;
        }

        private static void Back()
        {
            ServiceLocator.GetService<MarketPopupStatus>()
                .SetAction(StatusMarketPopup.None);
            
            ServiceLocator.GetService<BuildingWindowStatus>()
                .SetAction(StatusBuilding.MarketMain);

            ServiceLocator.GetService<MarketStatus>()
                .SetAction(StatusMarket.Transfers);
        }

        private void ActiveButtons(bool state)
        {
            buttonSave.interactable = state;
            buttonReset.interactable = state;
        }

        private void Reset()
        {
            LoadSettings(settingsDefault);
        }

        private void LoadSettings(TransfersFilterSettings settings)
        {
            if (settings == null) settings = settingsDefault;
            
            sliderPowerMax.maxValue = settingsDefault.sliderPowerBounds;
            sliderPowerMin.maxValue = settingsDefault.sliderPowerBounds;
                
            sliderPowerMax.value = settings.sliderPowerMax;
            sliderPowerMin.value = settings.sliderPowerMin;
            
            sliderPriceMax.maxValue = settingsDefault.sliderPriceBounds;
            sliderPriceMin.maxValue = settingsDefault.sliderPriceBounds;
                
            sliderPriceMax.value = settings.sliderPriceMax;
            sliderPriceMin.value = settings.sliderPriceMin;

            dropdownPosition.value = settings.positionId;
            dropdownRange.value = (int)settings.range;

            dropdownTrait_1.value = settings.trait_1; 
            dropdownTrait_2.value = settings.trait_2; 
            dropdownTrait_3.value = settings.trait_3; 
            dropdownTrait_4.value = settings.trait_4;
        }

        private void SetMaxPrice(float value)
        {
            if (CheckPriceMaxValueInterval()) return;

            var intValue = (int)value;
            settingsTemporary.sliderPriceMax = intValue;
            textPriceMaxValue.text = intValue.ToString();
        }
        
        private void SetMaxPower(float value)
        {
            if (CheckPowerMaxValueInterval()) return;

            var intValue = (int)value;
            settingsTemporary.sliderPowerMax = intValue;
            textPowerMaxValue.text = intValue.ToString();
        }

        private bool CheckPriceMaxValueInterval()
        {
            if (sliderPriceMin.value + minPriceInterval >= sliderPriceMax.value)
            {
                var borderValue = (int) sliderPriceMin.value + minPriceInterval;
                sliderPriceMax.value = borderValue;
                settingsTemporary.sliderPriceMax = borderValue;
                textPriceMaxValue.text = borderValue.ToString();
                return true;
            }

            return false;
        }
        
        private bool CheckPowerMaxValueInterval()
        {
            if (sliderPowerMin.value + minPowerInterval >= sliderPowerMax.value)
            {
                var borderValue = (int) sliderPowerMin.value + minPowerInterval;
                sliderPowerMax.value = borderValue;
                settingsTemporary.sliderPowerMax = borderValue;
                textPowerMaxValue.text = borderValue.ToString();
                return true;
            }

            return false;
        }

        private void SetMinPrice(float value)
        {
            if (CheckMinPriceValueInterval()) return;

            var intValue = (int) value;
            settingsTemporary.sliderPriceMin = intValue;
            textPriceMinValue.text = intValue.ToString();
        }

        private bool CheckMinPriceValueInterval()
        {
            if (sliderPriceMax.value - minPriceInterval <= sliderPriceMin.value)
            {
                var borderValue = (int)sliderPriceMax.value - minPriceInterval;
                sliderPriceMin.value = borderValue;
                settingsTemporary.sliderPriceMin = borderValue;
                textPriceMinValue.text = borderValue.ToString();
                return true;
            }

            return false;
        }
        
        private void SetMinPower(float value)
        {
            if (CheckMinPowerValueInterval()) return;

            var intValue = (int) value;
            settingsTemporary.sliderPowerMin = intValue;
            textPowerMinValue.text = intValue.ToString();
        }

        private void SetTrait1(int value)
        {
            settingsTemporary.trait_1 = value;
            
            CheckTraitDropdowns(dropdownTrait_1, value);
        }

        private void SetTrait2(int value)
        {
            settingsTemporary.trait_2 = value;
            
            CheckTraitDropdowns(dropdownTrait_2, value);
        }

        private void SetTrait3(int value)
        {
            settingsTemporary.trait_3 = value;
            
            CheckTraitDropdowns(dropdownTrait_3, value);
        }

        private void SetTrait4(int value)
        {
            settingsTemporary.trait_4 = value;
            
            CheckTraitDropdowns(dropdownTrait_4, value);
        }

        private void CheckTraitDropdowns(TMP_Dropdown tmp_dropdown, int value)
        {
            foreach (var dropdown in dropdownList)
            {
                if (dropdown.value == value && dropdown != tmp_dropdown)
                {
                    dropdown.value = 0;
                }
            }
        }

        private void SetPositionId(int value)
        {
            settingsTemporary.positionId = value;
        }

        private void SetRange(int value)
        {
            settingsTemporary.range = (TransferOrder) value;
        }

        private bool CheckMinPowerValueInterval()
        {
            if (sliderPowerMax.value - minPowerInterval <= sliderPowerMin.value)
            {
                var borderValue = (int)sliderPowerMax.value - minPowerInterval;
                sliderPowerMin.value = borderValue;
                settingsTemporary.sliderPowerMin = borderValue;
                textPowerMinValue.text = borderValue.ToString();
                return true;
            }

            return false;
        }
    }
}