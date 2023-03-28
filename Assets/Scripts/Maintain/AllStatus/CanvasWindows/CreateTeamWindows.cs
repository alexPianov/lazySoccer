using LazySoccer.Status;
using LazySoccer.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTeamWindows : BaseWindows<StatusCreateTeam>
{
    public override void Awake()
    {
        if (baseStatus == null)
            baseStatus = ServiceLocator.GetService<CreateTeamStatus>();
        base.Awake();
    }
}
