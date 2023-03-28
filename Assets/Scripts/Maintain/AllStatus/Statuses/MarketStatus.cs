using UnityEngine;

namespace LazySoccer.Status
{
    public class MarketStatus : BaseStatus<StatusMarket>
    {
        [SerializeField] private StatusMarket back;
        [HideInInspector] public StatusMarket lastStatus;
        public override StatusMarket StatusAction 
        { 
            set {

                if (value == StatusMarket.Back)
                {
                    status = back;
                }
                else
                    status = value;

                lastStatus = status;
                OnStateStatusAction?.Invoke(status);
            }
        }
    }
}