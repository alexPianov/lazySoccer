using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using UnityEngine;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumGamesTree : MonoBehaviour
    {
        //public List<GameTier> GameTiers;
        public List<Match> Matches;

        [Header("Refs")] 
        [SerializeField] private List<StadiumGamesTreeGroup> treeGroupList;

        public async UniTask SetTournament(Tournament tournament)
        {
            var currentSeason = ServiceLocator.GetService<ManagerPlayerData>().PlayerData.Season;
            
            // GameTiers = await ServiceLocator
            //     .GetService<StadiumTypesOfRequests>().GetGameTierAll(tournament);
            //
            // var currentTier = GameTiers.Find(tier => tier.tournament == tournament);
            
            Matches = await ServiceLocator
                .GetService<StadiumTypesOfRequests>().GetCup(tournament, currentSeason);
        }

        public void SetGroup(MatchStep matchStep)
        {
            var matches=  Matches.FindAll(match => match.match == matchStep);
            var treeGroup = treeGroupList.Find(group => group.MatchStep == matchStep);
            
            treeGroup.SetMatchData(matches);
        }
    }
}