using System;
using I2.Loc;
using LazySoccer.Table;
using TMPro;
using UnityEngine;
using Utils;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterTeamTable: SlotPlayerFactory
    {
        [SerializeField] private CenterMemberList memberList;
        [SerializeField] private TMP_Text teamCount;
        public SlotPlayer SlotPlayerLocal { get; set; }

        public void CreateTable()
        {
            CreatePlayersList();
            RefreshTeamCount();
            AddListener();
        }

        public void AddListener()
        {
            foreach (var playerSlot in GetTable())
            {
                if (memberList == null)
                {
                    var listener = playerSlot.gameObject
                        .AddComponent<CenterMemberListenerLocal>();
                    
                    listener.SetTable(this);
                }
                else
                {
                    var listener = playerSlot.gameObject
                        .AddComponent<CenterMemberListener>();
                    
                    listener.SetTable(memberList);
                }
            }
        }

        protected void RefreshTeamCount(int value = 0, bool hideMaxTeamCount = false)
        {
            if (value == 0) value = GetTable().Count; 
            
            var count = String.Format("{0}/{1}", value, StringStore.MaxPlayersInTeam);

            if (hideMaxTeamCount)
            {
                count = value.ToString();
            }
            
            teamCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", count);
        }
    }
}