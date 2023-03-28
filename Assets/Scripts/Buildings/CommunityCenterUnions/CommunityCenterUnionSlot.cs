using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnions
{
    public class CommunityCenterUnionSlot : MonoBehaviour
    {
        [Title("Text")]
        [SerializeField] private TMP_Text textPlace;
        [SerializeField] private TMP_Text textUnionName;
        [SerializeField] private TMP_Text textRating;
        [SerializeField] private TMP_Text textMembers;
        
        [Title("Image")]
        [SerializeField] private Image imageEmblem;
        
        public GeneralClassGETRequest.Union unionData { get; private set; }

        public void SetInfo(GeneralClassGETRequest.Union union, int rate)
        {
            unionData = union;

            if(textPlace) textPlace.text = $"{rate}.";
            if(textUnionName) textUnionName.text = union.name;
            if(textRating) textRating.text = union.rating.ToString();
            if(textMembers) textMembers.text = union.currentMembersCount + "/50";
        }

        public void SetUnionEmblem(Sprite emblemSprite)
        {
            Debug.Log("SetUnionEmblem: " + emblemSprite);
            if(imageEmblem && emblemSprite) imageEmblem.sprite = emblemSprite;
        }
    }
}