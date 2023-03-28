using System.Collections.Generic;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.CommunityCenter.CommunityCenterLeaders;

namespace Scripts.Infrastructure.Managers
{
    public class ManagerCommunityData : MonoBehaviour
    {
        #region World Rating

        private Dictionary<LeaderFilter, List<WorldRatingUser>> worldRatingUsers = new();

        public void SetUserWorldRating(LeaderFilter leaderFilter, List<WorldRatingUser> users)
        {
            if (worldRatingUsers.ContainsKey(leaderFilter))
            {
                worldRatingUsers.Remove(leaderFilter);
            }
            
            Debug.Log("SetUserWorldRating: " + leaderFilter + " Users: " + users.Count);
            
            worldRatingUsers.Add(leaderFilter, users);
        }

        public bool WorldRatingDataIsCached(LeaderFilter leaderFilter)
        {
            return worldRatingUsers.ContainsKey(leaderFilter);
        }
        
        public List<WorldRatingUser> GetUsersWorldRating(LeaderFilter leaderFilter)
        {
            worldRatingUsers.TryGetValue(leaderFilter, out var list);
            return list;
        }

        #endregion

        #region Union

        private List<Union> communityUnions = new();
        private Union communityUserUnion;
        private UnionProfile communityUserUnionProfile;
        
        public void SetUnions(List<Union> unions, UnionFilterSettings s)
        {
            var filteredUnions = unions;

            if (filteredUnions == null)
            {
                Debug.Log("Unions is null");
                filteredUnions = new List<Union>();
                communityUnions = filteredUnions;
                return;
            }
            
            if (s.toggleHideClosed)
            {
                Debug.Log("unions: " + unions.Count);
                filteredUnions = unions
                    .FindAll(union => union.policy == RecruitingPolicy.Open);
            }

            if (s.toggleHideFull && unions != null)
            {
                filteredUnions = unions
                    .FindAll(union => union.maxMembersCount != s.maxMembers);
                
                if (s.toggleHideClosed)
                {
                    filteredUnions = filteredUnions
                        .FindAll(union => union.policy == RecruitingPolicy.Open);
                }
            }
            
            communityUnions = filteredUnions;
            Debug.Log("--- communityUnions is get | count: " + communityUnions.Count);
        }

        public List<Union> GetUnions(bool getUserUnions)
        {
            if (getUserUnions)
            {
                List<Union> unions = new ();
                
                if (communityUserUnion != null)
                {
                    unions.Add(communityUserUnion);
                }
                
                return unions;
            }
            
            return communityUnions;
        }
        
        public void SetUserUnion(Union union)
        {
            communityUserUnion = union;
        }

        public UnionProfile GetOwnUnionProfile()
        {
            return communityUserUnionProfile;
        }

        public UnionBuilding GetDonationBuilding()
        {
            foreach (var building in communityUserUnionProfile.unionBuildings)
            {
                if(building.isDonationEnabled) return building;
            }

            return null;
        }
        
        public void SetUserUnionProfle(UnionProfile union)
        {
            communityUserUnionProfile = union;
        }
        
        #endregion

        #region Friends

        private List<User> _friends = new();
        private List<FriendshipRequest> _friendshipRequests = new();
        
        public enum FriendType
        {
            Confirmed, Income, Outcome
        }
        
        public void SetFriends(List<User> users)
        {
            _friends = users;
        }
        
        public void SetFriendshipRequests(List<FriendshipRequest> friendshipRequests)
        {
            _friendshipRequests = friendshipRequests;
        }

        public List<User> GetFriends(FriendType friendType, bool noMatchRequests = true)
        {
            var _hud = ServiceLocator.GetService<ManagerPlayerData>().PlayerHUDs;

            var _userList = new List<User>();

            if (friendType == FriendType.Confirmed)
            {
                _userList = _friends;
            }

            if (_friendshipRequests == null) return new List<User>();

            if (friendType == FriendType.Income)
            {
                foreach (var request in _friendshipRequests)
                {
                    if (request.recipient == null) break;
                    if (request.sender == null) break;
                    if (noMatchRequests && request.type == FriendshipType.Game) continue;

                    if (request.recipient.userName == _hud.Name.Value)
                    {
                        _userList.Add(request.sender);
                    }
                }
            }
            
            if (friendType == FriendType.Outcome)
            {
                foreach (var request in _friendshipRequests)
                {
                    if (request.sender == null) break;
                    if (request.recipient == null) break;
                    if (noMatchRequests && request.type == FriendshipType.Game) continue;
                    
                    if (request.sender.userName == _hud.Name.Value)
                    {
                        _userList.Add(request.recipient);
                    }
                }
            }
            
            return _userList;
        }

        public FriendshipRequest GetFriendshipRequest(string userId)
        {
            return _friendshipRequests.Find(request => request.sender.userId == userId);
        }

        public List<FriendshipRequest> GetMatchRequests()
        {
            var _requests = new List<FriendshipRequest>();
            
            foreach (var request in _friendshipRequests)
            {
                if (request.sender == null) break;
                if (request.recipient == null) break;
                    
                if (request.type == FriendshipType.Game)
                {
                    _requests.Add(request);
                }
            }

            return _requests;
        }

        public bool HaveMatchWith(User user)
        {
            var matches = GetMatchRequests();

            foreach (var match in matches)
            {
                if (match.sender.userId == user.userId ||
                    match.recipient.userId == user.userId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}