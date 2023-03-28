using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketTransferDeadlinePlanner : MonoBehaviour
    {
        [Header("Date")]
        [SerializeField] private CalendarController calendarController;
        [SerializeField] private GameObject panel;

        private DateTime currentDay;
        private bool save;

        public void SaveDate()
        {
            save = true;
        }

        public async UniTask<DateTime> GetDeadlineDate()
        {
            panel.SetActive(true);
            calendarController.UpdateCalendar();
            
            save = false;
            await UniTask.WaitUntil(() => save);
            save = false;
            
            panel.SetActive(false);
            return calendarController.GetDate();
        }
    }
}