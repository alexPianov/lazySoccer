using Sirenix.OdinInspector;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings
{
    [CreateAssetMenu(fileName = "Economy", menuName = "Building/Economy", order = 1)]
    public class Economy : ScriptableObject
    {
        [Title("Medical Center")]
        public int instantHeal;
        public int instantExam;
        
        [Title("Fitness Center")]
        public int instantForm;
        public int instantCharge;
        
        [Title("Training Center")]
        public int instantTraining;

        [Title("Office")] 
        public int changeTeamName;

        [Title("Settings")] 
        public int changeNickname;
    }
}