using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;
using static LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate.CommunityCenterUnionCreateBuildings;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnionCreate
{
    public class CommunityCenterUnionCreateBuilding : MonoBehaviour
    {
        [SerializeField] private Button buttonAllow;
        [SerializeField] private TMP_Text textButton;
        [SerializeField] private Sprite spriteIsInvited;
        public UnionBuildingDonation CurrentBuilding;
        
        private Sprite spriteNotInvited;
        private Image imageButtonInvite;

        private void Awake()
        {
            imageButtonInvite = buttonAllow.GetComponent<Image>();
            spriteNotInvited = imageButtonInvite.sprite;
            buttonAllow.onClick.AddListener(OnClickAllow);
        }
        
        private CommunityCenterUnionCreateBuildings _buildings;
        public void SetMaster(CommunityCenterUnionCreateBuildings buildings)
        {
            _buildings = buildings;
        }

        private void OnClickAllow()
        {
            if (_buildings.CurrentBuilding == CurrentBuilding)
            {
                _buildings.PickBuilding(UnionBuildingDonation.None);
                return;
            }
            
            EnableDonation(true);
            _buildings.PickBuilding(CurrentBuilding);
        }

        public void EnableDonation(bool state)
        {
            if (state)
            {
                textButton.text = "Cancel donation";
                imageButtonInvite.sprite = spriteIsInvited;
                textButton.color = DataUtils.GetColorFromHex("A455FF");
            }
            else
            {
                textButton.text = "Allow donation";
                imageButtonInvite.sprite = spriteNotInvited;
                textButton.color = DataUtils.GetColorFromHex("FFFFFF");
            }
        }

        public void ActiveButton(bool state)
        {
            buttonAllow.interactable = state;
        }
    }
}