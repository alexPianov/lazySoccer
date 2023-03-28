using UnityEngine;

namespace LazySoccer.Status
{
    public class OfficeCustomizeStatusForm : BaseStatus<StatusOfficeCustomizeForm>
    {
        [SerializeField] private StatusOfficeCustomizeForm back;
        public override StatusOfficeCustomizeForm StatusAction 
        { 
            set 
            {
                if (value == StatusOfficeCustomizeForm.Back)
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