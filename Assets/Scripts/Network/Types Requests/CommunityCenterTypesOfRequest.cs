using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings;
using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.CommunityCenter.CommunityCenterLeaders;

namespace LazySoccer.Network
{
    public class CommunityCenterTypesOfRequest : BaseTypesOfRequests<CommunityRequests>
    {
        [SerializeField] private CommunityDbURL dbURL;
        [SerializeField] private UnionFilterSettings CustomFilterSettings;

        private bool _requestProcessing;
        
        public override string FullURL(string URL, CommunityRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }
        
        public async UniTask<bool> GET_WorldRating(LeaderFilter leaderFilter, 
            int maxBorder = 100, int minBorder = 0, bool globalLoading = true)
        {
            var param = $"/{(int)leaderFilter}/{maxBorder}/{minBorder}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.RatingWorld, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param,
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get Users World Rating: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<WorldRatingUser>>
                    (result.downloadHandler.text);

                if (a != null)
                {
                    ServiceLocator.GetService<ManagerCommunityData>()
                        .SetUserWorldRating(leaderFilter, a);
                }
                else
                {
                    Debug.LogError("Deserialized world rating data is null");
                }
            }

            return true;
        }
        
        public async UniTask<List<GeneralClassGETRequest.User>> GET_Users(string userName)
        {
            var param = $"/{userName}/{10}/{0}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.Users, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param,
                ActiveGlobalLoading: false);
            
            Debug.Log("Get Users: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<GeneralClassGETRequest.User>>
                    (result.downloadHandler.text);
            
                if(a != null) return a;
            }

            return null;
        }

        #region Friends GET Requests

        public async UniTask GET_Friends(bool globalLoading = true)
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.Friends, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get Friends: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<GeneralClassGETRequest.User>>
                    (result.downloadHandler.text);
            
                if(a != null) ServiceLocator.GetService<ManagerCommunityData>().SetFriends(a);
            }
        }
        
        public async UniTask GET_FriendsRequests(bool globalLoading = true)
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.FriendsRequests, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get Friendship Requests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<FriendshipRequest>>
                    (result.downloadHandler.text);
            
                if(a != null) ServiceLocator.GetService<ManagerCommunityData>().SetFriendshipRequests(a);
            }
        }

        #endregion

        #region Friends POST Requests

        public class RequestId { public string requestId; }
        public class FriendId { public string friendId; }
        public class SenderId { public string senderId; }
        public class MatchRequest { public string friendId; 
            public DateTime matchDate; public int bet;
        }
        public class MatchRequestAnswer { public string requestId; }
        
        public async UniTask<bool> POST_FriendshipRequest(string _friendId)
        {
            Debug.Log("POST_FriendshipRequest: " + _friendId);
            
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new FriendId { friendId = _friendId };

            string jsonData = JsonUtility.ToJson(obj);

            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendsRequestFriendship, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success= _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_Friends(false);
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;

            return success;
        }
        
        public async UniTask<bool> POST_FriendshipConfirm(string _senderId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new SenderId { senderId = _senderId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendsRequestFriendshipConfirm, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success= _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_Friends(false);
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;

            return success;
        }
        
        public async UniTask<bool> POST_FriendshipReject(string requestId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new RequestId { requestId = requestId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendsRequestReject, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success = _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_Friends(false);
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;

            return success;
        }
        
        public async UniTask<bool> POST_DeleteFriend(string _friendId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new FriendId { friendId = _friendId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendsDelete, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success = _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_Friends(false);
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;

            return success;
        }
        
        public async UniTask<bool> POST_MatchRequest(string _friendId, 
            DateTime _matchDate, int _bet = 0)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new MatchRequest { friendId = _friendId, 
                matchDate = _matchDate, bet = _bet };
            
            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            
            Debug.Log("POST_MatchRequest: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendMatchRequest, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success = _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await UniTask.Delay(100);
                Debug.Log("GET_FriendsRequests");
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;

            return success;
        }
        
        public async UniTask<bool> POST_MatchCancel(string _requestId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new MatchRequestAnswer { requestId = _requestId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            Debug.Log("POST_MatchCancel | JSON: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendsRequestReject, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success = _generalPopupMessage.ValidationCheck(result);
            
            if (success)
            {
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;
            
            return success;
        }

        public async UniTask<bool> POST_MatchConfirm(string _requestId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new MatchRequestAnswer { requestId = _requestId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            Debug.Log("POST_MatchConfirm | JSON: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.FriendMatchConfirm, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            var success = _generalPopupMessage.ValidationCheck(result);
            
            if (success)
            {
                await GET_FriendsRequests(false);
            }
            
            _requestProcessing = false;
            
            return success;
        }
        #endregion

        #region Union GET Requests

        public async UniTask GET_Unions(bool setSearchName = false, bool globalLoading = true)
        {
            var s = CustomFilterSettings;
            var searchName = "";
            if (setSearchName) searchName = s.searchString;
            
            //var param = $"/{searchName}/{s.sliderRatingMax}/{s.sliderRatingMin}";
            var param = $"?name={searchName}&take={s.sliderRatingMax}&skip={s.sliderRatingMin}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.Unions, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param,
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get Unions Requests: " + result.downloadHandler.text);

            if (result.downloadHandler.text == "[]")
            {
                ServiceLocator.GetService<ManagerCommunityData>().SetUnions(null, s);
                return;
            } 
            
            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<Union>>
                    (result.downloadHandler.text);

                if (a != null)
                {
                    ServiceLocator.GetService<ManagerCommunityData>().SetUnions(a, s);
                }
            }
        }
        
        public async UniTask GET_UserUnionProfile(bool globalLoading = true)
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.UnionProfile, 
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: globalLoading);

            var communityData = ServiceLocator.GetService<ManagerCommunityData>();
            
            Debug.Log("Get Unions Profile Requests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var unionProfile = DataUtils.Deserialize<UnionProfile>(result.downloadHandler.text);

                communityData.SetUserUnionProfle(unionProfile);
                
                var union = DataUtils.Deserialize<Union>(result.downloadHandler.text);
                
                if (union != null)
                {
                    var emblem = await GET_UnionEmblem(union.unionId, globalLoading);
                    
                    union.unionId = unionProfile.unionId;
                    union.place = unionProfile.place;
                    union.name = unionProfile.name;
                    union.rating = unionProfile.rating;
                    union.currentMembersCount = unionProfile.unionTeams.Count;
                    union.maxMembersCount = unionProfile.maxMembersCount;
                    union.policy = unionProfile.policy;
                    union.emblem = emblem;

                    _managerPlayer.PlayerHUDs.SetUnionName(unionProfile.name);
                
                    communityData.SetUserUnion(union);
                
                    if (union.emblem != null)
                    {
                        _managerPlayer.PlayerHUDs.SetUnionEmblem(union.emblem.emblemId);
                    }
                    else
                    {
                        Debug.LogError("Failed to find uniom emblem");
                    }
                }
                else
                {
                    CancelUnionData();
                }
            }
            else
            {
                CancelUnionData();
            }
        }

        private void CancelUnionData()
        {
            var communityData = ServiceLocator.GetService<ManagerCommunityData>();
            _managerPlayer.PlayerHUDs.SetUnionName(null);
            communityData.SetUserUnionProfle(null);
            communityData.SetUserUnion(null);
        }
        
        public async UniTask<Emblem> GET_UnionEmblem(int unionId, bool globalLoading = true)
        {
            var param = $"/{unionId}";
            
            Debug.Log("GET_UnionEmblem: " + unionId);

            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.UnionEmblem, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param,
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Get Union By Id Requests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<Emblem>(result.downloadHandler.text);

                if (a != null) return a; 
            }

            return null;
        }

        public async UniTask<UnionProfile> GET_UnionProfile(int unionId)
        {
            //var param = $"?unionId={unionId}";
            var param = $"/{unionId}";
            
            Debug.Log("GET_UnionProfile: " + unionId);

            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.UnionProfile, 
                Token: _managerPlayer.PlayerData.Token, URLParam: param,
                ActiveGlobalLoading: false);
            
            Debug.Log("Get Union By Id Requests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<UnionProfile>
                    (result.downloadHandler.text);

                if (a != null) return a; 
            }

            return null;
        }
        
        public async UniTask<List<UserUnionRequest>> GET_UnionRequests(int unionId)
        {
            var param = $"/{unionId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.UnionRequests, 
                Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false,
                URLParam: param);
            
            Debug.Log("GET_UnionRequests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<UserUnionRequest>>
                    (result.downloadHandler.text);

                if (a != null) return a; 
            }

            return null;
        }

        public async UniTask<List<UserUnionRequest>> GET_UnionRequests()
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, CommunityRequests.UnionRequestsUser, 
                Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            Debug.Log("GET_UnionRequests: " + result.downloadHandler.text);

            if (result.downloadHandler.text != null)
            {
                var a = DataUtils.Deserialize<List<UserUnionRequest>>
                    (result.downloadHandler.text);

                if (a != null) return a; 
            }

            return null;
        }
        

        #endregion

        #region Union POST Requests

        public class UnionCreate { public string name;  public int recruitingPolicy; }
        public class UnionRequest { public int unionId; }
        public class UnionInviteRequest { public string[] usersId; }
        public class UnionAdminRequest { public int requestId; }
        public class UnionMemberKickRequest { public int teamUnionId; public string? replacementMasterId; }
        public class UnionMemberRequest { public int teamUnionId; public string? replacementMemberId; }
        public class UnionMemberPromoteRequest { public int teamUnionId; }
        public class UnionBuildingUpdate { public int unionBuildingId; }
        public class UnionBuildingDonate { public int unionBuildingId; public int donationSum; }
        public async UniTask<bool> POST_UnionCreate(string unionName, int policy)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionCreate { name = unionName,  recruitingPolicy = policy };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionCreate, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionRequestInviteUsers(string[] _usersId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionInviteRequest { usersId = _usersId };
            
            string jsonData = JsonUtility.ToJson(obj);
            Debug.Log("POST_UnionRequestInviteUsers JSON: " + jsonData);
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionRequestInviteUsers, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionRequestJoin(int _unionId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionRequest { unionId = _unionId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            Debug.Log("POST_UnionRequestJoin JSON: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionRequestJoin, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }

        public async UniTask<bool> POST_UnionRequestAcceptUser(int _requestId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionAdminRequest { requestId = _requestId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionRequestAccept, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;

            var success= _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_UserUnionProfile(false);
            }

            return success;
        }
        
        public async UniTask<bool> POST_UnionRequestDeleteUser(int _requestId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionAdminRequest { requestId = _requestId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionRequestDelete, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);

            _requestProcessing = false;

            var success = _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await GET_UserUnionProfile(false);
            }
            
            return success;
        }
        
        public async UniTask<bool> POST_UnionMemberDemote(int _memberTeamUnionId, 
            string _replacementMemberId = "")
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionMemberRequest { teamUnionId = _memberTeamUnionId, replacementMemberId = _replacementMemberId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionMemberDemote, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionMemberKick
            (int _memberTeamUnionId, string _replacementMasterId = "")
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionMemberKickRequest
            {
                teamUnionId = _memberTeamUnionId, 
                replacementMasterId = _replacementMasterId
            };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            Debug.Log("JSON: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionMemberKick, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionMemberPromote(int _memberTeamUnionId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            var obj = new UnionMemberPromoteRequest { teamUnionId = _memberTeamUnionId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionMemberPromote, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }

        public async UniTask<bool> POST_UnionUpdate(UnionUpdateRequest request)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;
            
            string jsonData = JsonUtility.ToJson(request);
            
            Debug.Log("POST_UnionUpdate JSON: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionUpdate, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionBuildingUpdate(int _unionBuildingId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;

            var obj = new UnionBuildingUpdate {unionBuildingId = _unionBuildingId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionUpdateBuilding, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionBuildingDonationStart(int _unionBuildingId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;

            var obj = new UnionBuildingUpdate { unionBuildingId = _unionBuildingId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionDonationStart, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionBuildingDonationAbort(int _unionBuildingId)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;

            var obj = new UnionBuildingUpdate { unionBuildingId = _unionBuildingId };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionDonationAbort, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_UnionBuildingDonate(int _unionBuildingId, int _donationSum)
        {
            if (_requestProcessing) return false;
            
            _requestProcessing = true;

            var obj = new UnionBuildingDonate { unionBuildingId = _unionBuildingId, donationSum = _donationSum };
            
            string jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, 
                CommunityRequests.UnionDonate, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);
            
            _requestProcessing = false;
            
            return _generalPopupMessage.ValidationCheck(result);
        }
        #endregion
    }
}