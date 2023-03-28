using UnityEngine;

namespace LazySoccer.Status
{
    public class StadiumStatus : BaseStatus<StatusStadium>
    {
        [SerializeField] private StatusStadium back;
        [HideInInspector] public StatusStadium lastStatus;
        public override StatusStadium StatusAction 
        { 
            set {

                if (value == StatusStadium.Back)
                {
                    status = back;
                }
                else
                    status = value;

                lastStatus = status;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}