using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.Status
{
    public class OfficePlayerStatus : BaseStatus<StatusOfficePlayer>
    {
        [SerializeField] private StatusOfficePlayer back;
        public override StatusOfficePlayer StatusAction 
        { 
            set {

                if (value == StatusOfficePlayer.Back)
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
