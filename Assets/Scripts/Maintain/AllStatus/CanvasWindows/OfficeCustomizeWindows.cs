using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class OfficeCustomizeWindows : BaseWindows<StatusOfficeCustomize>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<OfficeCustomizeStatus>();
            base.Awake();
        }
    }
}