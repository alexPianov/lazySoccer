using System;
using LazySoccer.Network;
using UnityEngine;

[CreateAssetMenu(fileName = "CommunityDbURL", menuName = "Networking/CommunityDbURL", order = 2)]
public class CommunityDbURL : BaseDbURL<CommunityRequests>
{
    public CommunityDbRequest dictionatyURL;
}
    
[Serializable]
public class CommunityDbRequest : BaseDbRequest<CommunityRequests> { }