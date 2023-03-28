using UnityEngine;

namespace LazySoccer.Status
{
    public class CommunityCenterStatus : BaseStatus<StatusCommunityCenter>
    {
        [SerializeField] private StatusCommunityCenter back;
        public override StatusCommunityCenter StatusAction 
        { 
            set 
            {
                if (value == StatusCommunityCenter.Back)
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