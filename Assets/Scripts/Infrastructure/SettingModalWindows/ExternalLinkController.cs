using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LazySoccer.SceneLoading.TextLink
{
    public class ExternalLinkController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private string link;
        
        [SerializeField]
        private TMP_Text text;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, eventData.pressEventCamera);
            
            if (linkIndex == -1) return;
            
            TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
            
            string selectedLink = linkInfo.GetLinkID();
            
            Debug.Log("Selected Link: " + selectedLink);
            
            if (selectedLink != "")
            {
                Application.OpenURL(link);
            }
        }
    }
}