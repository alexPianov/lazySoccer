using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class StadiumWindows : BaseWindows<StatusStadium>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<StadiumStatus>();
            base.Awake();
        }
    }
}