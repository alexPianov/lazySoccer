using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.SceneLoading.Buildings.OfficeEmblem.EmblemSprites;

namespace LazySoccer.SceneLoading.Infrastructure.Customize
{
    public class CustomizeTeamEmblemSlot : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image selectDot;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Pick);
        }

        protected CustomizeTeamEmblemFactory _emblemFactory;
        public EmblemSprite Emblem { get; private set; }
    
        public void SetEmblemFactory(CustomizeTeamEmblemFactory emblemFactory)
        {
            _emblemFactory = emblemFactory;
        }

        public void SetEmblem(EmblemSprite emblem)
        {
            Emblem = emblem;
            image.sprite = emblem.emblemSprite;
        }

        public void SelectDot(bool state)
        {
            selectDot.enabled = state;
        }

        public void Pick()
        {
            _emblemFactory.PickSlot(this);
        }
    }
}