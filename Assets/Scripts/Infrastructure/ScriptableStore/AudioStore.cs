using System;
using System.Collections.Generic;
using Scripts.Infrastructure.Managers;
using UnityEngine;

namespace Scripts.Infrastructure.ScriptableStore
{
    [CreateAssetMenu(fileName = "Audio", menuName = "Effects/Audio", order = 1)]
    public class AudioStore : ScriptableObject
    {
        [SerializeField]
        private List<Sound> sounds;

        [System.Serializable]
        public class Sound
        {
            public ManagerAudio.AudioSound soundType;
            public AudioClip soundClip;
        }

        public AudioClip GetSound(ManagerAudio.AudioSound soundType)
        {
            return sounds.Find(sound => sound.soundType == soundType).soundClip;
        }
    }
}