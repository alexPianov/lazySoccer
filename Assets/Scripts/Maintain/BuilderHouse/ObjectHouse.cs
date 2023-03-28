using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccet.Building
{
    [CreateAssetMenu(fileName = "House", menuName = "Building/House", order = 1)]
    public class ObjectHouse : ScriptableObject
    {
        [Title("Type House")]
        [SerializeField] private TypeHouse typeHouse;
        public TypeHouse TypeHouse
        {
            get { return typeHouse; }
            private set { typeHouse = value; }
        }
        [SerializeField] private string _nameHouse;
        public string NameHouse
        {
            get { return _nameHouse; }
            private set { _nameHouse = value; }
        }

        private List<Influence> _influences;
        
        public List<Influence> Influences
        {
            get { return _influences; }
            set { _influences = value; }
        }
        
        [Title("Image")]
        public Sprite CurrentSprite
        {
            get { return LevelImage[MaxMinLevel(Level.Value)]; }
            private set { LevelImage[MaxMinLevel(Level.Value)] = value; }
        }
        private int MaxMinLevel(int value) => Mathf.Clamp(value, 1, 20);
        
        [Title("Date level")]
        [SerializeField] private bool isUpgrading;
        public bool IsUpgrading
        {
            get => isUpgrading;
            set => isUpgrading = value;
        }

        [Title("Sprite List")]
        [SerializeField] private List<Sprite> SpriteList;

        [Title("Image Level")]
        [SerializeField] private LevelImage LevelImage;
        public BaseAction<int> IdBuild;
        public BaseAction<DateTime?> DateOfCompletion;
        public BaseAction<DateTime?> DateOfStart;
        public BaseAction<TypeHouse> BuildingType;
        public BaseAction<int> Level;
        public BaseAction<string> Description;
        public BaseAction<int> BuildingLvLId;
        public BaseAction<int?> MaintenanceCost;
        public BaseAction<int?> BuildingCost;
        public BaseAction<int?> CostInstantBuilding;
        public BaseAction<double> BuildTime;
        public BaseAction<bool> InUpgrade;

#if UNITY_EDITOR
        [Button]
        private void AutoAddDictionary()
        {
            var target = 0;
            for (int i = 1; i <= 20; i++)
            {
                if (!LevelImage.ContainsKey(i))
                    if (i % 5 == 0)
                        target++;
                LevelImage.Add(i, SpriteList[target]);
            }
        }
#endif
    }

    [Serializable]
    public class LevelImage : UnitySerializedDictionary<int, Sprite> { }
}
