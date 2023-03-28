using I2.Loc;
using LazySoccer.Network.Get;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterUnion
{
    public class CommunityCenterUnionDisplay : MonoBehaviour
    {
        [Title("Image")]
        [SerializeField] private Image imageUnionEmblem;
        
        [Title("Text")]
        [SerializeField] private TMP_Text textUnionName;
        [SerializeField] private TMP_Text textUnionPlace;
        [SerializeField] private TMP_Text textUnionRating;

        public void SetInfo(GeneralClassGETRequest.Union union)
        {
            textUnionName.text = union.name;
            textUnionPlace.GetComponent<LocalizationParamsManager>().SetParameterValue("param", union.place.ToString());
            textUnionRating.GetComponent<LocalizationParamsManager>().SetParameterValue("param", union.rating.ToString());
        }

        public void SetEmblem(Sprite spriteEmblem)
        {
            if (imageUnionEmblem && spriteEmblem) imageUnionEmblem.sprite = spriteEmblem;
        }
    }
}