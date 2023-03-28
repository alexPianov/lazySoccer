using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazySoccer.SceneLoading
{
    public class DictionaryLoadingScene :MonoBehaviour
    {
        [SerializeField] private List<DictionaryScene> _dictionaryScene;

        public AssetReference LoadingNameScene(string name) => _dictionaryScene.FindLast(x => x.Name == name).Asset;
    }
    [Serializable]
    public class DictionaryScene
    {
        [SerializeField] private string _name;
        public string Name
        {
            get => _name; 
            private set => _name = value; 
        }
        [SerializeField] private AssetReference _asset;
        public AssetReference Asset {

            get => _asset;
            private set => _asset = value;
        }
    }
}

