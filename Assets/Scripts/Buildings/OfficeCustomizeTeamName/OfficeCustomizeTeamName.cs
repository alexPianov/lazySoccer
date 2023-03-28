using Cysharp.Threading.Tasks;
using I2.Loc;
using LazySoccer.Network;
using LazySoccer.SceneLoading.Buildings;
using LazySoccer.SceneLoading.Infrastructure.Customize;
using LazySoccer.Status;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeCustomizeTeamName : CustomizeTeamName
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_InputField inputNewName;
    [SerializeField] private TMP_InputField inputShortName;
    [SerializeField] private Button button;
    private Economy _economy;
    private ManagerBalanceUpdate _managerBalanceUpdate;
    private OfficeCustomizeStatus _officeCustomizeStatus;
    private GeneralPopupMessage _generalPopupMessage;
    private FindValidationContainer _valid;

    private void Start()
    {
        base.Start();

        _generalPopupMessage = ServiceLocator.GetService<GeneralPopupMessage>();
        _managerBalanceUpdate = ServiceLocator.GetService<ManagerBalanceUpdate>();
        _officeCustomizeStatus = ServiceLocator.GetService<OfficeCustomizeStatus>();
        _economy = ServiceLocator.GetService<ManagerEconomy>().GetEconomy();
        
        _valid = GetComponent<FindValidationContainer>(); 
        
        button.onClick.AddListener(Save);
        button.interactable = false;
        
        inputNewName.onValueChanged.AddListener(InputName);
        inputShortName.onValueChanged.AddListener(InputShortName);
    }

    private int balance;
    public void CheckBalance()
    {
        balance = managerPlayerData.PlayerHUDs.Balance.Value;

        // description.text = string.Format
        //     ("For changing your current team name, you will be charged <b>{0} LAZY</b>", 
        //         _economy.changeTeamName);

        description.GetComponent<LocalizationParamsManager>()
            .SetParameterValue("param", _economy.changeTeamName.ToString());
        
        CheckSaveButton();
    }
    
    public void InputName(string field)
    {
        newName = field;
        CheckSaveButton();
    }
    
    public void InputShortName(string field)
    {
        newShortName = field.ToUpper();
        inputShortName.text = newShortName;
        
        CheckSaveButton();
    }
    
    private void CheckSaveButton()
    {
        button.interactable = ValidationUtils.StringTeamNameValid(newName) &&
                              ValidationUtils.StringTeamNameShortValid(newShortName) &&
                              balance > _economy.changeTeamName;
    }
    
    public async void Save()
    {
        if (!_valid.isRatification()) return;
        
        if (ValidationUtils.OffensiveShortcuts(newShortName))
        {
            _generalPopupMessage.ShowInfo("Short name contains the offensive word");
            inputShortName.GetComponent<ValidationField>().ActiveErrorHighlight();
            return;
        }
        
        if (newName == managerPlayerData.PlayerHUDs.NameTeam.Value)
        {
            _generalPopupMessage.ShowInfo("The new team name must not match the old one");
            
            inputNewName.GetComponent<ValidationField>().ActiveErrorHighlight();
            
            return;
        }
        
        var success = await base.Save();

        if (success)
        {
            await _managerBalanceUpdate.UpdateBalance(_economy.changeTeamName);
            _officeCustomizeStatus.SetAction(StatusOfficeCustomize.TeamMenu);
            _generalPopupMessage.ShowInfo("Team name was changed");
        }
        else
        {
            inputNewName.GetComponent<ValidationField>().ActiveErrorHighlight();
            //inputShortName.GetComponent<ValidationField>().ActiveErrorHighlight();
        }
    }
}
