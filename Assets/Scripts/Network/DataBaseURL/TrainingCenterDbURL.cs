using System;
using LazySoccer.Network;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingCenterDbURL", menuName = "Networking/TrainingCenterDbURL", order = 2)]
public class TrainingCenterDbURL : BaseDbURL<TrainingCenterRequests>
{
    public TrainingDbRequest dictionatyURL;
}
    
[Serializable]
public class TrainingDbRequest : BaseDbRequest<TrainingCenterRequests> { }