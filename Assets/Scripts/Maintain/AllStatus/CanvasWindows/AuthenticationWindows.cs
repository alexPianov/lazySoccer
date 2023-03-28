using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class AuthenticationWindows : BaseWindows<StatusAuthentication>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<AuthenticationStatus>();
            base.Awake();
        }
    }
}
