using __Project.Scripts.Scroll;
using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;

namespace __Project.Scripts.Scroll
{
    public class LocationStrListBox : ListBox
    {
        [SerializeField]
        private Image _contentImage;
        [SerializeField]
        private TMPro.TMP_Text _contentText;

        protected override void UpdateDisplayContent(object content)
        {
            var colorString = (LocationCard)content;

            _contentImage.sprite = colorString.Image;
            _contentText.text = colorString.Name;
        }
    }
}
