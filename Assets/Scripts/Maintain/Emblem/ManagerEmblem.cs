using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LazySoccer.User.Emblem {
    public class ManagerEmblem : MonoBehaviour
    {
        [SerializeField] private DbEmblemGame _listEmblem;

        private int _idActiveEmblem = 1;
        public int IdActiveEmblem
        {
            get { return _idActiveEmblem; }
            set
            {
                _idActiveEmblem = value;
                onActionID?.Invoke(_idActiveEmblem);
                onActionSprite?.Invoke(_listEmblem.Emblems[_idActiveEmblem]);
            }
        }
        public Action<int> onActionID;
        public Action<Sprite> onActionSprite;

        public void GenerationCardEmblem(Transform parents)
        {
            foreach(var emblem in _listEmblem.Emblems.Keys)
            {
                var objects = Instantiate(_listEmblem.BaseContainerEmblem, parents);
                var card = objects.GetComponent<CardEmblem>();
                card.Init(emblem, _listEmblem.Emblems[emblem], emblem == 1 ? true : false);
            }
        }
        public Sprite FirstSprite() => _listEmblem.Emblems[_idActiveEmblem];
    }
}
