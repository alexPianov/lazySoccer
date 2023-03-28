using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseTextLink<T> : MonoBehaviour, IPointerClickHandler
{
    [InfoBox("Choose true for StatusGame or false - Event")]
    [SerializeField] private bool isStatusOrEvent;

    [SerializeField, ShowIf(nameof(isStatusOrEvent))] public T _status;
    [SerializeField, HideIf(nameof(isStatusOrEvent))] private UnityEvent _event;
    [SerializeField]
    private TMP_Text text;
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, eventData.pressEventCamera);
        if (linkIndex == -1)
            return;
        TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
        string selectedLink = linkInfo.GetLinkID();
        if (selectedLink != "")
        {
            if (isStatusOrEvent)
                ChangeEvent();
                //ServiceLocator.GetService<T>().StatusAction = _status;
            else
                _event?.Invoke();
        }
    }
    public virtual void ChangeEvent()
    {

    }
}
