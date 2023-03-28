using LazySoccer.Network;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MedicalCenterDbURL", menuName = "Networking/MedicalCenterDbURL", order = 2)]
public class MedicalCenterDbURL : BaseDbURL<MedicalCenterRequests>
{
    public MedicalCenterDbRequest dictionatyURL;
}
    
[Serializable]
public class MedicalCenterDbRequest : BaseDbRequest<MedicalCenterRequests> { }