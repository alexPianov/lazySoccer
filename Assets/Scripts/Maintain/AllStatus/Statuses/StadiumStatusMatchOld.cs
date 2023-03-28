using UnityEngine;

namespace LazySoccer.Status
{
    public class StadiumStatusMatchOld : BaseStatus<StatusStadiumMatchOld>
    {
        [SerializeField] private StatusStadiumMatchOld back;
        [HideInInspector] public StatusStadiumMatchOld lastStatus;
        public override StatusStadiumMatchOld StatusAction 
        { 
            set {

                status = value;

                lastStatus = status;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}