using System;
using LazySoccer.Network;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.SportSchool
{
    public class SportSchool : MonoBehaviour
    {
        [SerializeField] private Button buttonTake;
        [SerializeField] private Button buttonInstant;
        [SerializeField] private Slider sliderProgress;
        private SportSchoolTypesOfRequest _sportSchoolTypesOfRequest;
        
        private void Start()
        {
            _sportSchoolTypesOfRequest = ServiceLocator.GetService<SportSchoolTypesOfRequest>();
            
            buttonInstant.onClick.AddListener(InstantFind);
            buttonTake.onClick.AddListener(Take);
        }

        public void UpdateStatus()
        {
            sliderProgress.value = 1;
        }

        public void Take()
        {
            buttonTake.interactable = false;
            buttonInstant.interactable = false;
        }

        public void InstantFind()
        {
            buttonTake.interactable = false;
            buttonInstant.interactable = false;
        }
    }
}