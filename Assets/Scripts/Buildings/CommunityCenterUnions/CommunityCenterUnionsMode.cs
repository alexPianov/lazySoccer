using System;
using LazySoccer.Popup;
using LazySoccer.Status;
using LazySoccer.Windows;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionsMode : MonoBehaviour
    {
        [Title("Mode")]
        public bool userCanJoinUnions { get; private set; }
        public bool userCanJoinTradeChannels { get; private set; }
        public bool userCanCreateUnions { get; private set; }
        public bool userCanCreateTournaments { get; private set; }
        
        [Title("Refs")]
        [SerializeField] private BuildingInformation BuildingInformation;
        
        private void Start()
        {
            BuildingInformation.updateLevel.AddListener(UpdateModeAccess);
        }

        public void UpdateModeAccess()
        {
            var level = BuildingInformation.GetHouseInfo().Level.Value;
            
            SetMode(level);
        }

        private void SetMode(int level)
        {
            switch (level) 
            {
                case <= 4: 
                    userCanJoinUnions = false;
                    userCanJoinTradeChannels = false;
                    userCanCreateUnions = false;
                    userCanCreateTournaments = false;
                    return;
                    
                case <= 9: 
                    userCanJoinUnions = true;
                    userCanJoinTradeChannels = false;
                    userCanCreateUnions = false;
                    userCanCreateTournaments = false;
                    return;
                
                case <= 14:
                    userCanJoinUnions = true;
                    userCanJoinTradeChannels = true;
                    userCanCreateUnions = false;
                    userCanCreateTournaments = false;
                    return;
                
                case <= 19: 
                    userCanJoinUnions = true;
                    userCanJoinTradeChannels = true;
                    userCanCreateUnions = true;
                    userCanCreateTournaments = false;
                    return;
                
                case <= 20: 
                    userCanJoinUnions = true;
                    userCanJoinTradeChannels = true;
                    userCanCreateUnions = true;
                    userCanCreateTournaments = true;
                    return;
                
                default: 
                    userCanJoinUnions = false;
                    userCanJoinTradeChannels = false;
                    userCanCreateUnions = false;
                    userCanCreateTournaments = false;
                    return;
            }
        }
    }
}