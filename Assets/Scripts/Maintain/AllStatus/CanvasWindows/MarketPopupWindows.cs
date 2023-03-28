using LazySoccer.Status;

namespace LazySoccer.Windows
{
    public class MarketPopupWindows : BaseWindows<StatusMarketPopup>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<MarketPopupStatus>();
            base.Awake();
        }
    }
}