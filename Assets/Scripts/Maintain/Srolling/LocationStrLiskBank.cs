using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirFishLab.ScrollingList;
using UnityEngine.UI;
using System;

namespace __Project.Scripts.Scroll
{
    public class LocationStrLiskBank : BaseListBank
    {
        [SerializeField]
        private LocationCard[] _contents;

        public override object GetListContent(int index)
        {
            return _contents[index];
        }

        public override int GetListLength()
        {
            return _contents.Length;
        }
    }
    [Serializable]
    public class LocationCard
    {
        public Sprite Image;
        public string Name;
    }
}
