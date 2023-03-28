using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

public class OfficeStatisticsTrophiesSlot : MonoBehaviour
{
    public TMP_Text trophyName;
    public TMP_Text trophyDescription;
    public Image trophyImage;

    public void SetTrophyInfo(TeamTrophy trophy, Sprite sprite = null)
    {
        Debug.Log("Set player trophy: " + trophy.trophySTR);
        
        if(trophyName) trophyName.GetComponent<Localize>()
            .SetTerm($"Team-Trophy-{trophy.trophySTR}");
            
        if(trophyDescription) trophyDescription.GetComponent<Localize>()
            .SetTerm($"Team-Trophy-{trophy.trophySTR}-Description");
        
        if(sprite) trophyImage.sprite = sprite;
    }
}
