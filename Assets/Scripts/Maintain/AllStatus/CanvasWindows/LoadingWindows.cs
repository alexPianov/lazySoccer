using LazySoccer.Status;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.Windows
{
    public class LoadingWindows : BaseWindows<StatusLoading>
    {
        public override void Start()
        {
            if (baseStatus == null)
            {
                baseStatus = ServiceLocator.GetService<LoadingStatus>();
            }
            base.Start();
        }
    }
}
