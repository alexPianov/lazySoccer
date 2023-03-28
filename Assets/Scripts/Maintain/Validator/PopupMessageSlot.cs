using System;
using Cysharp.Threading.Tasks;
using I2.Loc;
using Playstel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Validator
{
    public class PopupMessageSlot : UiTransparency
    {
        [Title("Text")]
        [SerializeField] private TMP_Text errorField;
        
        [Title("Sprite")]
        [SerializeField] private Sprite backgroundRed;
        [SerializeField] private Sprite backgroundGreen;
        [SerializeField] private Sprite iconFaceRed;
        [SerializeField] private Sprite iconFaceGreen;
        
        [Title("Image")]
        [SerializeField] private Image background;
        [SerializeField] private Image iconFace;

        private const int showingTimeMs = 4000;
        
        private async void Start()
        {
            Transparency(false);
            await UniTask.Delay(showingTimeMs);
            Transparency(true);
            await UniTask.Delay(1000);
            Destroy(gameObject);
        }
        
        public void SetInfo(string messageText, bool positive, string param)
        {
            errorField.GetComponent<Localize>().SetTerm("Msg-" + messageText);

            if (!string.IsNullOrEmpty(param))
            {
                errorField.GetComponent<LocalizationParamsManager>().SetParameterValue("param", param);
            }
            
            Color(positive);
        }
        
        
        public void Color(bool positive)
        {
            if (positive)
            {
                background.sprite = backgroundGreen;
                iconFace.sprite = iconFaceGreen;
            }
            else
            {
                background.sprite = backgroundRed;
                iconFace.sprite = iconFaceRed;
            }
        }
        
        
    }
}