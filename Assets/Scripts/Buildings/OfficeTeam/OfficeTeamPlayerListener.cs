using System;
using LazySoccer.Network;
using LazySoccer.Table;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.MedicalCenter
{
    [RequireComponent(typeof(Button))]
    public class OfficeTeamPlayerListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddAsPickedPlayer);
        }

        private OfficeTeamTable _teamTable;
        public void SetTeamTable(OfficeTeamTable table)
        {
            _teamTable = table;
        }

        private void AddAsPickedPlayer()
        {
            _teamTable.PlayerDetails(GetComponent<SlotPlayer>());
        }
    }
}