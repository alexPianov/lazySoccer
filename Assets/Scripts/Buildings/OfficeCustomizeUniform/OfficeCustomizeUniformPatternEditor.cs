using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.OfficeUniform
{
    public class OfficeCustomizeUniformPatternEditor : MonoBehaviour
    {
        [SerializeField] private Transform uniformContainer;
        
        [Title("Current Pattern")]
        [SerializeField] private OfficeCustomizeUniformPattern currentPattern;
        private List<OfficeCustomizeUniformPatternSlot> patternSlotList;
        [SerializeField] private List<OfficeCustomizeUniformPatternColorSlider> colorSliders = new();

        public PatternPart CurrentPart { get; private set; }
        [HideInInspector] public UnityEvent UpdatePatternEvent;
        
        public enum UnitformColor
        {
            Red, Green, Blue
        }
        
        public enum PatternPart
        {
            TShirt, Elements
        }

        private GameObject lastObject;
        public void SetSlotsToEditor(OfficeCustomizeUniformPatternSlot currentSlot, 
            List<OfficeCustomizeUniformPatternSlot> slotList)
        {
            patternSlotList = slotList;

            if (lastObject) Destroy(lastObject);

            lastObject = Instantiate(currentSlot.PatternPrefab, uniformContainer);

            if (lastObject.TryGetComponent(out OfficeCustomizeUniformPattern patternDisplay))
            {
                currentPattern = patternDisplay;
            }
            else
            {
                Debug.LogError("Failed to find UniformPattern in pattern object");
            }

            UpdatePatternPart(PatternPart.TShirt);
        }

        public void UpdatePatternPart(PatternPart part)
        {
            CurrentPart = part;
            UpdateCurrentPatternSlider();
            
            UpdatePatternEvent.Invoke();
        }

        private void UpdateCurrentPatternSlider()
        {
            var color = currentPattern.GetColor(CurrentPart);
            Debug.Log("UpdateCurrentPatternSlider: " + CurrentPart);
            UpdateColorSlider(UnitformColor.Red, color.r);
            UpdateColorSlider(UnitformColor.Green, color.g);
            UpdateColorSlider(UnitformColor.Blue, color.b);
        }
        
        private void UpdateColorSlider(UnitformColor color, float value)
        {
            var slider = colorSliders.Find(colorSlider => colorSlider.currentColorSetup == color);
            
            if (slider == null) { Debug.LogError("Failed to find color slider: " + color); return; }
            
            slider.SetSliderValue(value);
        }

        public void SetRedValue(Slider slider)
        {
            var color = currentPattern.GetColor(CurrentPart);
            var newColor = new Color(slider.value, color.g, color.b);
            currentPattern.SetColor(newColor, CurrentPart);
            EditPatterns(newColor);
        }
        
        public void SetGreenValue(Slider slider)
        {
            var color = currentPattern.GetColor(CurrentPart);
            var newColor = new Color(color.r, slider.value, color.b);
            currentPattern.SetColor(newColor, CurrentPart);
            EditPatterns(newColor);
        }
        
        public void SetBlueValue(Slider slider)
        {
            var color = currentPattern.GetColor(CurrentPart);
            var newColor = new Color(color.r, color.g, slider.value);
            currentPattern.SetColor(newColor, CurrentPart);
            EditPatterns(newColor);
        }

        private void EditPatterns(Color newColor)
        {
            foreach (var pattern in patternSlotList)
            {
                pattern.CurrentPattern.SetColor(newColor, CurrentPart);
            }
        }
    }
}