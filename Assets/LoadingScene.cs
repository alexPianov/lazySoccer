using Cysharp.Threading.Tasks;
using LazySoccer.Status;
using Sirenix.OdinInspector;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace LazySoccer.SceneLoading
{
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] private DictionaryLoadingScene _dictionaryLoading;
        private bool mobileMode;
        
        [Title("")]
        [SerializeField] private AsyncOperationHandle<SceneInstance> handle;
        
        private bool isFirst = true;

        private void Start()
        {
            if (_dictionaryLoading == null)
                _dictionaryLoading = GetComponent<DictionaryLoadingScene>();

        }

        public void SetMobileMode(bool state)
        {
            mobileMode = state;
        }

        public async UniTask AssetLoaderScene<T>(string nameScene, T endStatus, UniTask action)
        {
            var scene = nameScene;

            if (!mobileMode)
            {
                if (Application.platform == RuntimePlatform.WebGLPlayer ||
                    Application.platform == RuntimePlatform.OSXEditor ||
                    Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.LinuxEditor)
                {
                    scene += "_Web";
                }

                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    var activeScene = SceneManager.GetSceneAt(i);
                        
                    if (activeScene.buildIndex > 0)
                    {
                        Debug.Log("UnloadSceneAsync: " + activeScene.name + " Index: " + activeScene.buildIndex);
                        SceneManager.UnloadSceneAsync(activeScene);
                    }
                }
                
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive)
                    .completed += operation =>
                {
                    Debug.Log("Scene was loaded: " + scene);
                };
                
                return;
            }
            
            if (_dictionaryLoading != null)
            {
                var loadingAsset = _dictionaryLoading.LoadingNameScene(scene);
                await SceneLoading(loadingAsset);
                await action;

                ServiceLocator.GetService<BaseStatus<T>>().StatusAction = endStatus;
            }
        }

        public async UniTask SceneLoading(AssetReference asset)
        {
            if (!isFirst)
            {
                await UnloadScene();
            }
            await SceneLoad(asset);
        }
        private async UniTask SceneLoad(AssetReference asset)
        {
            isFirst = false;
            var handler = Addressables.LoadSceneAsync(asset, LoadSceneMode.Additive, true);
            await handler.Task;
            handler.Completed += SceneLoadCompleted;
        }

        private async UniTask UnloadScene()
        {
            await Addressables.UnloadSceneAsync(handle, true).Task;
        }

        private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                //Debug.Log("Scene Load Completed");
                handle = obj;
            }
        }
    }
}
