using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PositionPicker : MonoBehaviour
{
    [SerializeField]
    public LineupsStorage lineupsStorage;
    [SerializeField]
    public PlayerPlaceholdersStorage playerPlaceholders;

    [SerializeField]
    public TMP_Dropdown lineupDropdown;

    private void Start()
    {
        SetPositions();
    }

    public void SetPositions()
    {
        var lineup = lineupsStorage.lineupsStorage[lineupDropdown.value];
        playerPlaceholders.SetupPlayerPlaceholders(lineup);
    }
}

public enum PositionCodes
{
    //FiveFourOne,
    FiveThreeTwo,
    //FourFiveOne,
    //FourFourTwo,
    FourThreeThree,
   // FourTwoFour,
    //ThreeThreeFour,
    //ThreeFourThree,
    ThreeFiveTwo,
    //ThreeSixOne
}
