using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamEmblemSlotFromPopup : CustomizeTeamEmblemSlot
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(PickAndClose);
        }

        public void PickAndClose()
        {
            Debug.Log("PickAndClose");
            base.Pick();
            _emblemFactory.TeamEmbem.ClosePopupTable();
        }
    }
}