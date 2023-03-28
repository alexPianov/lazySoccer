using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class ModalWindows : BaseWindows<StatusModalWindows>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<ModalWindowStatus>();
            base.Awake();
        }
    }
}
