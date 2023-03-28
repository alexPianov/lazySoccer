using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class OfficeCustomizeFormWindows : BaseWindows<StatusOfficeCustomizeForm>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<OfficeCustomizeStatusForm>();
            base.Awake();
        }
    }
}