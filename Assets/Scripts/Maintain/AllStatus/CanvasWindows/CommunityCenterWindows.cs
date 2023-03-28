using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class CommunityCenterWindows : BaseWindows<StatusCommunityCenter>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<CommunityCenterStatus>();
            base.Awake();
        }
    }
}
