using System;
using System.Collections.Generic;
using I2.Loc;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Status;
using LazySoccer.Table;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEditor;
using UnityEngine;
using Utils;

namespace LazySoccer.SceneLoading.Buildings.MedicalCenter
{
    public class OfficeTeamTable : SlotPlayerFactory
    {
        [SerializeField] private TMP_Text teamCount;
        [SerializeField] private OfficePlayer.OfficePlayer pickedPlayer;
        [SerializeField] private TMP_Dropdown dropdown;
        private List<SlotPlayer> table = new();

        public void CreateTable()
        {
            table = CreatePlayersList();
            
            RefreshTeamCount();
            AddListener();

            dropdown.value = 0;
        }

        public void Filter(TMP_Dropdown status)
        {
            foreach (var slotPlayer in table)
            {
                if (status.value == 0)
                {
                    slotPlayer.Active(true);
                    continue;
                }
                
                var match = (int)slotPlayer.GetTeamPlayer().status == status.value - 1;
                slotPlayer.Active(match);
            }
            
            ServiceLocator.GetService<ManagerAudio>().PlaySound(ManagerAudio.AudioSound.Action);
        }

        private void AddListener()
        {
            foreach (var playerSlot in GetTable())
            {
                var listener = playerSlot.gameObject.AddComponent<OfficeTeamPlayerListener>();
                listener.SetTeamTable(this);
            }
        }

        private void RefreshTeamCount()
        {
            var value = String.Format("{0}/{1}", GetTable().Count, StringStore.MaxPlayersInTeam);
            teamCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", value);
        }

        public void PlayerDetails(SlotPlayer slotPlayer)
        {
            pickedPlayer.SetPlayer(slotPlayer.GetTeamPlayer());
        }
    }
}