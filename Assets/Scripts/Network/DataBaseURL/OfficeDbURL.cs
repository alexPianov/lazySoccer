using LazySoccer.Network;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "OfficeDbURL", menuName = "Networking/OfficeDbURL", order = 2)]
public class OfficeDbURL : BaseDbURL<OfficeRequests>
{
    public OfficeDbRequest dictionatyURL;
}
    
[Serializable]
public class OfficeDbRequest : BaseDbRequest<OfficeRequests> { }