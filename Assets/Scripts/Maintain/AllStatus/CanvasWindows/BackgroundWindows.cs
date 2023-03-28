using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class BackgroundWindows : BaseWindows<StatusBackground>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<BackgroundStatus>();
            base.Awake();
        }
    }
}
