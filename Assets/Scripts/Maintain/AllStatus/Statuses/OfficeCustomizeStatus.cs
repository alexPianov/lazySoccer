using UnityEngine;

namespace LazySoccer.Status
{
    public class OfficeCustomizeStatus : BaseStatus<StatusOfficeCustomize>
    {
        [SerializeField] private StatusOfficeCustomize back;
        public override StatusOfficeCustomize StatusAction 
        { 
            set 
            {
                if (value == StatusOfficeCustomize.Back)
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