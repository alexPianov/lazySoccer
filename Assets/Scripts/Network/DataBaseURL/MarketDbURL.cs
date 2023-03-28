using System;
using UnityEngine;

namespace LazySoccer.Network.DataBaseURL
{
    [CreateAssetMenu(fileName = "MarketDbURL", menuName = "Networking/MarketDbURL", order = 2)]
    public class MarketDbURL : BaseDbURL<MarketRequests>
    {
        public MarketDbRequest dictionatyURL;
    }
    
    [Serializable]
    public class MarketDbRequest : BaseDbRequest<MarketRequests> { } 
}