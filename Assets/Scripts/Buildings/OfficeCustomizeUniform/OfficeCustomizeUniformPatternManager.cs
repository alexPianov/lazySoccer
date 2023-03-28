using System.Collections.Generic;
using LazySoccer.SceneLoading;
using LazySoccer.SceneLoading.Buildings.OfficeCustomize;
using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using LazySoccer.SceneLoading.Buildings.OfficeUniform;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.OfficeCustomize.UniformPatterns;

public class OfficeCustomizeUniformPatternManager : MonoBehaviour
{
    [Title("Mode")]
    [SerializeField] private UniformType currentFormType;
    
    [Title("Data")]
    [SerializeField] private UniformPatterns uniformPatterns;

    [Title("Pattern List")] 
    private List<OfficeCustomizeUniformPatternSlot> patternSlots = new();

    public OfficeCustomizeUniformPatternSlot PickedSlot { get; private set; }

    [Title("Refs")]
    [SerializeField] private GameObject uniformSlotPrefab;
    [SerializeField] private Transform uniformContainer;
    [SerializeField] private OfficeCustomizeUniformPatternEditor patternEditor;
    [SerializeField] private Transform selectFrame;

    private ManagerTeamData _managerStatisticData;
    private ManagerSprites _managerSprites;
    
    private void Awake()
    {
        _managerStatisticData = ServiceLocator.GetService<ManagerTeamData>();
        _managerSprites = ServiceLocator.GetService<ManagerSprites>();
    }

    private string colorUnitform;
    private string colorElements;
    
    public void CreatePatterns()
    {
        if(patternSlots.Count > 0) return;

        CreateColors();
        
        var emblemSprite = GetEmblemSprite();
        var patterns = uniformPatterns.GetAllPatterns();

        foreach (var pattern in patterns)
        {
            CreatePatternSlots(pattern, emblemSprite);
        }

        if (PickedSlot == null && patternSlots[0])
        {
            PickSlot(patternSlots[0]);
        }
    }

    private void CreateColors()
    {
        colorUnitform = GenerateRandomColorForHueShiftShader();
        colorElements = GenerateRandomColorForHueShiftShader();
    }
    
    private void CreatePatternSlots(UniformPattern pattern, Sprite emblemSprite)
    {
        var patternPrefab = Instantiate(uniformSlotPrefab, uniformContainer);

        if (patternPrefab.TryGetComponent(out OfficeCustomizeUniformPatternSlot patternSlot))
        {
            var isPicked = GetUniform(pattern.uniformId, out Uniform uniform);

            patternSlot.SetPatternImageGroup
                (uniform, emblemSprite, pattern.patternPrefab);

            patternSlot.SetPatternManager(this);
            
            patternSlots.Add(patternSlot);

            if (isPicked)
            {
                PickSlot(patternSlot);
            } 
        }
    }

    public void PickSlot(OfficeCustomizeUniformPatternSlot slot)
    {
        PickedSlot = slot;
        
        DeselectAll();
        PickedSlot.SelectPattern(true);
        
        patternEditor.SetSlotsToEditor(PickedSlot, patternSlots);
        
        SelectFrameMove(PickedSlot.transform);
    }

    private void SelectFrameMove(Transform transform)
    {
        selectFrame.SetParent(transform);
        selectFrame.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void DeselectAll()
    {
        foreach (var slot in patternSlots)
        {
            slot.SelectPattern(false);
        }
    }

    private bool GetUniform(int currentId, out Uniform resultUniform)
    {
        resultUniform = CreateUniform(currentId);
        
        var statistic = _managerStatisticData.teamStatisticData;

        if (statistic == null || statistic.uniforms == null)
        {
            return false;
        }
        
        var typedUniforms = statistic.uniforms
            .FindAll(uniforms => uniforms.type == currentFormType);

        if (typedUniforms.Count == 0) return false; 
            
        resultUniform = typedUniforms[0];

        foreach (var uniform in typedUniforms)
        {
            if (uniform.uniformsId == currentId) return true;  
        }

        return false;
    }
    
    private Uniform CreateUniform(int uniformId)
    {
        return new Uniform
        {
            uniformsId = uniformId,
            type = currentFormType,
            
            emblemColor = colorElements,
            uniformsColor = colorUnitform
        };
    }

    private string GenerateRandomColorForHueShiftShader()
    {
        return new Color(
            Random.Range(0, 0.5f), 
            Random.Range(0.5f, 1f), 
            Random.Range(0.25f, 0.5f))
            .ToHexString();
    }

    private string GenerateRandomColor()
    {
        return Random.ColorHSV(0.5f, 0.5f, 1f, 1f, 0.5f, 1f).ToHexString();
    }

    private Sprite GetEmblemSprite()
    {
        var statisticData = ServiceLocator.GetService<ManagerTeamData>();

        if (statisticData == null) {Debug.LogError("StatisticData is null"); return null;}
        if (statisticData.teamStatisticData == null) {Debug.LogError("teamStatisticData is null"); return null;}
        if (statisticData.teamStatisticData.emblem == null) {Debug.LogError("emblem is null"); return null;}
        
        var emblemId = statisticData.teamStatisticData.emblem.emblemId;
        
        return _managerSprites.GetTeamSprite(emblemId);
    }
}
