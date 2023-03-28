using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LazySoccer.User.Emblem;
using System;

public class CreateTeam : MonoBehaviour
{
    [SerializeField] private ManagerPlayerData _playerData;
    [SerializeField] private ManagerEmblem _managerEmblem;

    [SerializeField] private FindValidationContainer _validation;

    [Title("Input")]
    [SerializeField] private TMP_InputField _nameTeam;
    [SerializeField] private TMP_InputField _shourtNameTeam;

    [Title("Text")]
    [SerializeField] private TMP_Text _fullTextTeam;

    [Title("Emblem")]
    [SerializeField] private Image _emblem;

    [Title("ButtonSelectColor")]
    [SerializeField] private Button _nextLayout;

    [Title("BaseContainerCreateEmblem")]
    [SerializeField] private Transform _content;

    private void Awake()
    {
        if (_managerEmblem == null) _managerEmblem = ServiceLocator.GetService<ManagerEmblem>();
        if (_playerData == null) _playerData = ServiceLocator.GetService<ManagerPlayerData>();

        _emblem.sprite = _managerEmblem.FirstSprite();
    }

    void Start()
    {
        _fullTextTeam.text = "";
        _nextLayout.interactable = false;

        _nameTeam.onValueChanged.AddListener(InputNameTeam);
        _shourtNameTeam.onValueChanged.AddListener(InputShourtNameTeam);
        _nextLayout.onClick.AddListener(OnClickNext);

        _managerEmblem.GenerationCardEmblem(_content);

        _managerEmblem.onActionSprite += UpdateSprite;
    }
    
    private void InputNameTeam(string value)
    {
        _playerData.PlayerHUDs.NameTeam.Value = value;
        UpdateTextFullTeam();
        UpdateChecked();
    }
    private void InputShourtNameTeam(string value)
    {
        _playerData.PlayerHUDs.NameShortTeam.Value = value;
        UpdateTextFullTeam();
        UpdateChecked();
    }
    private void UpdateTextFullTeam()
    {
        string nameTeam = _playerData.PlayerHUDs.NameTeam.Value;
        string nameShortTeam = _playerData.PlayerHUDs.NameShortTeam.Value;
        _fullTextTeam.text = $"{nameTeam} ({nameShortTeam})";
    }
    
    private void UpdateChecked()
    {
        string nameTeam = _playerData.PlayerHUDs.NameTeam.Value;
        string nameShortTeam = _playerData.PlayerHUDs.NameShortTeam.Value;
        _nextLayout.interactable = !String.IsNullOrEmpty(nameTeam) && !String.IsNullOrEmpty(nameShortTeam);
    }
    
    private void UpdateSprite(Sprite sprite)
    {
        _emblem.sprite = sprite;
    }
    private void OnClickNext()
    {
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction = StatusCreateTeam.CustomizeUniform;
    }
}
