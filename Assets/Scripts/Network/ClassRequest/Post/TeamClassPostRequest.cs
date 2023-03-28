using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.Network.Post
{
    public class TeamClassPostRequest : MonoBehaviour
    {
        public class CreateTeamPost
        {
            public string name;
            public string shortName;
        }
        
        public class ChangeTeamName
        {
            public string name;
            public string shortName;
        }
    }
}
