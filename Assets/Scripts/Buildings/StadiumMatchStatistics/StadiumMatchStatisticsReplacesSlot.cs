using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Sprites;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatchStatistics
{
    public class StadiumMatchStatisticsReplacesSlot : MonoBehaviour
    {
        [SerializeField] private TMP_Text textNamePlayerFirst;
        [SerializeField] private TMP_Text textNamePlayerSecond;
        
        [SerializeField] private TMP_Text textGoalsPlayerFirst;
        [SerializeField] private TMP_Text textGoalsPlayerSecond;
        
        [SerializeField] private TMP_Text textPositionPlayerFirst;
        [SerializeField] private TMP_Text textPositionPlayerSecond;
        
        public void SetInfo(PlayerGameStats substiturePlayer, PlayerGameStats relegatedPlayer)
        {
            var playerFirst = substiturePlayer.playerName;
            var playerSecond = relegatedPlayer.playerName;
            
            textNamePlayerFirst.text = playerFirst;
            textNamePlayerSecond.text = playerSecond;

            textGoalsPlayerFirst.text = substiturePlayer.goalScored.ToString();
            textGoalsPlayerSecond.text = relegatedPlayer.goalScored.ToString();
            
            textPositionPlayerFirst.text = DataUtils.GetSpecialityAbbreviation(substiturePlayer.playerPosition);
            textPositionPlayerSecond.text = DataUtils.GetSpecialityAbbreviation(relegatedPlayer.playerPosition);
        }
    }
}