using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class CreateTeamOptionsWindows : BaseWindows<StatusCreateTeamOptions>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<CreateTeamOptionsStatus>();
            base.Awake();
        }
    }
}