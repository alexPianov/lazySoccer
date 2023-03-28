using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPopupManager : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer popup;
    [SerializeField]
    private Animator popupAnimator;
    [SerializeField]
    private Sprite[] popupSprites;

    public void ActivatePopup(int statusCode)
    {
        popup.sprite = popupSprites[statusCode - 1];
        popupAnimator.SetBool("isActivated", true);
    }
    public void DeactivatePopup()
    {
        popupAnimator.SetBool("isActivated", false);
    }
}
