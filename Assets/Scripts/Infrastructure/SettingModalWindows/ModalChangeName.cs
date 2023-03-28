using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalChangeName : BaseModalWindows
{
    [Title("Input")]
    [SerializeField] private TMP_InputField inputFieldName;
    [SerializeField] private TMP_Text textCurrentNickname;
    [SerializeField] private TMP_Text textNicknameTip;
    [SerializeField] private TMP_Text textButtonText;
    [SerializeField] private Button buttonChange;
    [SerializeField] private FindValidationContainer _valid;

    private ManagerPlayerData _managerPlayerData;
    private ManagerEconomy _managerEconomy;
    private ManagerBalanceUpdate _managerBalanceUpdate;
    protected override void Start()
    {
        _valid = GetComponent<FindValidationContainer>();

        _managerBalanceUpdate = ServiceLocator.GetService<ManagerBalanceUpdate>();
        _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
        _managerEconomy = ServiceLocator.GetService<ManagerEconomy>();
        
        base.Start();
        
        inputFieldName.onValueChanged.AddListener(InputNewName);
        buttonChange.interactable = false;
    }

    public void UpdatePanelInfo()
    {
        var currentNickname = _managerPlayerData.PlayerHUDs.Name.Value;

        textCurrentNickname.text = $"({currentNickname})";

        if (_managerPlayerData.PlayerData.FreeNameChange)
        {
            textNicknameTip.GetComponent<Localize>().SetTerm("3-ChangeNickname-Text-Free");
            textButtonText.GetComponent<Localize>().SetTerm("3-ChangeNickname-Button-ChangeFree");
            buttonChange.interactable = true;
            return;
        }
        
        textNicknameTip.GetComponent<Localize>().SetTerm("3-ChangeNickname-Text-ForFee");
        
        var changeNicknamePrice = _managerEconomy.GetEconomy().changeNickname;
        
        textButtonText.GetComponent<Localize>().SetTerm("3-ChangeNickname-Button-Change");
        textButtonText.GetComponent<LocalizationParamsManager>()
            .SetParameterValue("param", changeNicknamePrice.ToString());
        
        var currentBalance = _managerPlayerData.PlayerHUDs.Balance.Value;

        buttonChange.interactable = currentBalance >= changeNicknamePrice;
    }
    
    public void ClearFields()
    {
        inputFieldName.text = "";
        
        base.ClearFields();
        
        CheckForm();
    }

    private string newName;
    private void InputNewName(string value)
    {
        newName = value;
        CheckForm();
    }

    private void CheckForm()
    {
        if(ValidationUtils.StringNicknameValid(newName))
        {
            ActiveSaveButton(true);
        }
        else
        {
            ActiveSaveButton(false);
        }
    }
    protected async override void ClickSave()
    {
        if(!_valid.isRatification()) return;

        if (newName == _managerPlayerData.PlayerHUDs.Name.Value)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("The new nickname must not match the old one", false);
            
            inputFieldName.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }

        var result = await ServiceLocator.GetService<ChangeUserTypesOfRequests>()
            .ChangeName(newName);

        if (result)
        {
            _managerPlayerData.PlayerHUDs.Name.Value = newName;
            _managerPlayerData.PlayerData.UserName = newName;
            
            if (_managerPlayerData.PlayerData.FreeNameChange)
            {
                _managerPlayerData.PlayerData.FreeNameChange = false;
            }
            else
            {
                await _managerBalanceUpdate.UpdateBalance(_managerEconomy.GetEconomy().changeNickname);
            }
            
            ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Name is changed!");
            ServiceLocator.GetService<ModalWindowStatus>().SetAction(StatusModalWindows.None);
        }
        else
        {
            inputFieldName.GetComponent<ValidationField>().ActiveErrorHighlight();
        }
    }
}
