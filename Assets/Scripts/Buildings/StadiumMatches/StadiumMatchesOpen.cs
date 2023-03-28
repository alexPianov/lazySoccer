using System;
using LazySoccer.Network.Get;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumMatchesOpen : UiUtilsExpand
    {
        [SerializeField] private StadiumMatchesUpdate _matchesOldUpdate;
        [SerializeField] private AdditionClassGetRequest.Tournament Tournament;

        protected override void Active()
        {
            _matchesOldUpdate.UpdateMatch(Tournament);
            base.Active();
        }
    }
}