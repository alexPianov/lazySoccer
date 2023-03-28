using System;
using Scripts.Infrastructure.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamName : MonoBehaviour
    {
        [Title("Input")]
        [SerializeField] public TMP_InputField teamName;
        [SerializeField] public TMP_InputField teamNameShort;
        [SerializeField] private FindValidationContainer _validationContainer;

        [Title("Text")]
        [SerializeField] private TMP_Text textNameFull;
        [SerializeField] private GameObject displayNameFull;

        [Title("Button")]
        [SerializeField] private Button buttonNext;

        [Title("Ref")] 
        [SerializeField] private RegistrationTeamCreate TeamCreate;
        [SerializeField] private Color inputActiveColor = new Color(255,255,255,255);
        [SerializeField] private Color inputDeactiveColor = new Color(255,255,255,100);

        private ManagerPlayerData _playerData;
        
        private void Awake()
        {
            _validationContainer = GetComponent<FindValidationContainer>();
            _playerData = ServiceLocator.GetService<ManagerPlayerData>();

            teamName.onValueChanged.AddListener(InputNameTeam);
            teamNameShort.onValueChanged.AddListener(InputShortNameTeam);

            buttonNext.interactable = false;
        }

        public void CheckForTeamCreation()
        {
            CheckName();
            CheckNameShort();
        }

        private void CheckName()
        {
            var nameIsExists = NameExists();
            teamName.interactable = !nameIsExists;
            
            TeamCreate.teamIsCreated = nameIsExists;
            
            if (nameIsExists)
            {
                buttonNext.interactable = true;

                teamName.text = _playerData.PlayerHUDs.NameTeam.Value;
                
                teamName.textComponent.color = inputDeactiveColor;
                
                UpdateFullName();
            }
            else
            {
                textNameFull.text = "";
                
                displayNameFull.SetActive(false);
                buttonNext.interactable = false;
                
                teamName.text = "";
                
                teamName.textComponent.color = inputActiveColor;
                
                _playerData.PlayerHUDs.NameShortTeam.Value = "";
            }
        }

        private bool NameExists()
        {
            return _playerData.PlayerHUDs.NameTeam.Value.Length > 0;
        }

        private void CheckNameShort()
        {
            var nameShortIsExists = _playerData.PlayerHUDs.NameShortTeam.Value.Length > 0;
            teamNameShort.interactable = !nameShortIsExists;

            if (nameShortIsExists)
            {
                teamNameShort.text = _playerData.PlayerHUDs.NameShortTeam.Value;
                teamNameShort.textComponent.color = inputDeactiveColor;
                
                UpdateFullName();
            }
            else
            {
                teamNameShort.text = "";
                teamNameShort.textComponent.color = inputActiveColor;
            }
        }
        
        private void InputNameTeam(string value)
        {
            _playerData.PlayerHUDs.NameTeam.Value = value;
            UpdateFullName();
            CheckNextButton();
        }
        
        private void InputShortNameTeam(string value)
        {
            _playerData.PlayerHUDs.NameShortTeam.Value = value.ToUpper();
            teamNameShort.text = value.ToUpper();
            
            UpdateFullName();
            CheckNextButton();
        }
        
        private void UpdateFullName()
        {
            string nameTeam = _playerData.PlayerHUDs.NameTeam.Value;
            string nameShortTeam = _playerData.PlayerHUDs.NameShortTeam.Value;
            textNameFull.text = $"{nameTeam} ({nameShortTeam})";
        }
        
        private void CheckNextButton()
        {
            string nameTeam = _playerData.PlayerHUDs.NameTeam.Value;
            string nameShortTeam = _playerData.PlayerHUDs.NameShortTeam.Value;

            buttonNext.interactable =
                ValidationUtils.StringTeamNameValid(nameTeam) &&
                ValidationUtils.StringTeamNameShortValid(nameShortTeam);
            
            displayNameFull.SetActive(nameTeam.Length != 0 || nameShortTeam.Length != 0);
        }

        public bool IsValid()
        {
            string nameShortTeam = _playerData.PlayerHUDs.NameShortTeam.Value;
            
            if (ValidationUtils.OffensiveShortcuts(nameShortTeam))
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Short name contains the offensive word", false);
            
                teamNameShort.GetComponent<ValidationField>().ActiveErrorHighlight();
                
                return false;
            }
            
            return _validationContainer.isRatification();
        }

    }
}