using System.Collections.Generic;
using System.Linq;
using LazySoccer.SceneLoading.Buildings.CommunityCenterUnions;
using LazySoccer.SceneLoading.UI;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionGroupButtons : MonoBehaviour
    {
        [SerializeField]
        private List<GroupButtonsCommunityCenterUnion> groupButtons = new();

        public void RefreshGroupButtons(MemberType memberType)
        {
            DisableAllButtons();
            
            if (memberType == MemberType.Master)
            {
                ActiveButton(StatusCommunityCenterUnion.Members, true);
                ActiveButton(StatusCommunityCenterUnion.Buildings, true);
                ActiveButton(StatusCommunityCenterUnion.Requests, true);
                ActiveButton(StatusCommunityCenterUnion.Settings, true);
            }
            
            if (memberType == MemberType.Officer)
            {
                ActiveButton(StatusCommunityCenterUnion.Members, true);
                ActiveButton(StatusCommunityCenterUnion.Buildings, true);
                ActiveButton(StatusCommunityCenterUnion.Requests, true);
                ActiveButton(StatusCommunityCenterUnion.Settings, false);
            }
            
            if (memberType == MemberType.Simple)
            {
                ActiveButton(StatusCommunityCenterUnion.Members, true);
                ActiveButton(StatusCommunityCenterUnion.Buildings, true);
                ActiveButton(StatusCommunityCenterUnion.Requests, false);
                ActiveButton(StatusCommunityCenterUnion.Settings, false);
            }
        }

        public void DisableAllButtons()
        {
            foreach (var groupButton in groupButtons)
            {
                groupButton.gameObject.SetActive(false);
            }
        }
        
        public void ActiveButton(StatusCommunityCenterUnion status, bool state)
        {
            foreach (var groupButton in groupButtons)
            {
                if (groupButton._action == status)
                {
                    groupButton.gameObject.SetActive(state);
                }
            }
        }
    }
}