using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUniform : MonoBehaviour
{
    [Title("ButtonSelect")]
    [SerializeField] private Button _nextLayout;
    [SerializeField] private Button _backLayout;

    void Start()
    {
        _nextLayout.onClick.AddListener(OnClickNext);
        _backLayout.onClick.AddListener(OnClickBack);
    }

    private void OnClickNext()
    {
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction = StatusCreateTeam.Tutorial;
    }
    private void OnClickBack()
    {
        ServiceLocator.GetService<CreateTeamStatus>().StatusAction = StatusCreateTeam.Back;
    }
}
