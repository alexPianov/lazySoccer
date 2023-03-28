using LazySoccer.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.OfficeUniform.OfficeCustomizeUniformPatternEditor;

public class OfficeCustomizeUniformPattern : MonoBehaviour
{
    [SerializeField] private Image patternTShirt;
    [SerializeField] private Image patternTShirtElement;
    [SerializeField] private Image patternEmblem;
    [SerializeField] private Image patternShorts;
    
    public void SetEmblemSprite(Sprite sprite)
    {
        patternEmblem.sprite = sprite;
    }

    public void SetColor(string hexColor, PatternPart part)
    {
        SetColor(DataUtils.GetColorFromHex(hexColor), part);
    }

    public Uniform CurrentUniform { get; private set; }
    public void SetUniform(Uniform uniform)
    {
        CurrentUniform = uniform;
    }

    public void SetColor(Color color, PatternPart part)
    {
        switch (part)
        {
            case PatternPart.Elements: 
                patternTShirtElement.color = color;
                patternShorts.color = color;
                if(CurrentUniform != null) CurrentUniform.emblemColor = color.ToHexString();
                break;
            case PatternPart.TShirt: 
                patternTShirt.color = color;
                if(CurrentUniform != null) CurrentUniform.uniformsColor = color.ToHexString();
                break;
        }
    }

    public Color GetColor(PatternPart part)
    {
        switch (part)
        {
            case PatternPart.Elements: return patternShorts.color;
            case PatternPart.TShirt: return patternTShirt.color;
            default: return new Color();
        }
    }
}