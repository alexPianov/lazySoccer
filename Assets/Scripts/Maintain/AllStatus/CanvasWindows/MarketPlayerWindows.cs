using LazySoccer.Status;
using LazySoccer.Windows;

namespace LazySoccer.SceneLoading.AllStatus
{
    public class MarketPlayerWindows : BaseWindows<StatusMarketPlayer>
    {
        public override void Awake()
        {
            if (baseStatus == null)
                baseStatus = ServiceLocator.GetService<MarketPlayerStatus>();
            base.Awake();
        }
    }
}