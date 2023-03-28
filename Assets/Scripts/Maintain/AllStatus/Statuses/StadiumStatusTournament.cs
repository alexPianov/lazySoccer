using UnityEngine;

namespace LazySoccer.Status
{
    public class StadiumStatusTournament : BaseStatus<StatusStadiumTournament>
    {
        [SerializeField] private StatusStadiumTournament back;
        [HideInInspector] public StatusStadiumTournament lastStatus;
        public override StatusStadiumTournament StatusAction 
        { 
            set {

                status = value;

                lastStatus = status;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}