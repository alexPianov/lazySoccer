using I2.Loc;
using LazySoccer.SceneLoading.Buildings.OfficeEmblem;
using LazySoccer.SceneLoading.PlayerData.Enum;
using LazySoccer.Table;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class PlayerInformation : SlotPlayer
    {
        [Title("Refs")]
        [SerializeField] private TMP_Text playerAgeFull;
        [SerializeField] private TMP_Text playerAgeName;
        [SerializeField] private TMP_Text playerTeam;
        [SerializeField] private Image playerTeamEmblem;
        
        [Title("Player Data")]
        [SerializeField] private OfficePlayer pickedPlayer;

        public void UpdatePlayerInfo()
        {
            SetInfo(pickedPlayer.CurrentPlayer);

            var age = pickedPlayer.CurrentPlayer.age;
            
            playerAgeFull.GetComponent<LocalizationParamsManager>().SetParameterValue("param", age.ToString());
            playerAgeName.GetComponent<Localize>().SetTerm("TeamPlayer-Age-" + PlayerStatus.GetAgeAsText(age));

            playerTeam.text = pickedPlayer.CurrentPlayer.name;
            
            var emblemId = pickedPlayer.TeamStatistic.teamStatisticData.emblem.emblemId;
            
            playerTeamEmblem.sprite = ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(emblemId);
        }
    }
}