using UnityEngine;

namespace LazySoccer.Status
{
    public class CreateTeamOptionsStatus : BaseStatus<StatusCreateTeamOptions>
    {
        [SerializeField] private StatusCreateTeamOptions back;
        public override StatusCreateTeamOptions StatusAction
        {
            set
            {
                if (value == StatusCreateTeamOptions.Back)
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
}