using System;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.OfficeUniform.OfficeCustomizeUniformPatternEditor;

namespace LazySoccer.SceneLoading.Buildings.OfficeUniform
{
    [RequireComponent(typeof(Button))]
    public class OfficeCustomizeUniformPatternSlot : MonoBehaviour
    {
        public Transform patternContainer;
        public Image patternSelect;
        public GameObject PatternPrefab { get; private set; }
        public OfficeCustomizeUniformPattern CurrentPattern { get; private set; }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(PickSlot);
        }

        public void SetPatternImageGroup(Uniform uniform, Sprite emblem, GameObject patternImageGroup)
        {
            PatternPrefab = Instantiate(patternImageGroup, patternContainer);

            if (PatternPrefab.TryGetComponent(out OfficeCustomizeUniformPattern pattern))
            {
                CurrentPattern = pattern;
                
                pattern.SetUniform(uniform);
                pattern.SetEmblemSprite(emblem);
                pattern.SetColor(uniform.uniformsColor, PatternPart.TShirt);
                pattern.SetColor(uniform.emblemColor, PatternPart.Elements);
            }
        }

        private OfficeCustomizeUniformPatternManager _patternManager;
        public void SetPatternManager(OfficeCustomizeUniformPatternManager manager)
        {
            _patternManager = manager;
        }

        public void PickSlot()
        {
            _patternManager.PickSlot(this);
        }

        public void SelectPattern(bool state)
        {
            if(patternSelect) patternSelect.enabled = state;
        }
    }
}