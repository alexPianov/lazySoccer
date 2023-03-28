using System;
using System.Collections.Generic;
using I2.Loc;
using LazySoccer.Network.Get;
using LazySoccer.Table;
using TMPro;
using UnityEngine;
using Utils;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriend
{
    public class CommunityCenterFriendTeamTable : SlotPlayerFactory
    {
        [SerializeField] private TMP_Text teamCount;

        [SerializeField] private CommunityCenterFriend centerFriend;

        public void CreateTable()
        {
            CreatePlayersList(centerFriend.CurrentTeamRoster);
            RefreshTeamCount();
        }

        private void RefreshTeamCount()
        {
            var value = String.Format("{0}/{1}",
                GetTable().Count, StringStore.MaxPlayersInTeam);
            
            teamCount.GetComponent<Localize>().SetTerm("3-General-Roster-Text-TeamCount");
            teamCount.GetComponent<LocalizationParamsManager>().SetParameterValue("param", value);
        }
    }
}