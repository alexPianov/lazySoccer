using LazySoccer.Table;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberListenerLocal : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddToCenter);
        }

        private CenterTeamTable _centerTeamTable;
        public void SetTable(CenterTeamTable table)
        {
            _centerTeamTable = table;
        }

        private void AddToCenter()
        {
            Debug.Log("AddToCenter");
            _centerTeamTable.SlotPlayerLocal = GetComponent<SlotPlayer>();
        }
    }
}