using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class MarketWindows : BaseWindows<StatusMarket>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<MarketStatus>();
            base.Awake();
        }
    }
}