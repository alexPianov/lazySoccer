using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LazySoccer.User.Uniform
{
    [CreateAssetMenu(fileName = "SlotColorScheme", menuName = "Player/SlotColorScheme", order = 1)]
    public class SlotColorScheme : ScriptableObject
    {
        [Title("Trauma")] 
        public Color traumaEnabled;
        public Color traumaDisabled;

        [Title("Position")] 
        public Color positionGoalkeeper;
        public Color positionMidfilder;
        public Color positionDefender;
        public Color positionForward;
    
        [Title("Form")]
        public Color formLow;
        public Color formMiddle;
        public Color formHigh;
    }
}
