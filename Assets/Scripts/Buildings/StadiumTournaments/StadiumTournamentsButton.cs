using System;
using LazySoccer.SceneLoading.Buildings.Stadium;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace __Project.Scripts.Buildings.StadiumDivision
{
    public class StadiumTournamentsButton : MonoBehaviour
    {
        [SerializeField] private Tournament tournament;
        [SerializeField] private StadiumTournaments stadiumDivisionList;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Open);
        }

        private void Open()
        {
            stadiumDivisionList.OpenTournament(tournament);
        }
    }
}