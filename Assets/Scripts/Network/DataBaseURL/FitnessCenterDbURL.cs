using System;
using UnityEngine;

namespace LazySoccer.Network.DataBaseURL
{
    [CreateAssetMenu(fileName = "FitnessCenterDbURL", menuName = "Networking/FitnessCenterDbURL", order = 2)]
    public class FitnessCenterDbURL: BaseDbURL<FitnessCenterRequests>
    {
        public FitnessCenterDbRequest dictionatyURL;
    }
    
    [Serializable]
    public class FitnessCenterDbRequest : BaseDbRequest<FitnessCenterRequests> { }
    
}