using LazySoccer.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerOfferSlot : MonoBehaviour
    {
        [Header("Text")]
        public TMP_Text textRank;
        public TMP_Text textManagerName;
        public TMP_Text textTeamName;
        public TMP_Text textOfferDate;
        public TMP_Text textOfferPrice;
        
        [Header("Image")]
        public Image imageManagerAvatar;
        public Image imageTeamEmblem;

        private MarketPlayerOfferList _master;
        public void SetMaster(MarketPlayerOfferList master)
        {
            _master = master;
        }
        
        public void SetInfo(MarketOffer offer, int rate)
        {
            textRank.text = $"{rate}.";
            textManagerName.text = offer.manager.userName;
            textTeamName.text = offer.manager.team.name;
            textOfferDate.text = DataUtils.GetDate(offer.date);
            textOfferPrice.text = $"{offer.price} LAZY";
        }

        public void SetTeamEmblem(Sprite sprite)
        {
            if (imageTeamEmblem && sprite) imageTeamEmblem.sprite = sprite;
        }
        
        public void SetManagerEmblem(Sprite sprite)
        {
            if (imageManagerAvatar && sprite) imageManagerAvatar.sprite = sprite;
        }
    }
}