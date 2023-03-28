using LazySoccer.Table;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Centers
{
    public class CenterMemberListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddToCenter);
        }

        private CenterMemberList _memberList;
        public void SetTable(CenterMemberList table)
        {
            _memberList = table;
        }

        private void AddToCenter()
        {
            _memberList.AddMember(GetComponent<SlotPlayer>());
        }
    }
}