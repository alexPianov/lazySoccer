using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Status
{
    public class PopupStatus : BaseStatus<StatusPopup>
    {
        [SerializeField] private StatusPopup back;
        public override StatusPopup StatusAction
        {
            set
            {
                if (value == StatusPopup.Back)
                {
                    status = back;
                }
                else
                    status = value;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}
