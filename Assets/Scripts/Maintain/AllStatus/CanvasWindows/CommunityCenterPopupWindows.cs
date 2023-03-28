using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class CommunityCenterPopupWindows : BaseWindows<StatusCommunityCenterPopup>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<CommunityCenterPopupStatus>();
            base.Awake();
        }
    }
}
