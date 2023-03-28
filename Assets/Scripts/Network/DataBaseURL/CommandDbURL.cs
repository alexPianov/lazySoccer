using LazySoccer.Network;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandDbURL", menuName = "Networking/CommandDbURL", order = 2)]
public class CommandDbURL : BaseDbURL<CommandRequests>
{
    public CommandDbRequest dictionatyURL;
}
    
[Serializable]
public class CommandDbRequest : BaseDbRequest<CommandRequests> { }