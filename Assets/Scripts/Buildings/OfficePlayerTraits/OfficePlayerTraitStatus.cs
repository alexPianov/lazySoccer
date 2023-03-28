using System;
using I2.Loc;
using LazySoccer.SceneLoading.PlayerData.Enum;
using Scripts.Infrastructure.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    public class OfficePlayerTraitStatus : MonoBehaviour
    {
        public TMP_Text playerStatus;
        public Image statusImage;

        public void UpdateStatus(TeamPlayerStatus status)
        {
            if(playerStatus.GetComponent<Localize>())
            {
                Debug.Log("UpdateStatus: " + status);
                playerStatus.GetComponent<Localize>().SetTerm("TeamPlayer-Status-" + status);
            }
            
            statusImage.sprite = ServiceLocator.GetService<ManagerSprites>()
                .GetStatusSprites().Get(status);
        }

        private int DaysRemain(TeamPlayer player)
        {
            if (player.status == TeamPlayerStatus.Healing)
            {
                
            }
            
            if (player.status == TeamPlayerStatus.RestoringForm)
            {
                
            }
            
            if (player.status == TeamPlayerStatus.OnExamination)
            {
                
            }

            return 0;
        }
    }
}