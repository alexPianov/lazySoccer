using UnityEngine;

namespace LazySoccer.Status
{
    public class CommunityCenterFriendStatus : BaseStatus<StatusCommunityCenterFriend>
    {
        [SerializeField] private StatusCommunityCenterFriend back;
        public override StatusCommunityCenterFriend StatusAction 
        { 
            set 
            {
                if (value == StatusCommunityCenterFriend.Back)
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