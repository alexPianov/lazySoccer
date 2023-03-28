using System.Collections;
using System.Collections.Generic;
using LazySoccer.SceneLoading.Buildings.OfficePlayer;
using Sirenix.OdinInspector;
using UnityEngine;

public class OfficePlayerTraits : MonoBehaviour
{
    [SerializeField] private OfficePlayerTraitList traitList;
    [SerializeField] private OfficePlayerTraitSpeciality traitSpeciality;
    [SerializeField] private OfficePlayerTraitStatus playerStatus;
    [SerializeField] private OfficePlayerTraitSkills playerSkills;
    
    [Title("Player Data")]
    [SerializeField] private OfficePlayer officePlayer;
    
    public void UpdateInfo()
    {
        if (officePlayer.playerPosition == null) return;
        
        traitSpeciality.UpdateSpeciality(officePlayer.playerPosition, officePlayer.playerSalary);
        traitList.CreateTraits(officePlayer.playerTraits);
        playerStatus.UpdateStatus(officePlayer.playerStatus);
        playerSkills.CreateSkills(officePlayer.skills);
    }

}
