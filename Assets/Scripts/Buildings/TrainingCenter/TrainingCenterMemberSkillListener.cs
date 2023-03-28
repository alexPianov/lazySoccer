using LazySoccer.Table;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.TrainingCenter
{
    public class TrainingCenterMemberSkillListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(AddToCenter);
        }

        private TrainingCenterMemberSkill _master;
        public void SetMaster(TrainingCenterMemberSkill table)
        {
            _master = table;
        }

        private void AddToCenter()
        {
            _master.PickSkill(GetComponent<SlotPlayerSkill>().Skill);
        }
    }
}