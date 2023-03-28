using LazySoccer.SceneLoading.Infrastructure.Audio;
using Scripts.Infrastructure.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UiUtilsExpand : MonoBehaviour
{
    public GameObject expandingObject;
    public GameObject closingObject;
    public bool expandButton; 
    
    [SerializeField]
    private UiUtilsExpandGroup _expandGroup;
    private ManagerAudio _managerAudio;
    
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ActiveWithSound); 
        _managerAudio = ServiceLocator.GetService<ManagerAudio>();
    }

    public void SetExpandGroup(UiUtilsExpandGroup expandGroup)
    {
        _expandGroup = expandGroup;
    }

    private void ActiveWithSound()
    {
        if(_managerAudio) _managerAudio.PlaySound(ManagerAudio.AudioSound.Next);
        Active();
    }

    public void ActiveSimple()
    {
        Active();
    }

    protected virtual void Active()
    {  
        if(_expandGroup) _expandGroup.CloseAll();
        if(expandingObject) expandingObject.SetActive(true);
        if(closingObject) closingObject.SetActive(false);
    }

    public void CloseExpandContainer()
    {
        if (expandButton)
        {
            if(expandingObject) expandingObject.SetActive(false);
            if(closingObject) closingObject.SetActive(true);
        }
    }
}
