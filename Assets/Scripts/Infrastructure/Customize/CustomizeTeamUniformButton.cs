using System;
using LazySoccer.SceneLoading.Buildings.OfficeUniform;
using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamUniformButton : MonoBehaviour
    {
        public Image selectLine;
        public TMP_Text text;
        public OfficeCustomizeUniformPatternEditor.PatternPart currentPart;
        public OfficeCustomizeUniformPatternEditor PatternEditor;

        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangePattern);
            PatternEditor.UpdatePatternEvent.AddListener(ChangeStatus);
            ChangeStatus();
        }

        public void ChangePattern()
        {
            PatternEditor.UpdatePatternPart(currentPart);
        }

        protected string ActiveColor;
        protected string NeutralColor;
        public void ChangeStatus()
        {
            Select(PatternEditor.CurrentPart == currentPart);
        }

        private void Select(bool state)
        {
            selectLine.enabled = state;
            
            if (state) text.color = DataUtils.GetColorFromHex(ActiveColor);
            else { text.color = DataUtils.GetColorFromHex(NeutralColor); }
        }
    }
}