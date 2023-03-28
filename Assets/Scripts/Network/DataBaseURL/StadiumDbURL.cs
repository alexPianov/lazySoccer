using System;
using LazySoccer.Network;
using UnityEngine;

[CreateAssetMenu(fileName = "StadiumDbURL", menuName = "Networking/StadiumDbURL", order = 2)]
public class StadiumDbURL : BaseDbURL<StadiumRequests>
{
    public StadiumDbRequest dictionatyURL;
}
    
[Serializable]
public class StadiumDbRequest : BaseDbRequest<StadiumRequests> { }