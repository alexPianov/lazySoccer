using System;
using Lean.Gui;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Scripts.Infrastructure.Managers.ManagerAudio;

namespace LazySoccer.SceneLoading.Infrastructure.Audio
{
    public class AudioButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] 
        private AudioSound currentSound;
        
        private ManagerAudio _managerAudio;
        
        private void Start()
        {
            _managerAudio = ServiceLocator.GetService<ManagerAudio>();
            
            if (GetComponent<Toggle>())
            {
                GetComponent<Toggle>().onValueChanged.AddListener(Toggle);
                return;
            }

            if (GetComponent<TMP_InputField>())
            {
                GetComponent<TMP_InputField>().onSelect.AddListener(Input);
                return;
            } 
            
            if (GetComponent<LeanButton>())
            {
                GetComponent<LeanButton>().OnClick.AddListener(PlaySound);
                return;
            }

            if (GetComponent<Button>() == null)
            {
                gameObject.AddComponent<Button>();
            }
            
            if (GetComponent<Button>())
            {
                GetComponent<Button>().onClick.AddListener(PlaySound);
            }
        }

        private void Toggle(bool state)
        {
            PlaySound();
        }
        
        
        private void Dropdown(int num)
        {
            PlaySound();
        }


        private void Input(string state)
        {
            PlaySound();
        }
        
        public void PlaySound()
        {
            if (_managerAudio)
            {
                _managerAudio.PlaySound(currentSound);
            }
            else
            {
                ServiceLocator.GetService<ManagerAudio>().PlaySound(currentSound);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        { 
            if (GetComponent<TMP_Dropdown>())
            {
                PlaySound();
            }
        }
    }
}