using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    [CreateAssetMenu(fileName = "Traits", menuName = "Player/Traits", order = 1)]
    public class PlayerTraits : ScriptableObject
    {
        public List<Trait> traits;

        [System.Serializable]
        public class Trait
        {
            public TraitName traitNameEnum;
            public string traitName;
            public string traitAbbreviation;
            public string traitDescription;
            public string traitRarity;
            public bool traitIsNegative;
        }
        
        public Trait GetTraitInfo(TraitName traitName)
        {
            return traits.Find(trait => trait.traitNameEnum == traitName);
        }
    }
}