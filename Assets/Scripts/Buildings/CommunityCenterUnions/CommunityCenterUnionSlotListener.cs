using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionSlotListener : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Active);
        }
        
        private CommunityCenterUnion.CommunityCenterUnion _unionDisplay;
        
        public void SetUnionDisplay(CommunityCenterUnion.CommunityCenterUnion union)
        {
            _unionDisplay = union;
        }

        private void Active()
        {
            _unionDisplay.ShowUnion(GetComponent<CommunityCenterUnionSlot>().unionData);
        }
    }
}