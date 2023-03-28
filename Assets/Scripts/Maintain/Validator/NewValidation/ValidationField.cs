using System;
using UnityEngine;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.Events;

public class ValidationField : MonoBehaviour
{
    [SerializeField] private TMP_InputField textField;
    [SerializeField] private ErrorsStatus errorsStatus;
    [SerializeField] private Sprite errorHighlight;

    private Sprite defaultSprite;
    private GeneralValidation _generalValidation;
    private void Start()
    {
        _generalValidation = ServiceLocator.GetService<GeneralValidation>();
        
        textField = GetComponent<TMP_InputField>();
        textField.onSelect.AddListener(arg0 => OnSelect());
        textField.onDeselect.AddListener(arg0 => OnSelect());
        
        if(textField) defaultSprite = textField.image.sprite;
    }
    
    private void OnEnable()
    {
        if(GetComponent<TMP_InputField>()) defaultSprite 
            = GetComponent<TMP_InputField>().image.sprite;
    }
    
    private void OnDisable()
    {
        if(textField) textField.image.sprite = defaultSprite;
    }
    
    private void OnSelect()
    {
        if(textField) textField.image.sprite = defaultSprite;
    }

    private UnityAction textSelection;
    
    [Button]
    public bool isValidation()
    {
        var textError = _generalValidation.TextError(errorsStatus, textField.text);
        
        if (string.IsNullOrEmpty(textError))
        {
            return true;
        }

        if (textError.Length > 0)
        {
            ActiveErrorHighlight();
        }
        
        return textError.Length == 0;
    }

    public void ActiveErrorHighlight()
    {
        if(errorHighlight) textField.image.sprite = errorHighlight;
    }

    public void SetTextInput(ErrorsStatus status, string text)
    {
        if (textField == null)
        {
            textField = GetComponent<TMP_InputField>();
        }
        
        if(status == errorsStatus)
        {
            textField.text = text;
        }
    }

    public void ClearTextInput()
    {
        GetComponent<TMP_InputField>().text = "";
    }
}
