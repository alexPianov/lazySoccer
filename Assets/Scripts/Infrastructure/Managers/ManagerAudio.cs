using System;
using Scripts.Infrastructure.ScriptableStore;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Scripts.Infrastructure.Managers
{
    public class ManagerAudio : MonoBehaviour
    {
        [Header("Store")]
        [SerializeField] private AudioStore audioStore;
        
        [Header("Source")]
        [SerializeField] private AudioSource sourceSounds;
        [SerializeField] private AudioSource sourceMusic;
        
        [Header("Mixer")]
        [SerializeField] private AudioMixer mixerSounds;
        [SerializeField] private AudioMixer mixerMusic;
        
        public enum AudioSound
        {
            Next, Close, Action, Success, Error,
            MenuMusic, CreateTeamMusic, LoginMusic, MatchMusic
        }

        private void Awake()
        {
            SceneManager.sceneLoaded += NewScene;
        }

        #region Player

        public void PlaySound(AudioSound type)
        {
            var sound = audioStore.GetSound(type);
            PlaySound(sound);
        }

        public void PlayMusic(AudioSound type)
        {
            var sound = audioStore.GetSound(type);
            PlayMusic(sound);
        }

        public void PauseMusic(bool state)
        {
            if (state)
            {
                sourceMusic.Pause();
            }
            else
            {
                sourceMusic.Play();
            }
        }

        private bool inPause;
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null)
            {
                inPause = true;
                sourceMusic.Pause(); return;
            }
            
            inPause = false;
            sourceMusic.clip = clip;
            sourceMusic.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            if(clip == null) return;
            
            sourceSounds.pitch = RandomValue(0.1f);
            sourceSounds.PlayOneShot(clip, RandomValue(0.2f, 1));
        }

        private float RandomValue(float amplitude, float volume = 1)
        {
            return Random.Range(volume - amplitude, volume + amplitude);
        }

        #endregion

        #region Volume

        public void MuteMusic(bool state)
        {
            if (state) mixerMusic.SetFloat("Volume", -80);
            else mixerMusic.SetFloat("Volume", 0);
        }

        public void MuteSounds(bool state)
        {
            if (state) mixerSounds.SetFloat("Volume", -80);
            else mixerSounds.SetFloat("Volume", 0);
        }
        
        #endregion

        #region Music

        private void Update()
        {
            if (!sourceMusic.isPlaying && InLobby())
            {
                //PlayMusic(AudioSound.MenuMusic);
            }
        }

        private static bool InLobby()
        {
            return SceneManager.GetActiveScene().name == "GameLobby" || 
                   SceneManager.GetActiveScene().name == "GameLobby_Web";
        }

        private void NewScene(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "LogInSignUp" || scene.name == "LogInSignUp_Web")
            {
                PlayMusic(AudioSound.LoginMusic);
            }

            if (scene.name == "CreateTeam" || scene.name == "CreateTeam_Web")
            {
                PlayMusic(AudioSound.CreateTeamMusic);
            }

            if (scene.name == "GameLobby" || scene.name == "GameLobby_Web")
            {
                PlayMusic(AudioSound.MenuMusic);
            }
        }
        
        void OnApplicationPause(bool pauseStatus)
        {
            PauseMusic(!pauseStatus);
        }

        void OnApplicationFocus(bool focusStatus) 
        {
            PauseMusic(!focusStatus);
        }

        #endregion
    }
}