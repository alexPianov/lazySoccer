using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Error;
using LazySoccer.Network.Get;
using LazySoccer.Utils;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    [Serializable]
    public class TeamTypesOfRequests : BaseTypesOfRequests<TeamRequests>
    {
        [SerializeField] private GetTeamDbURL dbURL;
        public override string FullURL(string URL, TeamRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        #region Team GET Requests

        public async UniTask<bool> GET_TeamPlayers(bool globalLoading = true)
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                TeamRequests.Players, 
                Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            Debug.Log("Team Players: " + result.downloadHandler.text);
            
            var a = DataUtils.Deserialize<List<TeamPlayer>>(result.downloadHandler.text);
            
            _managerTeamData.SetTeamPlayers(a);

            return a.Count > 1 && a.Count < 30;
        }
        
        public async UniTask<List<TeamPlayer>> GET_TeamPlayers(int teamId, bool globalLoading = false)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{teamId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.Players, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: globalLoading);

            Debug.Log("Get Statistic for player: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamPlayer>>(result.downloadHandler.text);
        }

        public async UniTask GET_TeamRewards(bool globalLoading = true)
        {
            var teamId = _managerPlayer.PlayerData.TeamId;
            var teamReward = await _teamTypesOfRequests
                .GET_TeamRewards(teamId, globalLoading);
            _managerTeamData.SetTeamRewards(teamReward);
        }
        
        public async UniTask GET_TeamTrophies(bool globalLoading = true)
        {
            var teamId = _managerPlayer.PlayerData.TeamId;
            var teamTrophies = await _teamTypesOfRequests.GET_TeamTrophies(teamId, globalLoading);
            _managerTeamData.SetTeamTrophies(teamTrophies);
        }

        public async UniTask<List<TeamReward>> GET_TeamRewards(int teamId, bool globalLoading = false)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{teamId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.Rewards, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: globalLoading);

            Debug.Log("GET_TeamRewards: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamReward>>(result.downloadHandler.text);
        }

        public async UniTask<List<TeamTrophy>> GET_TeamTrophies(int teamId, bool globalLoading = true)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{teamId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.Trophies, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param, ActiveGlobalLoading: globalLoading);

            Debug.Log("GET_TeamTrophies: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamTrophy>>(result.downloadHandler.text);
        }
        
        public async UniTask<TeamPlayerStatistics> GET_TeamPlayerStatistics(int playerId)
        {
            //var param = $"?playerId={playerId}";
            var param = $"/{playerId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, TeamRequests.Player, 
                Token: _managerPlayer.PlayerData.Token, 
                URLParam: param, ActiveGlobalLoading: false);

            var a = DataUtils.Deserialize<TeamPlayerStatistics>(result.downloadHandler.text);

            if (a == null) Debug.LogError("Failed to get team player statistics");
            
            return a;
        }
        
        public async UniTask<List<TeamPlayerReward>> GET_TeamPlayerRewards(int playerId)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{playerId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.PlayerRewards, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);

            Debug.Log("GET_TeamPlayerRewards: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamPlayerReward>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<TeamPlayerTrophy>> GET_TeamPlayerTrophies(int playerId)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{playerId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.PlayerTrophies, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);

            Debug.Log("GET_TeamPlayerTrophies: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamPlayerTrophy>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<TeamPlayerTransfer>> GET_TeamPlayerTransfers(int playerId)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{playerId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.PlayerTransfers, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);

            Debug.Log("Get Statistic for player: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamPlayerTransfer>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<Match>> GET_TeamPlayerMatches(int playerId, int seasonId, int take = 100, int skip = 0)
        {
            //var param = $"?playerId={playerId}";
            var param = $"/{playerId}/{seasonId}/{take}/{skip}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.PlayerGameStats, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);

            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        
        public async UniTask<List<TeamPlayerSkill>> GET_PlayerSkill(int playerId)
        {
            //var param = $"?teamId={teamId}";
            var param = $"/{playerId}";
            
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.PlayerSkills, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param,
                ActiveGlobalLoading: false);

            Debug.Log("GET_PlayerSkill: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<TeamPlayerSkill>>(result.downloadHandler.text);
        }

        
        public async UniTask GET_Season(bool globalLoading = true)
        {
            var result = await GetRequest(_networkingManager.BaseURL, 
                TeamRequests.Season, 
                Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: globalLoading);

            Debug.Log("GET_Season: " + result.downloadHandler.text);
            
            var season = DataUtils.Deserialize<Season>(result.downloadHandler.text);

            _managerPlayer.PlayerData.Season = season.seasonId;
        }
        
        #endregion
        
        #region Team POST Requests

        public class UniformArray
        {
            public Uniform[] uniforms;
        }
        
        public async UniTask<bool> POST_ChangeUniform(TeamRequests requestType, 
            List<Uniform> uniforms)
        {
            Debug.Log("POST_ChangeUniform: " + requestType);

            var obj = new UniformArray()
            {
                uniforms = uniforms.ToArray()
            };

            var jsonData = DataUtils.Serialize(obj);
            
            Debug.Log("JSON Data: " + jsonData);
            
            var result = await PostRequest(_networkingManager.BaseURL, requestType, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: true);

            return _generalPopupMessage.ValidationCheck(result);
        }

        #endregion
    }
}