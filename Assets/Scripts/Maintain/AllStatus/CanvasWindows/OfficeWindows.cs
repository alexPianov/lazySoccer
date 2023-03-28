using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class OfficeWindows : BaseWindows<StatusOffice>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<OfficeStatus>();
            base.Awake();
        }
    }
}
