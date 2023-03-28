using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading.Buildings.Stadium;
using LazySoccer.Utils;
using Newtonsoft.Json;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Network
{
    public class StadiumTypesOfRequests: BaseTypesOfRequests<StadiumRequests>
    {
        [SerializeField] private StadiumDbURL dbURL;
        public override string FullURL(string URL, StadiumRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        public override async UniTask SendMessage(StadiumRequests type) { }

        #region Stadium GET Requests
        
        public async UniTask<List<StadiumPositions>> GetPositions()
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.Positions, 
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetPositions: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<StadiumPositions>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<Match>> GetMatches(StadiumMatches.MatchType matchType, Tournament tournament, int seasonId = 1, int skip = 0, int take = 500)
        { 
            string param = null;  

            var requests = StadiumRequests.UserMatchesHistory;
            Debug.Log(skip + " " + take);
            
            if (matchType == StadiumMatches.MatchType.History)
            {
                requests = StadiumRequests.UserMatchesHistory;
                param = $"/{(int)tournament}/{seasonId}/{skip}/{take}";
                //param = $"/{skip}/{take}";
            }
            
            if (matchType == StadiumMatches.MatchType.Upcoming)
            {
                requests = StadiumRequests.UserMatchesUpcoming;
                param = $"/{skip}/{take}";
            }
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                requests, 
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetMatchesOld: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<Match>> GetMatches(StadiumMatches.MatchType matchType, int skip = 0, int take = 30, int? seasonId = null)
        { 
            string param = null;

            var requests = StadiumRequests.UserMatchesHistory;
            Debug.Log(skip + " " + take);
            
            if (matchType == StadiumMatches.MatchType.History)
            {
                requests = StadiumRequests.UserMatchesHistory;
                param = $"/{skip}/{take}";
                
                if (seasonId != null)
                {
                    param = $"/{seasonId}/{skip}/{take}";
                }
            }
            
            if (matchType == StadiumMatches.MatchType.Upcoming)
            {
                requests = StadiumRequests.UserMatchesUpcoming;
                param = $"/{skip}/{take}";
            }
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                requests, 
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetMatches: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<GameTier>> GetGameTierAll(Tournament tournament)
        { 
            var param = $"/{(int)tournament}";

            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.GameTierAll, 
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetGameTierAll: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<GameTier>>(result.downloadHandler.text);
        }

        public async UniTask<List<NationalCountries>> GetNationalCountries()
        { 
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.NationalCountries,  
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetNationalCountries: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<NationalCountries>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<DivisionTournament>> GetDivisionTournament(Tournament tournament, int divisionId, int seasonId = 1)
        { 
            var param = $"/{(int)tournament}/{divisionId}/{seasonId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.DivisionTornament,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetDivisionTournament: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<DivisionTournament>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<Match>> GetNationalLeague(int divisionId, int seasonId, int skip = 0, int take = 500)
        { 
            var param = $"/{divisionId}/{seasonId}/{skip}/{take}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.NationalLeague,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetNationalLeague: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<DivisionTournament>> GetNationalLeagueStatistic(int divisionId, int seasonId)
        { 
            var param = $"/{divisionId}/{seasonId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.NationalLeagueStatistic,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetNationalLeagueStatistic: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<DivisionTournament>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<Match>> GetNationalCup(int countryId, int seasonId, int skip = 0, int take = 500)
        { 
            var param = $"/{countryId}/{seasonId}/{skip}/{take}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.NationalCup,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetNationalCup: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<DivisionTournament>> GetNationalCupStatistics(int countryId, int seasonId = 1)
        { 
            var param = $"/{countryId}/{seasonId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.NationalCupStatistics,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetNationalCupStatistics: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<DivisionTournament>>(result.downloadHandler.text);
        }

        public async UniTask<List<Match>> GetCup(Tournament tournament, int seasonId = 1)
        { 
            var param = $"/{(int)tournament}/{seasonId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.Cup,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetCup: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<Match>>(result.downloadHandler.text);
        }
        
        public async UniTask<List<DivisionTournament>> GetCupStatistics(Tournament tournament, int seasonId = 1)
        { 
            var param = $"/{(int)tournament}/{seasonId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.CupStatistics,  
                URLParam: param,
                Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: false);
            
            Debug.Log("GetCupStatistics: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<List<DivisionTournament>>(result.downloadHandler.text);
        }

        
        public async UniTask<MatchStatistics> GetMatch(string gameId)
        {
            var param = $"/{gameId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.Match, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param, ActiveGlobalLoading: false);
            
            Debug.Log("GetMatch: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<MatchStatistics>(result.downloadHandler.text);
        }
        
        public async UniTask<string> GetMatchTime(string gameId)
        {
            var param = $"/{gameId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                StadiumRequests.MatchTime, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param, ActiveGlobalLoading: false);
            
            Debug.Log("GetMatchTime: " + result.downloadHandler.text);

            return result.downloadHandler.text;
        }

        #endregion
        

        public async UniTask<bool> PostMatchLineup(MatchConfiguration matchConfiguration)
        {
            var json = JsonConvert.SerializeObject(matchConfiguration);
            
            Debug.Log("POST_PostMatchLineup JSON: " + json);
            
            var result = await PostRequest(_networkingManager.BaseURL,
                StadiumRequests.SetMatchLineup,
                JSON: json, Token: _managerPlayer.PlayerData.Token,
                ActiveGlobalLoading: false);

            var success = _generalPopupMessage.ValidationCheck(result);

            if (success)
            {
                await _teamTypesOfRequests.GET_TeamPlayers();
            }
            
            return success;
        }
    }
}