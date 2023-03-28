using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class BuildingWindows : BaseWindows<StatusBuilding>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<BuildingWindowStatus>();
            base.Awake();
        }
    }
}