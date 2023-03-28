using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazySoccer.User.Uniform;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class SlotPlayerColor : MonoBehaviour
{
    [Title("Color Schema")]
    [SerializeField] private SlotColorScheme scheme;
    
    [Title("Refs")]
    [SerializeField] private TMP_Text textForm;
    [SerializeField] private Image lineStatus;
    [SerializeField] private Image backPanel;

    public void SetTeamPlayer(TeamPlayer teamPlayer)
    {
        SetForm(teamPlayer.form);
        SetStatus(teamPlayer.position.position);
    }

    public void SetTrauma(bool state)
    {
        if (state) backPanel.color = scheme.traumaEnabled;
        else backPanel.color = scheme.traumaEnabled; 
    }

    public void SetForm(int value)
    {
        switch (value)
        {
            case <= 30:
                textForm.color = scheme.formLow; 
                return;
            case < 70: 
                textForm.color = scheme.formMiddle; 
                return;
            case >= 70:
                textForm.color = scheme.formHigh; 
                return;
        }
    }

    public void SetStatus(string position)
    {
        var positionShort = position.First();

        if (positionShort == 'G') lineStatus.color = scheme.positionGoalkeeper; 
        if (positionShort == 'D') lineStatus.color = scheme.positionDefender; 
        if (positionShort == 'F') lineStatus.color = scheme.positionForward; 
        if (positionShort == 'M') lineStatus.color = scheme.positionMidfilder; 
    }

}
