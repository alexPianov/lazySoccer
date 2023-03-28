using System;
using System.Collections.Generic;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using static Scripts.Infrastructure.Managers.ManagerCommunityData;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenter
{
    public class CommunityCenterFriendStatusCheck : MonoBehaviour
    {
        public bool StatusMatch(FriendType friendType, List<string> names)
        {
            foreach (var searchName in names)
            {
                var result = StatusMatch(friendType, searchName);
                if (result) return true;
            }
            
            return false;
        }
        
        public bool StatusMatch(FriendType friendType, string searchName)
        {
            var _managerCommunityData = ServiceLocator.GetService<ManagerCommunityData>();
            
            var userNameMatchResult = _managerCommunityData
                .GetFriends(friendType)
                .Find(user => user.userName == searchName);
                
            var teamNameMatchResult = _managerCommunityData
                .GetFriends(friendType)
                .Find(user => user.team.name == searchName);
                
            return userNameMatchResult != null || teamNameMatchResult != null;
        }
        
        public bool StatusMatchWithCurrentUser(List<string> names)
        {
            foreach (var searchName in names)
            {
                var result =  StatusMatchWithCurrentUser(searchName);
                if (result) return true;
            }
            
            return false;
        }
        
        public bool StatusMatchWithCurrentUser(string searchName)
        {
            var hud = ServiceLocator.GetService<ManagerPlayerData>().PlayerHUDs;

            if (searchName == hud.NameTeam.Value) return true;
            if (searchName == hud.Name.Value) return true;
                
            return false;
        }
    }
}