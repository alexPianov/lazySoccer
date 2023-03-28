using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using LazySoccer.Windows;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionsFilter : MonoBehaviour
    {
        [Title("Setting")]
        [SerializeField] private UnionFilterSettings settingsDefault;
        [SerializeField] private UnionFilterSettings settingsCustom;
        private UnionFilterSettings settingsTemporary;

        [Title("Input")]
        [SerializeField] private TMP_InputField inputSearchWord;

        [Title("Sliders")] 
        [SerializeField] private Slider sliderMaxRating;
        [SerializeField] private Slider sliderMinRating;
        private const int minInterval = 15;
        
        [Title("Sliders Text")]
        [SerializeField] private TMP_Text textMaxRating;
        [SerializeField] private TMP_Text textMinRating;
         
        [Title("Toggles")] 
        [SerializeField] private Toggle toggleHideClosed;
        [SerializeField] private Toggle toggleHideFull;
        
        [Title("Buttons")]
        [SerializeField] private Button buttonSave;
        [SerializeField] private Button buttonReset;

        private void Awake()
        {
            CreateTemporarySettings();
            
            inputSearchWord.onValueChanged.AddListener(SetSearchWord);

            sliderMaxRating.onValueChanged.AddListener(SetMaxRating);
            sliderMinRating.onValueChanged.AddListener(SetMinRating);

            toggleHideClosed.onValueChanged.AddListener(SetToggleHideClosed);
            toggleHideClosed.onValueChanged.AddListener(SetToggleHideFull);
            
            buttonSave.onClick.AddListener(Save);
            buttonReset.onClick.AddListener(Reset);
            
            LoadSettings(settingsCustom);
        }

        private void CreateTemporarySettings()
        {
            settingsTemporary = ScriptableObject.CreateInstance<UnionFilterSettings>();
        }

        private async void Save()
        {
            ActiveButtons(false);

            SaveSettings(settingsTemporary, settingsCustom);
            
            await UpdateUserUnion();
            
            Back(); 
            
            ActiveButtons(true);
        }

        private void SaveSettings(UnionFilterSettings temporarySettings, UnionFilterSettings customSettings)
        {
            Debug.Log("EditSettings | searchString: " + temporarySettings.searchString);
            customSettings.searchString = temporarySettings.searchString;
            
            customSettings.sliderRatingMax = temporarySettings.sliderRatingMax;
            customSettings.sliderRatingMin = temporarySettings.sliderRatingMin;
            
            customSettings.toggleHideClosed = temporarySettings.toggleHideClosed;
            customSettings.toggleHideFull = temporarySettings.toggleHideFull;
        }

        private async UniTask UpdateUserUnion()
        {
            await ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>()
                .GET_Unions(true, false);
        }

        private static void Back()
        {
            ServiceLocator.GetService<CommunityCenterPopupStatus>()
                .SetAction(StatusCommunityCenterPopup.None);
            
            ServiceLocator.GetService<BuildingWindowStatus>()
                .SetAction(StatusBuilding.CommunityCenter);

            ServiceLocator.GetService<CommunityCenterStatus>()
                .SetAction(StatusCommunityCenter.Unions);
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

        private void LoadSettings(UnionFilterSettings settings)
        {
            if (settings == null) settings = settingsDefault;

            inputSearchWord.text = settings.searchString;

            sliderMaxRating.maxValue = settingsDefault.sliderBounds;
            sliderMinRating.maxValue = settingsDefault.sliderBounds;
                
            sliderMaxRating.value = settings.sliderRatingMax;
            sliderMinRating.value = settings.sliderRatingMin;

            textMaxRating.text = settings.sliderRatingMax.ToString();
            textMinRating.text = settings.sliderRatingMin.ToString();

            toggleHideClosed.isOn = settings.toggleHideClosed;
            toggleHideFull.isOn = settings.toggleHideFull;
        }

        private void SetSearchWord(string value)
        {
            settingsTemporary.searchString = value;
        }

        private void SetMaxRating(float value)
        {
            if (CheckMaxValueInterval()) return;

            var intValue = (int)value;
            settingsTemporary.sliderRatingMax = intValue;
            textMaxRating.text = intValue.ToString();
        }

        private bool CheckMaxValueInterval()
        {
            if (sliderMinRating.value + minInterval >= sliderMaxRating.value)
            {
                var borderValue = (int) sliderMinRating.value + minInterval;
                sliderMaxRating.value = borderValue;
                settingsTemporary.sliderRatingMax = borderValue;
                textMaxRating.text = borderValue.ToString();
                return true;
            }

            return false;
        }

        private void SetMinRating(float value)
        {
            if (CheckMinValueInterval()) return;

            var intValue = (int) value;
            settingsTemporary.sliderRatingMin = intValue;
            textMinRating.text = intValue.ToString();
        }

        private bool CheckMinValueInterval()
        {
            if (sliderMaxRating.value - minInterval <= sliderMinRating.value)
            {
                var borderValue = (int)sliderMaxRating.value - minInterval;
                sliderMinRating.value = borderValue;
                settingsTemporary.sliderRatingMin = borderValue;
                textMinRating.text = borderValue.ToString();
                return true;
            }

            return false;
        }

        private void SetToggleHideClosed(bool state)
        {
            settingsTemporary.toggleHideClosed = state;
        }

        private void SetToggleHideFull(bool state)
        {
            settingsTemporary.toggleHideFull = state;
        }
    }
}