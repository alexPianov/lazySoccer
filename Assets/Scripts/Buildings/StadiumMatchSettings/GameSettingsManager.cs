using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSettingsManager : MonoBehaviour
{
    [SerializeField]
    private MatchSettingsManager matchSettingsManager;

    [SerializeField]
    private TMP_Dropdown gameStyleDropdown;
    [SerializeField]
    private TMP_Dropdown gameTacticsDropdown;
    [SerializeField]
    private TMP_Dropdown takeBallDropdown;
    [SerializeField]
    private TMP_Dropdown pressingDropdown;
    [SerializeField]
    private TMP_Dropdown passRangeDropdown;
    [SerializeField]
    private TMP_Dropdown defensiveLineDropdown;

    private List<int> playersID;

    public void SetupTakeBallDropdown(List<string> names, List<int> playersID)
    {
        takeBallDropdown.ClearOptions();
        takeBallDropdown.AddOptions(names);
        this.playersID = playersID;
    }

    public void ApplyGameSettings()
    {
        matchSettingsManager.SetMatchSettings(gameStyleDropdown.value, gameTacticsDropdown.value, pressingDropdown.value, passRangeDropdown.value, defensiveLineDropdown.value, playersID.Count > 0 ? playersID[takeBallDropdown.value]: 0);
        CloseGameSettingsMenu();

    }

    private void CloseGameSettingsMenu()
    {
        this.gameObject.SetActive(false);
    }
}
