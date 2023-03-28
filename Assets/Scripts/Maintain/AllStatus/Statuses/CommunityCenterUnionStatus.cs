using UnityEngine;

namespace LazySoccer.Status
{
    public class CommunityCenterUnionStatus : BaseStatus<StatusCommunityCenterUnion>
    {
        [SerializeField] private StatusCommunityCenterUnion back;
        public override StatusCommunityCenterUnion StatusAction 
        { 
            set 
            {
                if (value == StatusCommunityCenterUnion.Back)
                {
                    status = back;
                }
                else
                    status = value;
                
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}