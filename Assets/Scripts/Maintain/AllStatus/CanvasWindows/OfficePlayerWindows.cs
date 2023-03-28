using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class OfficePlayerWindows : BaseWindows<StatusOfficePlayer>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<OfficePlayerStatus>();
            
            base.Awake();
        }
    }
}
