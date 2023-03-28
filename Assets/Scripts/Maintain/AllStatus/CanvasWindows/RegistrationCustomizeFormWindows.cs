using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class RegistrationCustomizeFormWindows: BaseWindows<StatusOfficeCustomizeForm>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<RegistrationCustomizeStatusForm>();
            base.Awake();
        }
        
    }
}