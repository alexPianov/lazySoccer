using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.SceneLoading.Buildings.OfficeUniform.OfficeCustomizeUniformPatternEditor;

public class OfficeCustomizeUniformPatternColorSlider : MonoBehaviour
{
    public Slider colorSlider;
    public UnitformColor currentColorSetup;
    public void SetSliderValue(float value)
    {
        colorSlider.value = value;
    }
}
