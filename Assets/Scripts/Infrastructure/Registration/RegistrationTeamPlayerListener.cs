using LazySoccer.Table;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Registration
{
    public class RegistrationTeamPlayerListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Select);
        }

        private RegistrationTeamPlayersList _playersList;
        public void SetSource(RegistrationTeamPlayersList table)
        {
            _playersList = table;
        }

        private void Select()
        {
            _playersList.SelectPlayer(GetComponent<SlotPlayer>());
        }
    }
}