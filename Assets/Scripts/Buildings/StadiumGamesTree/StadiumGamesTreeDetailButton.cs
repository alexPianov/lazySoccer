using System;
using LazySoccer.Network.Get;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.Stadium
{
    public class StadiumGamesTreeDetailButton : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private StadiumGamesTreeDetail treeDetail;
        [SerializeField] private Button buttonOpen;
        
        [Header("Type")]
        [SerializeField] private AdditionClassGetRequest.MatchStep MatchStep;

        private void Start()
        {
            buttonOpen.onClick.AddListener(Open);
        }

        private void Open()
        {
            treeDetail.OpenGroup(MatchStep);
        }
    }
}