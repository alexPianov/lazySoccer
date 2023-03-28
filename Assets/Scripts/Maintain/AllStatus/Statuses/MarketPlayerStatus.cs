using UnityEngine;

namespace LazySoccer.Status
{
    public class MarketPlayerStatus : BaseStatus<StatusMarketPlayer>
    {
        [SerializeField] private StatusMarketPlayer back;
        public override StatusMarketPlayer StatusAction 
        { 
            set {

                if (value == StatusMarketPlayer.Back)
                {
                    status = back;
                }
                else
                    status = value;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}