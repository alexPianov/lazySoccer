using I2.Loc;
using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.Table
{
    public class SlotPlayerTrophy : MonoBehaviour
    {
        [Title("Text")]
        public TMP_Text trophyName;
        public TMP_Text trophyDescription;
        
        [Title("Image")]
        public Image trophyImage;

        public void SetInfo(AdditionClassGetRequest.TeamPlayerTrophy trophy, Sprite sprite = null)
        {
            //trophyDescription.text = trophy.description;
            
            if(trophyName) trophyName.GetComponent<Localize>()
                .SetTerm($"TeamPlayer-Trophy-{trophy.trophy}-{trophy.trophyFor}");
            
            if(trophyDescription) trophyDescription.GetComponent<Localize>()
                .SetTerm($"TeamPlayer-Trophy-{trophy.trophy}-{trophy.trophyFor}-Description");
            
            if(sprite) trophyImage.sprite = sprite;
        }
    }
}