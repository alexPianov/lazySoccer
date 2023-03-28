using System;
using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Status;
using LazySoccer.Table;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamPlayersList : SlotPlayerFactory
    {
        [Title("Players")] 
        [SerializeField] private List<SlotPlayer> players;

        [Title("Refs")]
        [SerializeField] private RegistrationTeamPlayerDisplay display;
        [SerializeField] private GameObject displayPanel; 
        [SerializeField] private RegistrationTeamPlayersSave playersSave;

        public void UpdateWindowStatus()
        {
            ServiceLocator.GetService<CreateTeamOptionsStatus>()
                .SetAction(StatusCreateTeamOptions.GenerationOne);
        }

        public void OpenTeamList(int number)
        {
            UpdateTeamList((TeamPlayerStatus)number);
        }

        private void UpdateTeamList(TeamPlayerStatus playerStatus)
        {
            display.SetPlayer(null);
            
            RemoveAllSlots();

            var slots = CreatePlayersList(playerStatus);
            players.AddRange(slots);
            
            AddSelectListener();
            
            playersSave.SetOption(playerStatus);
            
            Debug.Log("players count: " + players.Count);
        }
        
        private void AddSelectListener()
        {
            for (var i = 0; i < players.Count; i++)
            {
                var listener = players[i].gameObject.AddComponent<RegistrationTeamPlayerListener>();
                
                listener.SetSource(this);
            }
        }

        public void SelectPlayer(SlotPlayer slotPlayer)
        {
            Debug.Log("SelectPlayer: " + slotPlayer.GetTeamPlayer().name);
            display.SetPlayer(slotPlayer);
            displayPanel.SetActive(true);
        }

        public void ClosePlayerDisplay()
        {
            displayPanel.SetActive(false);
        }

        public void RemoveAllSlots()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i]) Destroy(players[i].gameObject);
            }
            
            players.Clear();
        }
    }
}