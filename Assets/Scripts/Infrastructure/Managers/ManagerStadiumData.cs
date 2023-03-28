using System.Collections;
using System.Collections.Generic;
using System.IO;
using LazySoccer.Network.Get;
using UnityEngine;

public class ManagerStadiumData : MonoBehaviour
{
    public List<AdditionClassGetRequest.Match> matches { get; private set; } 
    
    public void SetMatches(List<AdditionClassGetRequest.Match> _matches)
    {
        matches = _matches;
    }
}
