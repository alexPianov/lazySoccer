using LazySoccer.Utils;
using Scripts.Infrastructure.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.AdditionClassGetRequest;

namespace LazySoccer.Table
{
    public class SlotPlayerTransfer: MonoBehaviour
    {
        [Title("Text")]
        public TMP_Text transferSeller;
        public TMP_Text transferBuyer;
        public TMP_Text transferPrice;
        public TMP_Text transferDate;
        
        [Title("Image")]
        public Image transferSellerImage;
        public Image transferBuyerImage;

        public void SetInfo(TeamPlayerTransfer playerTransfer)
        {
            transferSeller.text = playerTransfer.seller.name;
            transferBuyer.text = playerTransfer.buyer.name;
            transferPrice.text = $"{playerTransfer.price} LAZY";
            transferDate.text = DataUtils.GetDate(playerTransfer.deadLineDate);
            
            transferSellerImage.sprite = ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(playerTransfer.seller.teamEmblem.emblemId);
            
            transferBuyerImage.sprite = ServiceLocator.GetService<ManagerSprites>()
                .GetTeamSprite(playerTransfer.buyer.teamEmblem.emblemId);
        }
        
    }
}