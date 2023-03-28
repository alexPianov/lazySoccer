using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class CommunityCenterFriendWindows : BaseWindows<StatusCommunityCenterFriend>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<CommunityCenterFriendStatus>();
            base.Awake();
        }
    }
}
