using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.SceneLoading;
using LazySoccer.SceneLoading.PlayerData.Enum;
using TMPro;
using UnityEngine.UI;
using Utils;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.PlayerData.Enum.PlayerStatus;

namespace LazySoccer.Table
{
    public class SlotPlayerFactory : MonoBehaviour
    {
        [SerializeField] private Transform contentContainer;
        [SerializeField] private GameObject slotPlayerPrefab;
        [SerializeField] private Scrollbar scrollbar;

        private List<SlotPlayer> playersTable = new(); 
        public List<SlotPlayer> GetTable()
        {
            return playersTable;
        }

        protected virtual List<TeamPlayer> GetTeamPlayers(TeamPlayerStatus requiredStatus = TeamPlayerStatus.None)
        {
            return ServiceLocator.GetService<ManagerTeamData>().GetTeamPlayers(requiredStatus);
        }

        public async UniTask<int> GetTeamPlayersByTeamId(int teamId)
        {
            ClearTable();
            var players = await ServiceLocator.GetService<TeamTypesOfRequests>().GET_TeamPlayers(teamId);
            CreatePlayersList(players);
            return players.Count;
        }

        protected List<SlotPlayer> CreatePlayersList(List<TeamPlayer> players, bool nonRecord = false)
        {
            if (players == null || players.Count == 0) return null;
            
            ClearTable();
            
            var slots = new List<SlotPlayer>();
            
            foreach (var player in players)
            {
                var slot = CreateSlot(player, slotPlayerPrefab, nonRecord)
                    .GetComponent<SlotPlayer>();
                
                slots.Add(slot);
            }

            //await CancelScrollbar();
            
            return slots;
        }

        protected List<SlotPlayer> CreatePlayersList(TeamPlayerStatus requiredStatus = TeamPlayerStatus.None, bool nonRecord = false)
        {
            var players = GetTeamPlayers(requiredStatus);

            return CreatePlayersList(players, nonRecord);
        }

        public async UniTask CancelScrollbar()
        {
            if (scrollbar)
            {
                await UniTask.Delay(40);
                scrollbar.value = 0;
            }
        }

        private int _playerNumber;
        protected GameObject CreateSlot(TeamPlayer teamPlayer, GameObject prefab = null, bool nonRecord = false)
        {
            if (prefab == null)
            {
                prefab = slotPlayerPrefab;
            }
            
            var slot = Instantiate(prefab, contentContainer);

            if (teamPlayer == null) return slot;
                
            if (slot.TryGetComponent<SlotPlayer>(out var slotPlayer))
            {
                _playerNumber++;
                slotPlayer.SetInfo(teamPlayer, _playerNumber);
                
                if(!nonRecord) playersTable.Add(slotPlayer);
            }

            return slot;
        }

        private void ClearTable()
        {
            _playerNumber = 0;

            for (int i = 0; i < playersTable.Count; i++)
            {
                if (playersTable[i].gameObject)
                {
                    Destroy(playersTable[i].gameObject);
                } 
            }
            
            playersTable.Clear();
        }
    }
}
