using LazySoccer.Network;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GetTeamDbURL", menuName = "Networking/GetTeamDbURL", order = 2)]
public class GetTeamDbURL : BaseDbURL<TeamRequests>
{
    public GetTeamDbRequest dictionatyURL;
}

[Serializable]
public class GetTeamDbRequest : BaseDbRequest<TeamRequests> { }
    