using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.Status
{
    public class OfficeStatus : BaseStatus<StatusOffice>
    {
        [SerializeField] private StatusOffice back;
        public override StatusOffice StatusAction 
        { 
            set {

                if (value == StatusOffice.Back)
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
