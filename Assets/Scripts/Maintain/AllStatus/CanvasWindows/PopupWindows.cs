using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class PopupWindows : BaseWindows<StatusPopup>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<PopupStatus>();
            base.Awake();
        }
    }
}
