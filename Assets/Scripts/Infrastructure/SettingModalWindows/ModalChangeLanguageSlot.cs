using System;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalChangeLanguageSlot : MonoBehaviour
    {
        public ManagerLocalization.Language Language;
        
        [Title("Image")]
        public Image imageBackground;
        public Image imageDot;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SwitchLanguage);
        }

        private ModalChangeLanguage _master;
        public void SetLocalizationManager(ModalChangeLanguage master)
        {
            _master = master;
        }

        public void SwitchLanguage()
        {
            _master.SetLanguage(Language);
            _master.UpdateLanguageList();
        }

        public void Select(bool state)
        {
            imageBackground.enabled = state;
            imageDot.enabled = state;
        }
    }
}