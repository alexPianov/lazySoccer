using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTeamStatus : BaseStatus<StatusCreateTeam>
{
    [SerializeField] private StatusCreateTeam back;
    public override StatusCreateTeam StatusAction
    {
        set
        {
            if (value == StatusCreateTeam.Back)
            {
                status = back;
            }
            else
            {
                back = status;
                status = value;
            }
            OnStateStatusAction?.Invoke(status);
        }
    }
}
