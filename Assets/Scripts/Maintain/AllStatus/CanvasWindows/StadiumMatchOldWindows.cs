using LazySoccer.Status;
using LazySoccer.Windows;

namespace LazySoccer.Windows
{
    public class StadiumMatchOldWindows : BaseWindows<StatusStadiumMatchOld>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<StadiumStatusMatchOld>();
            base.Awake();
        }
    }
}