using Cysharp.Threading.Tasks;
using LazySoccer.SceneLoading;
using LazySoccer.Status;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ManagerLoading : MonoBehaviour
{
    [SerializeField] private LoadingStatus _loadingStatus;
    private async void Start()
    {
        if (_loadingStatus == null)
            _loadingStatus = ServiceLocator.GetService<LoadingStatus>();
    }
    public async UniTask ActiveteLoading(UniTask events, string NameLoading = "")
    {
        Debug.Log("ActiveteLoading 1");
        _loadingStatus.StatusAction = StatusLoading.Active;
        //loadingText.text = NameLoading;
        await events;
        _loadingStatus.StatusAction = StatusLoading.Deactive;
    }
    public async UniTask<T> ActiveteLoading<T>(UniTask<T> events)
    {
        Debug.Log("ActiveteLoading 2");
        _loadingStatus.StatusAction = StatusLoading.Active;
        var result = await events;
        _loadingStatus.StatusAction = StatusLoading.Deactive;
        return result;
    }
    public void ControlLoading(bool active)
    {
        if (active)
        {
            if(_loadingStatus.StatusAction == StatusLoading.Active) return;
            _loadingStatus.StatusAction = StatusLoading.Active;
        }
        else
        {
            if(_loadingStatus.StatusAction == StatusLoading.Deactive) return;
            _loadingStatus.StatusAction = StatusLoading.Deactive;
        }
    }

}
