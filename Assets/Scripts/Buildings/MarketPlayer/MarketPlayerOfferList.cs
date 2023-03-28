using System;
using System.Collections.Generic;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.SceneLoading.Infrastructure.Centers;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.MarketTransfers
{
    public class MarketPlayerOfferList : CenterSlotList
    {
        [SerializeField] private MarketPlayer _marketPlayer;

        public void UpdateList()
        {
            CreateSlots(_marketPlayer.Offers);
        }

        private void CreateSlots(List<GeneralClassGETRequest.MarketOffer> offers)
        {
            DestroyAllSlots();
            
            if(offers == null || offers.Count == 0) return;

            for (int i = 0; i < offers.Count; i++)
            {
                var item = offers[i];

                var slotInstance = CreateSlot();

                if (slotInstance.TryGetComponent(out MarketPlayerOfferSlot offerSlot))
                {
                    offerSlot.SetMaster(this);
                    offerSlot.SetInfo(item, i + 1);
                    var emblemId = item.manager.team.teamEmblem.emblemId;
                    offerSlot.SetTeamEmblem(GetEmblemSprite(emblemId));
                    offerSlot.SetManagerEmblem(null);
                }
            }
        }
    }
}