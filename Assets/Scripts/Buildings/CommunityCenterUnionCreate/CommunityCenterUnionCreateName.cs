using System;
using LazySoccer.Network;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreateName : MonoBehaviour
    {
        [Title("Input")]
        [SerializeField] private TMP_InputField inputFieldName;
        
        [Title("Button")]
        [SerializeField] private Button buttonApply; 
        [SerializeField] private CommunityCenterUnionCreate unionCreate;

        public FindValidationContainer _valid;
        
        private void Awake()
        {
            _valid = GetComponent<FindValidationContainer>();
            
            inputFieldName.onValueChanged.AddListener(InputNewName);
            buttonApply.onClick.AddListener(ClickApply);
            
            buttonApply.interactable = false;
        }

        private void OnEnable()
        {
            ClearFields();
        }

        private void ClearFields()
        {
            inputFieldName.text = "";
            nameUnion = "";
            
            CheckForm();
        }

        private string nameUnion;
        private void InputNewName(string value)
        {
            nameUnion = value;
            CheckForm();
        }

        private void CheckForm()
        {
            if(ValidationUtils.StringNicknameValid(nameUnion))
            {
                ActiveButton(true);
            }
            else
            {
                ActiveButton(false);
            }
        }

        private void ActiveButton(bool state)
        {
            buttonApply.interactable = state;
        }

        private async void ClickApply()
        {
            Debug.Log("Click apply");
            
            if (!_valid.isRatification())
            {
                return;
            }
            
            ActiveButton(false);
            
            var result = await unionCreate.Apply(nameUnion);
            
            ActiveButton(true);
            
            if (!result)
            {
                inputFieldName.GetComponent<ValidationField>().ActiveErrorHighlight();
            }
        }
    }
}