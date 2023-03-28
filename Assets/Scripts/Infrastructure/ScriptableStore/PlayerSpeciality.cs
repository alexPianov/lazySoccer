using System.Collections.Generic;
using UnityEngine;

namespace LazySoccer.SceneLoading.Buildings.OfficePlayer
{
    [CreateAssetMenu(fileName = "Speciality", menuName = "Player/Speciality", order = 1)]
    public class PlayerSpeciality : ScriptableObject
    {
        public List<Speciality> specialities;

        [System.Serializable]
        public class Speciality
        {
            public string specialityName;
            public string specialityAbbreviation;
            public string specialityDescription;
            public string specialityRarity;
        }

        public Speciality GetSpecialityInfo(string specialityName)
        {
            return specialities.Find(speciality => speciality.specialityName == specialityName);
        }
    }
}