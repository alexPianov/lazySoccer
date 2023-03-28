using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network.Error;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Buildings.OfficeCustomize;
using LazySoccer.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.Network.Post.TeamClassPostRequest;

namespace LazySoccer.Network
{
    public class OfficeTypesOfRequests : BaseTypesOfRequests<OfficeRequests>
    {
        [SerializeField] private OfficeDbURL dbURL;
        public override string FullURL(string URL, OfficeRequests type, string URLParam = "")
        {
            return URL + dbURL.dictionatyURL[type] + URLParam;
        }

        public override async UniTask SendMessage(OfficeRequests type) { }

        #region Office GET Requests

        public async UniTask GetTeamInfo(bool globalLoading = true)
        {
            var result = await GetRequest(_networkingManager.BaseURL, 
                OfficeRequests.Team, Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: globalLoading);

            Debug.Log("Get Statistic for player: " + result.downloadHandler.text);
            
            var a = DataUtils.Deserialize<TeamStatistic>(result.downloadHandler.text);

            _managerTeamData.SetTeamStatistic(a);

            if(a.emblem != null) _managerPlayer.PlayerHUDs.SetEmblem(a.emblem.emblemId);
            _managerPlayer.PlayerData.TeamId = a.teamId;

            await _teamTypesOfRequests.GET_TeamRewards(globalLoading);
            await _teamTypesOfRequests.GET_TeamTrophies(globalLoading);
        }

        public async UniTask<TeamStatistic> GetTeamStatistic(int teamId)
        {
            var param = $"/{teamId}";
            
            var result = await GetRequest
            (_networkingManager.BaseURL, 
                OfficeRequests.Team, 
                Token: _managerPlayer.PlayerData.Token,
                URLParam: param, ActiveGlobalLoading: false);
            
            Debug.Log("Get Statistic for player: " + result.downloadHandler.text);
            
            return DataUtils.Deserialize<TeamStatistic>(result.downloadHandler.text);
        }
        
        public async UniTask GetFinancialStatistics(OfficeRequests type, bool globalLoading = true)
        {
            var result = await GetRequest
            (_networkingManager.BaseURL, type, Token: _managerPlayer.PlayerData.Token, 
                ActiveGlobalLoading: globalLoading);
            
            var a = DataUtils.Deserialize<List<FinancialStatistics>>(result.downloadHandler.text);

            if (type == OfficeRequests.Season)
            {
                _managerTeamData.SetSeasonBalance(a);
            }

            if (type == OfficeRequests.PastDay)
            {
                _managerTeamData.SetPastDayBalance(a);
            }
        }
        

        #endregion

        #region Office POST Requests

        public async UniTask<bool> POST_ChangeEmblem(OfficeRequests requestType, int id, bool globalLoading = true)
        {
            Debug.Log("POST_ChangeEmblem: " + requestType + " to: " + id);

            var obj = new Emblem()
            {
                emblemId = id,
                type = "Cola",
                colors = new string[] { "Blue" }
            };

            var jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, requestType, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: globalLoading);

            return _generalPopupMessage.ValidationCheck(result);
        }
        
        public async UniTask<bool> POST_ChangeTeamName(OfficeRequests requestType, string teamName, string teamNameShort)
        {
            Debug.Log("POST_ChangeTeamName: " + requestType + " to: " + teamName);

            var obj = new ChangeTeamName()
            {
                name = teamName,
                shortName = teamNameShort
            };

            var jsonData = JsonUtility.ToJson(obj);
            
            var result = await PostRequest(_networkingManager.BaseURL, requestType, 
                JSON: jsonData, Token: _managerPlayer.PlayerData.Token, ActiveGlobalLoading: true);

            return _generalPopupMessage.ValidationCheck(result);
        }
        

        #endregion
    }
}