using UnityEngine;

namespace LazySoccer.Status
{
    public class CommunityCenterPopupStatus : BaseStatus<StatusCommunityCenterPopup>
    {
        [SerializeField] private StatusCommunityCenterPopup back;
        [HideInInspector] public StatusCommunityCenterPopup currentStatus;
        public override StatusCommunityCenterPopup StatusAction 
        { 
            set 
            {
                if (value == StatusCommunityCenterPopup.Back)
                {
                    status = back;
                }
                else
                {
                    if (currentStatus != value)
                    {
                        status = value;
                    }
                }

                currentStatus = status;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}