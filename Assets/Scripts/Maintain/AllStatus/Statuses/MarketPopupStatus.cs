using UnityEngine;

namespace LazySoccer.Status
{
    public class MarketPopupStatus : BaseStatus<StatusMarketPopup>
    {
        [SerializeField] private StatusMarketPopup back;
        public override StatusMarketPopup StatusAction 
        { 
            set {

                if (value == StatusMarketPopup.Back)
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